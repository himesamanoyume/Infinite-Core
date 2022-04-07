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

        GameEventManager.SubscribeEvent(GameEventManager.EVENT_ON_PLAYER_LEVEL_UP, PlayerLevelUp);
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

        Log(actorNumber, "名为" + charBase.PlayerName);
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

            Log(actorNumber, "level已经提升" + count + "级");
        }
    }

    /// <summary>
    /// 通过id使玩家强制立即彻底死亡 【GM命令】
    /// </summary>
    /// <param name="actorNumber"></param>
    public void OnPlayerDead(int actorNumber)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }


        charBase.CurrentHealth = 0;
        charBase.State = StateEnum.Dead;
        Log(actorNumber, "彻底死亡");
    }

    /// <summary>
    /// 通过id使玩家被击杀
    /// </summary>
    /// <param name="actorNumber"></param>
    public void OnPlayerKilled(int actorNumber)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        charBase.State = StateEnum.Respawning;
        charBase.CurrentHealth = 0;
        charBase.Death++;
        
        playerModel.SetActive(false);

        Log(actorNumber, "被击杀");
        OnPlayerRespawnCountDownStart(charBase.ActorNumber);
        
    }

    /// <summary>
    /// 玩家达成了击杀 根据击杀者id增加一次杀敌数
    /// </summary>
    /// <param name="actorNumber"></param>
    public void OnPlayerKill(int actorNumber)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        charBase.Kill++;
        Log(actorNumber, "击杀了敌人");
    }

    /// <summary>
    /// 玩家重生倒计时
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="time"></param>
    public void OnPlayerRespawnCountDownStart(int actorNumber)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        charBase.State = StateEnum.Respawning;
        charBase.RespawnCountDown = charBase.RespawnTime;
        Log(actorNumber, "开始重生倒计时");
    }

    /// <summary>
    /// 通过id直接修改玩家当前生命值
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="health"></param>
    public void SetPlayerHealth(int actorNumber,int health)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        if (health>charBase.MaxHealth)
        {
            Log(actorNumber,"预修改的值不得大于玩家最大生命");
            return;
        }
        if (health<0)
        {
            Log(actorNumber, "预修改的值不得小于0");
            return;
        }
        charBase.CurrentHealth = health;
        Log(actorNumber, "血量已修改");
    }

    /// <summary>
    /// 给玩家经验
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minExp"></param>
    /// <param name="maxExp"></param>
    public void ExpChange(int actorNumber,float minExp,float maxExp)
    {
        if (minExp< 0 || maxExp <0)
        {
            Log(actorNumber, "的经验值变动不能为负数");
            return;
        }

        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        float current = ToGivePlayerSomething(actorNumber, minExp, maxExp);
        charBase.CurrentExp += current;

        Log(actorNumber, "获得了"+ current + "经验");
    }

    /// <summary>
    /// 给予/扣除玩家金钱 最大值和最小值可为负数 即为扣钱
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minMoney"></param>
    /// <param name="maxMoney"></param>
    public void MoneyChange(int actorNumber, int minMoney, int maxMoney)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(actorNumber, minMoney, maxMoney);
        charBase.Money += current;

        if (current>=0)
        {
            Log(actorNumber, "获得了" + current + "金钱");
        }
        else
        {
            Log(actorNumber, "扣除了" + current + "金钱");
        }
    }

    /// <summary>
    /// 给与/扣除玩家血量 数值可为负数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="health"></param>
    public void HealthChange(int actorNumber,int health)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(actorNumber, health, health);
        charBase.CurrentHealth += current;

        if (current >= 0)
        {
            Log(actorNumber, "回复了" + current + "的血量");
        }
        else
        {
            Log(actorNumber, "扣除了" + current + "的血量");
        }
    }

    /// <summary>
    /// 给与/扣除玩家最大血量 数值可为负数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minMaxHealth"></param>
    /// <param name="maxMaxHealth"></param>
    public void MaxHealthChange(int actorNumber, int minMaxHealth, int maxMaxHealth)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(actorNumber, minMaxHealth, maxMaxHealth);
        charBase.MaxHealth += current;
        if (current >= 0)
        {
            Log(actorNumber, "提高了" + current + "的最大生命值");
        }
        else
        {
            Log(actorNumber, "扣除了" + current + "的最大生命值");
        }
    }

    /// <summary>
    /// 给与/扣除玩家护盾值 数值可为负数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="shield"></param>
    public void ShieldChange(int actorNumber,int shield)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(actorNumber, shield, shield);
        charBase.Shield += current;
        if (shield >= 0)
        {
            Log(actorNumber, "回复了" + current + "的护盾值");
        }
        else
        {
            Log(actorNumber, "扣除了" + current + "的护盾值");
        }
    }

    /// <summary>
    /// 给与/扣除玩家爆伤 数值可为负数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minCriticalHit"></param>
    /// <param name="maxCriticalHit"></param>
    public void CriticalHitChange(int actorNumber, int minCriticalHit, int maxCriticalHit)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(actorNumber, minCriticalHit, maxCriticalHit);
        charBase.CriticalHit += current;
        if (current >= 0)
        {
            Log(actorNumber, "回复了" + current + "的爆伤");
        }
        else
        {
            Log(actorNumber, "扣除了" + current + "的爆伤");
        }
    }

    /// <summary>
    /// 给与/扣除玩家暴击率 数值可为负数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minCriticalHitRate"></param>
    /// <param name="maxCriticalHitRate"></param>
    public void CriticalHitRateChange(int actorNumber, int minCriticalHitRate, int maxCriticalHitRate)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(actorNumber, minCriticalHitRate, maxCriticalHitRate);
        charBase.CriticalHitRate += current;
        if (current >= 0)
        {
            Log(actorNumber, "回复了" + current + "的暴击率");
        }
        else
        {
            Log(actorNumber, "扣除了" + current + "的暴击率");
        }
    }

    /// <summary>
    /// 给与/扣除玩家移速 数值可为负数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minMoveSpeedChange"></param>
    /// <param name="maxMoveSpeedChange"></param>
    public void MoveSpeedChange(int actorNumber, int minMoveSpeed, int maxMoveSpeed)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(actorNumber, minMoveSpeed, maxMoveSpeed);
        charBase.MoveSpeed += current;
        if (current >= 0)
        {
            Log(actorNumber, "回复了" + current + "的移速");
        }
        else
        {
            Log(actorNumber, "扣除了" + current + "的移速");
        }
    }

    /// <summary>
    /// 给与/扣除玩家攻速 数值可为负数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minAttackSpeed"></param>
    /// <param name="maxAttackSpeed"></param>
    public void AttackSpeedChange(int actorNumber, int minAttackSpeed, int maxAttackSpeed)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(actorNumber, minAttackSpeed, maxAttackSpeed);
        charBase.AttackSpeed += current;
        if (current >= 0)
        {
            Log(actorNumber, "回复了" + current + "的攻速");
        }
        else
        {
            Log(actorNumber, "扣除了" + current + "的攻速");
        }
    }

    /// <summary>
    /// (float)玩家获取经验,或通过装备随机词条数值等获得加成时的通用函数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="lowerLimit"></param>
    /// <param name="upperLimit"></param>
    /// <returns>返回值用于给装备显示词条数值</returns>
    public float ToGivePlayerSomething(int actorNumber,float lowerLimit,float upperLimit)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return 0; }

        float value = Random.Range(lowerLimit, upperLimit);
        return value;
    }

    /// <summary>
    /// (int)玩家获取经验,或通过装备随机词条数值等获得加成时的通用函数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="lowerLimit"></param>
    /// <param name="upperLimit"></param>
    /// <returns>返回值用于给装备显示词条数值</returns>
    public int ToGivePlayerSomething(int actorNumber, int lowerLimit, int upperLimit)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return 0; }

        int value = Random.Range(lowerLimit, upperLimit);
        return value;
    }


    public void Log(int actorNumber,string text)
    {
        Debug.Log("ActorNumber为" + actorNumber + "的玩家" + text);
    }

    public void Log(string text)
    {
        Debug.Log(text);
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
