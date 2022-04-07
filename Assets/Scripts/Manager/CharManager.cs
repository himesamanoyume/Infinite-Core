using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// 提供对角色数值的控制函数 不附加在玩家物体上
/// </summary>
public class CharManager : MonoBehaviour
{
    public GameObject total;

    public Dictionary<int, GameObject> playerModelList;
    public Dictionary<int, GameObject> recorders;

    public static CharManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerModelList = new Dictionary<int, GameObject>();
        recorders = new Dictionary<int, GameObject>();

        GameEventManager.SubscribeEvent(EventName.onPlayerLevelUp, PlayerLevelUp);
    }

    private void Update()
    {
        GetPlayerModelList();
        GetRecorderList();

       
    }

    /// <summary>
    /// 获取场内所有玩家模型
    /// </summary>
    void GetPlayerModelList()
    {
        if (PhotonNetwork.PlayerList.Length == playerModelList.Count)
        {
            return;
        }
        else
        {
            playerModelList.Clear();
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("PlayerModel");
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                foreach (GameObject obj in gameObjects)
                {

                    if (obj.GetPhotonView().OwnerActorNr == p.ActorNumber)
                    {
                        obj.name = (obj.GetPhotonView().OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)?p.NickName + " (My)": p.NickName;
                        playerModelList.Add(p.ActorNumber, obj);
                        break;
                    }
                }

            }
            Debug.LogError("添加完成");
        }
    }

    /// <summary>
    /// 获取场内所有玩家记录器
    /// </summary>
    void GetRecorderList()
    {
        if (recorders.Count == PhotonNetwork.PlayerList.Length)
        {
            return;
        }
        else
        {
            recorders.Clear();
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("PlayerRecorder");
            foreach (GameObject obj in gameObjects)
            {
                obj.name = (obj.GetPhotonView().OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber) ? obj.GetPhotonView().Owner.NickName + " Recorder (My)" : obj.GetPhotonView().Owner.NickName + " Recorder";

                obj.transform.SetParent(total.transform);

                recorders.Add(obj.GetComponent<CharBase>().ActorNumber, obj);

            }

            Debug.LogWarning("已添加记录者");
        }

    }

    /// <summary>
    /// 通过id获取玩家名字
    /// </summary>
    /// <param name="actorNumber"></param>
    public void GetPlayerNameById(int actorNumber)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber,out GameObject playerModel);
        if (charBase == null) { return; }

        Toast(new object[2] { actorNumber, "名为" + charBase.PlayerName });
    }

    /// <summary>
    /// 通过id使特定玩家升级1级
    /// </summary>
    /// <param name="actorNumber"></param>
    public void PlayerLevelUp(object[] args)
    {
        int actorNumber,count;
        if (args.Length == 2)
        {
            actorNumber = (int)args[0];
            count = (int)args[1];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }


            for (int i = 0; i < count; i++)
            {
                if (charBase.CurrentExp >= charBase.MaxExp)
                {
                    charBase.CurrentExp = charBase.CurrentExp -  charBase.MaxExp;
                }
                charBase.MaxExp += charBase.Level * 500;
                charBase.Level += 1;
                charBase.Attack += 100;
                charBase.MaxHealth += 1000;
                charBase.CurrentHealth += 1000;

                
                charBase.Restore += 10;
            }
            GameEventManager.EnableEvent(EventName.onPlayerLevelUp, false);
            Toast(new object[2] { actorNumber, "level已经提升" + count + "级" });
        }
    }

    /// <summary>
    /// 通过id使玩家强制立即彻底死亡 【GM命令】
    /// </summary>
    /// <param name="actorNumber"></param>
    public void SetPlayerDead(int actorNumber)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }


        charBase.CurrentHealth = 0;
        charBase.State = StateEnum.Dead;

        GameEventManager.EnableEvent(EventName.onPlayerDead, false);
        Toast(new object[2] { actorNumber, "彻底死亡" });
        
    }

    /// <summary>
    /// 通过id使玩家被击杀
    /// </summary>
    /// <param name="actorNumber"></param>
    public void OnPlayerKilled(object[] args)
    {
        int actorNumber;

        if(args.Length == 1)
        {
            actorNumber = (int)args[0];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            charBase.State = StateEnum.Respawning;
            charBase.CurrentHealth = 0;
            charBase.Death++;

            //这句话需要改为全局摧毁
            playerModel.SetActive(false);

            Toast(new object[2] { actorNumber, "被击杀" });
            GameEventManager.EnableEvent(EventName.onPlayerKilled, false);
            GameEventManager.EnableEvent(EventName.onPlayerRespawnCountDownStart, true);
        }     
        //OnPlayerRespawnCountDownStart(charBase.ActorNumber);
        
    }

    /// <summary>
    /// 玩家达成了击杀 根据击杀者id增加一次杀敌数
    /// </summary>
    /// <param name="actorNumber"></param>
    public void OnPlayerKill(object[] args)
    {
        int actorNumber;
        if(args.Length == 1)
        {
            actorNumber = (int)args[0];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            charBase.Kill++;

            GameEventManager.EnableEvent(EventName.onPlayerKill, false);
            Toast(new object[2] { actorNumber, "击杀了敌人" });
        }
    }

    /// <summary>
    /// 玩家回血
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerRestore(object[] args)
    {

    }

    /// <summary>
    /// 玩家处于无核状态下彻底死亡
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerDead(object[] args)
    {
        int actorNumber;
        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            charBase.State = StateEnum.Dead;
            GameEventManager.EnableEvent(EventName.onPlayerDead, false);
            Toast(new object[2] { actorNumber, "彻底死亡" });
        }
    }

    /// <summary>
    /// 玩家重生倒计时
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="time"></param>
    public void OnPlayerRespawnCountDownStart(object[] args)
    {
        int actorNumber;
        if(args.Length == 1)
        {
            actorNumber= (int)args[0];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            charBase.State = StateEnum.Respawning;
            charBase.RespawnCountDown = charBase.RespawnTime;

            GameEventManager.EnableEvent(EventName.onPlayerRespawnCountDownStart, false);
            GameEventManager.EnableEvent(EventName.onPlayerRespawning,true);
            Toast(new object[2] { actorNumber, "开始重生倒计时" });
        }

    }

    public void OnPlayerRespawning(object[] args)
    {
        int actorNumber;

        if( args.Length == 1)
        {
            actorNumber = (int)args[0];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            if (charBase.State == StateEnum.Respawning)
            {
                charBase.RespawnCountDown -= Time.deltaTime;
            }

            if (charBase.RespawnCountDown == 0)
            {
                GameEventManager.EnableEvent(EventName.onPlayerRespawning,false);
                GameEventManager.EnableEvent(EventName.onPlayerRespawnCountDownEnd, true);
            }
        }

    }

    public void OnPlayerRespawnCountDownEnd(object[] args)
    {
        int actorNumber;

        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            charBase.State = StateEnum.Alive;

            GameEventManager.EnableEvent(EventName.onPlayerRespawn, true);
            GameEventManager.EnableEvent(EventName.onPlayerRespawnCountDownEnd, false);
            
        }
    }

    public void OnPlayerRespawn(object[] args)
    {
        int actorNumber;
        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            charBase.CurrentHealth = charBase.MaxHealth;
            //复活逻辑

            //
            GameEventManager.EnableEvent(EventName.onPlayerRespawn, false);
        }
       
    }

    /// <summary>
    /// 通过id直接修改玩家当前生命值
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="health"></param>
    public void SetPlayerHealth(object[] args)
    {
        int actorNumber,health;
        if(args.Length == 2)
        {
            actorNumber = (int)args [0];
            health = (int)args[1];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            if (health > charBase.MaxHealth)
            {
                Toast(new object[2] { actorNumber, "预修改的值不得大于玩家最大生命" });
                return;
            }
            if (health < 0)
            {
                Toast(new object[2] { actorNumber, "预修改的值不得小于0" });
                return;
            }
            charBase.CurrentHealth = health;

            GameEventManager.EnableEvent(EventName.onPlayerCurrentHealthChanged, false);
            Toast(new object[2] { actorNumber, "血量已修改" });
        }
    }

    /// <summary>
    /// 给玩家经验
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minExp"></param>
    /// <param name="maxExp"></param>
    public void ExpChange(object[] args)
    {
        int actorNumber; float minExp, maxExp;

        if(args.Length == 3)
        {
            actorNumber = (int)args[0];
            minExp = (float)args[1];
            maxExp = (float)args[2];

            if (minExp < 0 || maxExp < 0)
            {
                Toast(new object[2] { actorNumber, "的经验值变动不能为负数" });
                return;
            }

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            float current = ToGivePlayerSomething(args);
            charBase.CurrentExp += current;

            GameEventManager.EnableEvent(EventName.onPlayerCurrentExpChanged, false);
            Toast(new object[2] { actorNumber, "获得了" + current + "经验" });
        }

        
    }

    /// <summary>
    /// 给予/扣除玩家金钱 最大值和最小值可为负数 即为扣钱
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minMoney"></param>
    /// <param name="maxMoney"></param>
    public void MoneyChange(object[] args)
    {
        int actorNumber, minMoney, maxMoney;

        if( args.Length == 3)
        {
            actorNumber = (int)args [0];
            minMoney = (int)args[1];
            maxMoney = (int)args[2];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            int current = ToGivePlayerSomething(args);
            charBase.Money += current;

            if (current >= 0)
            {
                Toast(new object[2] { actorNumber, "获得了" + current + "金钱" });
            }
            else
            {
                Toast(new object[2] { actorNumber, "扣除了" + current + "金钱" });
            }
        }

    }

    /// <summary>
    /// 给与/扣除玩家血量 数值可为负数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="health"></param>
    public void HealthChange(object[] args)
    {
        int actorNumber, health;

        if(args.Length == 2)
        {
            actorNumber = (int)args[0];
            health = (int)args[1];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            int current = ToGivePlayerSomething(new object[] { actorNumber, health, health });
            charBase.CurrentHealth += current;

            if (current >= 0)
            {
                Toast(new object[2] { actorNumber, "回复了" + current + "的血量" });
            }
            else
            {
                Toast(new object[2] { actorNumber, "扣除了" + current + "的血量" });
            }
        }
    }

    /// <summary>
    /// 给与/扣除玩家最大血量 数值可为负数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minMaxHealth"></param>
    /// <param name="maxMaxHealth"></param>
    public void MaxHealthChange(object[] args)
    {
        int actorNumber, minMaxHealth, maxMaxHealth;

        if(args.Length == 3)
        {
            actorNumber = (int)args[0];
            minMaxHealth = (int)args[1];
            maxMaxHealth = (int)args[2];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            int current = ToGivePlayerSomething(args);
            charBase.MaxHealth += current;
            if (current >= 0)
            {
                Toast(new object[2] { actorNumber, "提高了" + current + "的最大生命值" });
            }
            else
            {
                Toast(new object[2] { actorNumber, "扣除了" + current + "的最大生命值" });
            }
        }

        
    }

    /// <summary>
    /// 给与/扣除玩家护盾值 数值可为负数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="shield"></param>
    public void ShieldChange(object[] args)
    {
        int actorNumber, shield;

        if(args.Length == 2)
        {
            actorNumber = (int)args[0];
            shield = (int)args[1];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            int current = ToGivePlayerSomething(new object[] { actorNumber, shield, shield });
            charBase.Shield += current;
            if (shield >= 0)
            {
                Toast(new object[2] { actorNumber, "回复了" + current + "的护盾值" });
            }
            else
            {
                Toast(new object[2] { actorNumber, "扣除了" + current + "的护盾值" });
            }
        } 
    }

    /// <summary>
    /// 给与/扣除玩家爆伤 数值可为负数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minCriticalHit"></param>
    /// <param name="maxCriticalHit"></param>
    public void CriticalHitChange(object[] args)
    {
        int actorNumber, minCriticalHit, maxCriticalHit;

        if(args.Length == 3)
        {
            actorNumber = (int)args[0];
            minCriticalHit = (int)args[1];
            maxCriticalHit = (int)args[2];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            int current = ToGivePlayerSomething(args);
            charBase.CriticalHit += current;
            if (current >= 0)
            {
                Toast(new object[2] { actorNumber, "回复了" + current + "的爆伤" });
            }
            else
            {
                Toast(new object[2] { actorNumber, "扣除了" + current + "的爆伤" });
            }
        }
    }

    /// <summary>
    /// 给与/扣除玩家暴击率 数值可为负数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minCriticalHitRate"></param>
    /// <param name="maxCriticalHitRate"></param>
    public void CriticalHitRateChange(object[] args)
    {
        int actorNumber, minCriticalHitRate, maxCriticalHitRate;
        if (args.Length == 3)
        {
            actorNumber = (int)args[0];
            minCriticalHitRate = (int)args[1];
            maxCriticalHitRate = (int)args[2];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            int current = ToGivePlayerSomething(args);
            charBase.CriticalHitRate += current;
            if (current >= 0)
            {
                Toast(new object[2] { actorNumber, "回复了" + current + "的暴击率" });
            }
            else
            {
                Toast(new object[2] { actorNumber, "扣除了" + current + "的暴击率" });
            }
        }


    }

    /// <summary>
    /// 给与/扣除玩家移速 数值可为负数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minMoveSpeedChange"></param>
    /// <param name="maxMoveSpeedChange"></param>
    public void MoveSpeedChange(object[] args)
    {
        int actorNumber, minMoveSpeed, maxMoveSpeed;

        if(args.Length == 3)
        {
            actorNumber = (int)args[0];
            minMoveSpeed = (int)args[1];
            maxMoveSpeed = (int)args[2];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            int current = ToGivePlayerSomething(args);
            charBase.MoveSpeed += current;
            if (current >= 0)
            {
                Toast(new object[2] { actorNumber, "回复了" + current + "的移速" });
            }
            else
            {
                Toast(new object[2] { actorNumber, "扣除了" + current + "的移速" });
            }
        }

    }

    /// <summary>
    /// 给与/扣除玩家攻速 数值可为负数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minAttackSpeed"></param>
    /// <param name="maxAttackSpeed"></param>
    public void AttackSpeedChange(object[] args)
    {
        int actorNumber, minAttackSpeed, maxAttackSpeed;

        if(args.Length == 3)
        {
            actorNumber = (int)args[0];
            minAttackSpeed = (int)args[1];
            maxAttackSpeed = (int)args[2];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            int current = ToGivePlayerSomething(args);
            charBase.AttackSpeed += current;
            if (current >= 0)
            {
                Toast(new object[2] { actorNumber, "回复了" + current + "的攻速" });
            }
            else
            {
                Toast(new object[2] { actorNumber, "扣除了" + current + "的攻速" });
            }
        }

        
    }

    /// <summary>
    /// (int)玩家获取经验,或通过装备随机词条数值等获得加成时的通用函数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="lowerLimit"></param>
    /// <param name="upperLimit"></param>
    /// <returns>返回值用于给装备显示词条数值</returns>
    public int ToGivePlayerSomething(object[] args)
    {
        int actorNumber, lowerLimit, upperLimit;

        if(args.Length == 3)
        {
            actorNumber = (int)args[0];
            lowerLimit = (int)args[1];
            upperLimit = (int)args[2];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return 0; }

            int value = Random.Range(lowerLimit, upperLimit);
            return value;
        }
        else
        {
            Toast(new object[1] { "随机数值参数错误" });
            return 0;
        }
        
    }

    /// <summary>
    /// 吐丝
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="text"></param>
    public void Toast(object[] args)
    {
        int actorNumber;
        string text;
        if (args.Length == 2)
        {
            actorNumber = (int)args[0];
            text = args[1].ToString();
            Debug.Log("ActorNumber为" + actorNumber + "的玩家" + text);
        }
        if (args.Length == 1)
        {
            text = args[0].ToString();
            Debug.Log(text);
        }
        GameEventManager.EnableEvent(EventName.onToast, false);
    }

    /// <summary>
    /// 通过ActorNumber获取特定玩家的组件,适用于将要进行特定属性进行修改时
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <returns></returns>
    public CharBase FindPlayerByActorNumber(int actorNumber,out GameObject playerModel)
    {

        CharBase charBase;

        foreach (var recorder in recorders)
        {
            charBase = recorder.Value.GetComponent<CharBase>();
            if (charBase.ActorNumber == actorNumber)
            {
                
                playerModel = playerModelList[charBase.ActorNumber];
                return charBase;
            }
            
        }
        Debug.Log("未得到指定actorNumber的玩家");
        playerModel = null;
        return null;
    }

    /// <summary>
    /// 通过tag找父物体的某个单个子物体
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public GameObject FindChildObjWithTag(string tag,GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
        }
        return null;
    }

    /// <summary>
    /// 根据CharBase组件获取所有信息
    /// </summary>
    /// <param name="needTarget"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public void GetPlayerInfo(CharBase needTarget,CharBase provider)
    {

        needTarget.ActorNumber = provider.ActorNumber;
        needTarget.PlayerName = provider.PlayerName;
        needTarget.Kill = provider.Kill;
        needTarget.Death = provider.Death;
        needTarget.Money = provider.Money;
        needTarget.CurrentExp = provider.CurrentExp;
        needTarget.MaxExp = provider.MaxExp;
        needTarget.State = provider.State;
        //needTarget.Buff = provider.Buff;
        needTarget.Pro = provider.Pro;
        needTarget.Level = provider.Level;
        needTarget.Attack = provider.Attack;
        needTarget.MaxHealth = provider.MaxHealth;
        needTarget.CurrentHealth = provider.CurrentHealth;
        needTarget.CriticalHit = provider.CriticalHit;
        needTarget.CriticalHitRate = provider.CriticalHitRate;
        needTarget.Defence = provider.Defence;
        needTarget.AttackSpeed = provider.AttackSpeed;
        needTarget.Restore = provider.Restore;
        needTarget.SkillQ = provider.SkillQ;
        needTarget.SkillE = provider.SkillE;
        needTarget.SkillR = provider.SkillR;
        needTarget.SkillBurst = provider.SkillBurst;
        needTarget.HeadID = provider.HeadID;
        needTarget.ArmorID = provider.ArmorID;
        needTarget.HeadID = provider.HeadID;
        needTarget.KneeID = provider.KneeID;
        needTarget.TrousersID = provider.TrousersID;
        needTarget.BootsID = provider.BootsID;
        needTarget.MoveSpeed = provider.MoveSpeed;
        needTarget.AttackRange = provider.AttackRange;
        needTarget.RespawnTime = provider.RespawnTime;
        needTarget.RespawnCountDown = provider.RespawnCountDown;

        return;
    }

}
