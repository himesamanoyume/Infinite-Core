using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// 提供对角色数值的控制函数 不附加在玩家物体上
/// </summary>
public class CharManager : MonoBehaviourPunCallbacks
{
    public GameObject archer;
    public GameObject doctor;
    public GameObject solider;
    public GameObject tanker;
    public GameObject recorder;
    public GameObject playerInfoBarPrefab;
    public Transform playerInfoCanvas;

    public GameObject[] redPosList = new GameObject[5];
    public GameObject[] bluePosList = new GameObject[5];

    public Dictionary<int, GameObject> playerModelList;
    public Dictionary<int, GameObject> recorders;
    public Dictionary<int, GameObject> playerInfoBarList;

    public static CharManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }

    }

    private void Start()
    {
        playerModelList = new Dictionary<int, GameObject>();
        recorders = new Dictionary<int, GameObject>();
        playerInfoBarList = new Dictionary<int, GameObject>();

        SpawnPlayer(PhotonNetwork.LocalPlayer.ActorNumber);

        GameEventManager.EnableEvent(EventEnum.CharMgrGroup, true);
        GameEventManager.EnableEvent(EventEnum.PlayerGroup, true);
        GameEventManager.EnableEvent(EventEnum.PlayerControlGroup, true);

        #region Subscribe Event

        GameEventManager.SubscribeEvent(EventEnum.OnPlayerLevelUp, OnPlayerLevelUp);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerCurrentHealthChanged, OnPlayerCurrentHealthChanged);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerKilled, OnPlayerKilled);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerKill, OnPlayerKill);
        GameEventManager.SubscribeEvent(EventEnum.OnToast, Toast);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerRespawnCountDownStart, OnPlayerRespawnCountDownStart);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerRespawning, OnPlayerRespawning);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerRespawnCountDownEnd, OnPlayerRespawnCountDownEnd);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerRespawn, OnPlayerRespawn);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerRestoreing, OnPlayerRestoreing);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerRestoreChanged, OnPlayerRestoreChanged);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerLevelChanged, OnPlayerLevelChanged);
        GameEventManager.SubscribeEvent(EventEnum.AllowGetPlayerModelList, GetPlayerModelList);
        GameEventManager.SubscribeEvent(EventEnum.AllowGetRecorderList, GetRecorderList);
        GameEventManager.SubscribeEvent(EventEnum.AllowGetPlayerInfoBarList, GetPlayerInfoBarList);

        #endregion
    }

    #region Misc Functions

    delegate void SpawnPlayerDelegate(ProEnum pro, out GameObject playerModel);
    SpawnPlayerDelegate spawnPlayerDelegate;

    void SpawnPosRed(ProEnum pro, out GameObject playerModel)
    {
        playerModel = PhotonNetwork.Instantiate(pro.ToString(), redPosList[Random.Range(0, 5)].transform.position, Quaternion.identity);
    }

    void SpawnPosBlue(ProEnum pro, out GameObject playerModel)
    {
        playerModel = PhotonNetwork.Instantiate(((ProEnum)pro).ToString(), bluePosList[Random.Range(0, 5)].transform.position, Quaternion.identity);
    }

    void SpawnPlayerChild(Player player, SpawnPlayerDelegate spawnPlayerDelegate)
    {
        object pro;
        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_PRO, out pro);

        if (recorders.TryGetValue(player.ActorNumber, out GameObject playerRecorder))
        {

        }
        else
        {
            playerRecorder = PhotonNetwork.Instantiate("PlayerDataRecorder", new Vector3(100, 100, 100), Quaternion.identity);
            Debug.LogWarning(player.ActorNumber + " 记录器已生成");
        }

        spawnPlayerDelegate((ProEnum)pro, out GameObject playerModel);

        playerModel.name = player.NickName + " (My)";
        playerRecorder.name = player.NickName + " Recorder {My}";

        GetPlayerInfo(playerRecorder.GetComponent<CharBase>(), player);

        
    }

    /// <summary>
    /// 首次生成玩家
    /// </summary>
    /// <param name="player"></param>
    public void SpawnPlayer(int actorNumber)
    {
        //player只能是自己
        
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if(player.ActorNumber == actorNumber)
            {
                if (player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_TEAM, out object team))
                {
                    if ((TeamEnum)team == TeamEnum.Null) return;

                    switch ((TeamEnum)team)
                    {
                        case TeamEnum.Red:

                            spawnPlayerDelegate = SpawnPosRed;
                            SpawnPlayerChild(player, spawnPlayerDelegate);
                            break;
                        case TeamEnum.Blue:

                            spawnPlayerDelegate = SpawnPosBlue;
                            SpawnPlayerChild(player, spawnPlayerDelegate);
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 根据玩家属性获取所有信息
    /// </summary>
    /// <param name="needTarget"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    public void GetPlayerInfo(CharBase needTarget, Player player)
    {

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_ACTOR_NUMBER, out object actorNumber);
        needTarget.ActorNumber = (int)actorNumber;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_NAME, out object playerName);
        needTarget.PlayerName = (string)playerName;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_TEAM, out object team);
        needTarget.PlayerTeam = (TeamEnum)team;

        needTarget.State = StateEnum.Alive;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_KILL, out object kill);
        //needTarget.Kill = (int)kill;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_DEATH, out object death);
        //needTarget.Death = (int)death;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_MONEY, out object money);
        //needTarget.Money = (int)money;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_CURRENT_EXP , out object currentExp);
        //needTarget.CurrentExp = (float)currentExp;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_MAX_EXP , out object maxExp);
        //needTarget.MaxExp = (float)maxExp;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_STATE , out object state);
        //needTarget.State = (StateEnum)state;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_BUFF , out object buff);
        //needTarget.Buff = (BuffEnum[])buff;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_PRO, out object pro);
        needTarget.Pro = (ProEnum)pro;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_LEVEL , out object level);
        //needTarget.Level = (int)level;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_ATTACK, out object attack);
        needTarget.Attack = (float)attack;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_MAX_HEALTH, out object maxHealth);
        needTarget.MaxHealth = (float)maxHealth;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_CURRENT_HEALTH, out object currentHealth);
        needTarget.CurrentHealth = (float)currentHealth;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_CRITICAL_HIT, out object criticalHit);
        needTarget.CriticalHit = (float)criticalHit;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_CRITICAL_HIT_RATE, out object criticalHitRate);
        needTarget.CriticalHitRate = (float)criticalHitRate;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_DEFENCE, out object defence);
        needTarget.Defence = (float)defence;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_ATTACK_SPEED, out object attackSpeed);
        needTarget.AttackSpeed = (float)attackSpeed;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_RESTORE, out object restore);
        needTarget.Restore = (float)restore;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_SKILL_Q, out object skillQ);
        needTarget.SkillQ = (SkillQ)skillQ;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_SKILL_E, out object skillE);
        needTarget.SkillE = (SkillE)skillE;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_SKILL_R, out object skillR);
        needTarget.SkillR = (SkillR)skillR;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_SKILL_BURST, out object skillBurst);
        needTarget.SkillBurst = (SkillBurst)skillBurst;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_HEAD_ID , out object headId);
        //needTarget.HeadID = (EquipHead)headId;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_ARMOR_ID , out object armorId);
        //needTarget.ArmorID = (EquipArmor)armorId;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_HAND_ID , out object handId);
        //needTarget.HandID = (EquipHand)handId;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_KNEE_ID , out object kneeId);
        //needTarget.KneeID = (EquipKnee)kneeId;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_TROUSERS_ID , out object trousersId);
        //needTarget.TrousersID = (EquipTrousers)trousersId;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_BOOTS , out object bootsId);
        //needTarget.BootsID = (EquipBoots)bootsId;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_MOVE_SPEED, out object moveSpeed);
        needTarget.MoveSpeed = (float)moveSpeed;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_ATTACK_RANGE, out object attackRange);
        needTarget.AttackRange = (float)attackRange;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_RESPAWN_TIME , out object respawnTime);
        //needTarget.RespawnTime = (float)respawnTime;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_RESPAWN_COUNTDOWN , out object respawnCountDown);
        //needTarget.RespawnCountDown = (float)respawnCountDown;

        return;
    }

    /// <summary>
    /// 通过id获取玩家名字
    /// </summary>
    /// <param name="actorNumber"></param>
    public void GetPlayerName(int actorNumber)
    {
        FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
        if (charBase == null || recorder == null) { return; }

        FindPlayerModel(actorNumber, out GameObject playerModel);
        if (playerModel == null) { return; }

        Toast(new object[2] { actorNumber, "名为" + charBase.PlayerName });
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
        int lowerLimit, upperLimit;

        if (args.Length == 2)
        {
            lowerLimit = (int)args[0];
            upperLimit = (int)args[1];

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
        GameEventManager.EnableEvent(EventEnum.OnToast, false);
    }

    /// <summary>
    /// 通过ActorNumber获取特定玩家的记录器,适用于将要进行特定属性进行修改时
    /// </summary>
    /// <param name="actorNumber"></param>
    public void FindPlayerRecorder(int actorNumber, out GameObject playerRecorder, out CharBase charBase)
    {

        if (recorders == null)
        {
            Toast(new object[2] { actorNumber, "玩家记录器获取失败, recorders为空" });
            playerRecorder = null;
            charBase = null;
        }

        if (recorders.TryGetValue(actorNumber, out GameObject recorder))
        {
            charBase = recorder.GetComponent<CharBase>();
            playerRecorder = recorder;
            Toast(new object[2] { actorNumber, "玩家记录器获取成功" });
        }
        else
        {
            Toast(new object[2] { actorNumber, "玩家记录器获取失败" });
            charBase = null;
            playerRecorder = null;
        }

    }

    /// <summary>
    /// 获取对应玩家模型
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="playerModel"></param>
    public void FindPlayerModel(int actorNumber, out GameObject playerModel)
    {
        if (playerModelList == null)
        {
            Toast(new object[2] { actorNumber, "玩家模型获取失败, playerModelList为空" });

            playerModel = null;
        }

        if (playerModelList.TryGetValue(actorNumber, out GameObject model))
        {

            Toast(new object[2] { actorNumber, "玩家模型获取成功" });
            playerModel = model;
        }
        else
        {
            Toast(new object[2] { actorNumber, "玩家模型获取失败" });
            playerModel = null;
        }

    }

    public void FindPlayerInfoBar(int actorNumber, out GameObject playerInfoBar)
    {
        if (playerInfoBarList == null)
        {
            Toast(new object[2] { actorNumber, "玩家信息条获取失败, playerInfoBarList为空" });
            playerInfoBar = null;
        }

        if (playerInfoBarList.TryGetValue(actorNumber, out GameObject infoBar))
        {
            playerInfoBar = infoBar;
            Toast(new object[2] { actorNumber, "玩家信息条获取成功" });
        }
        else
        {
            playerInfoBar = null;
            Toast(new object[2] { actorNumber, "玩家信息条获取失败" });
        }
    }


    /// <summary>
    /// 通过tag找父物体的某个单个子物体
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public GameObject FindChildObjWithTag(string tag, GameObject parent)
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
    public void GetPlayerInfo(CharBase needTarget, CharBase provider)
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

    #endregion

    #region Event Response

    public void GetPlayerInfoBarList(object[] args)
    {
        GameObject[] infoBars = GameObject.FindGameObjectsWithTag("PlayerInfoBar");

        if (infoBars.Length == playerModelList.Count - 1) return;


        foreach (var recorder in recorders)
        {
            //对每个记录器获取charBase
            CharBase charBase = recorder.Value.GetComponent<CharBase>();
            //如果ActorNumber为自身时 跳过此次循环
            if (charBase.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber) continue;
            //为其他玩家的记录器时 如果状态为存活 查找表中是否已有生成过的信息条
            if (charBase.State == StateEnum.Alive)
            {
                FindPlayerInfoBar(charBase.ActorNumber, out GameObject t_playerInfoBar);
                //如果为空 则拿去对应的Recorder和PlayerModel 进行Init
                if (t_playerInfoBar == null)
                {
                    playerInfoBarList.Remove(charBase.ActorNumber);
                    Debug.LogWarning(charBase.ActorNumber + "没有信息条 正在生成");
                    FindPlayerRecorder(charBase.ActorNumber, out GameObject playerRecorder, out CharBase t_charBase);

                    FindPlayerModel(charBase.ActorNumber, out GameObject obj);

                    t_playerInfoBar = Instantiate(playerInfoBarPrefab);
                    t_playerInfoBar.transform.SetParent(playerInfoCanvas);
                    t_playerInfoBar.GetComponent<PlayerInfoBar>().InitPlayerInfoBar(playerRecorder, obj);

                    t_playerInfoBar.name = charBase.PlayerName + " InfoBar";

                    //对生成过的玩家的信息条添加记录进字典
                    playerInfoBarList.Add(charBase.ActorNumber, t_playerInfoBar);
                }

            }
            else
            {
                playerInfoBarList.Remove(charBase.ActorNumber);
                Debug.LogWarning(charBase.ActorNumber + "死亡 已Remove该Key");
            }
        }

    }

    /// <summary>
    /// 获取场内所有玩家模型
    /// </summary>
    public void GetPlayerModelList(object[] args)
    {
        int alivePlayerCount = 0;
        if (recorders == null) return;
        foreach (var recorder in recorders)
        {

            if (recorder.Value.GetComponent<CharBase>().State == StateEnum.Alive)
            {
                alivePlayerCount++;
            }
        }
        //Debug.Log(alivePlayerCount);

        if (alivePlayerCount == playerModelList.Count) return;

        playerModelList.Clear();

        if (PhotonNetwork.PlayerList.Length == playerModelList.Count)
        {
            return;
        }
        else
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("PlayerModel");
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                foreach (GameObject obj in gameObjects)
                {
                    if (obj.GetPhotonView().OwnerActorNr == p.ActorNumber)
                    {
                        obj.name = (obj.GetPhotonView().OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber) ? p.NickName + " (My)" : p.NickName;
                        playerModelList.Add(p.ActorNumber, obj);
                        break;
                    }
                }

            }
            if (alivePlayerCount == playerModelList.Count)
            {
                Debug.LogWarning("已添加完成全部存活的玩家模型");
            }
            else
            {
                Debug.LogWarning("人数不足");
            }

        }
    }

    /// <summary>
    /// 获取场内所有玩家记录器
    /// </summary>
    public void GetRecorderList(object[] args)
    {
        //Debug.Log("有在执行GetRecorderList");
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

                //obj.GetComponent<RecorderController>().SetParents(transform.parent.gameObject);

                recorders.Add(obj.GetComponent<CharBase>().ActorNumber, obj);
            }

            Debug.LogWarning("已添加记录者");
        }

    }

    /// <summary>
    /// 【事件回应】当玩家等级变动时再进行一次检测
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerLevelChanged(object[] args)
    {
        int actorNumber;
        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            FindPlayerRecorder(actorNumber, out GameObject recorder, out CharBase charBase);
            if (charBase == null || recorder == null)  return; 

            if (charBase.CurrentExp >= charBase.MaxExp)
            {
                GameEventManager.EnableEvent(EventEnum.OnPlayerLevelUp, true);
            }
            GameEventManager.EnableEvent(EventEnum.OnPlayerLevelChanged, false);
        }
    }

    /// <summary>
    /// 【事件回应】玩家升级
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerLevelUp(object[] args)
    {
        int actorNumber;
        if (args.Length == 1)
        {
            actorNumber = (int)args[0];
           
            FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            if (charBase.CurrentExp >= charBase.MaxExp)
            {
                charBase.CurrentExp -=  charBase.MaxExp;
            }
            charBase.MaxExp += charBase.Level * 50;
            charBase.Level += 1;
            charBase.Attack += 100;
            charBase.MaxHealth += 1000;
            charBase.CurrentHealth += 1000;
            charBase.RespawnTime += 5;   
            charBase.Restore += 2;
            
            GameEventManager.EnableEvent(EventEnum.OnPlayerLevelUp, false);
            Toast(new object[2] { actorNumber, "level已经提升1级" });
        }
    }

    /// <summary>
    /// 【事件回应】通过id使玩家被击杀
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerKilled(object[] args)
    {
        int actorNumber;
        int lastOneHurtActorNumber;
        if (args.Length == 2)
        {
            actorNumber = (int)args[0];
            lastOneHurtActorNumber = (int)args[1];

            FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            FindPlayerModel(actorNumber,out GameObject playerModel);
            if (playerModel == null) { return; }

            //if (actorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            //{
            //    FindPlayerInfoBar(actorNumber, out GameObject playerInfoBar);
            //    if (playerInfoBar == null) { return; }
            //    Debug.LogWarning(playerInfoBar.name);
            //    Destroy(playerInfoBar);
            //    playerInfoBarList.Remove(actorNumber);
            //}

            charBase.Death++;

            PhotonNetwork.Destroy(playerModel);
            playerModelList.Remove(actorNumber);
            playerInfoBarList.Remove(actorNumber);

            Toast(new object[2] { actorNumber, "被击杀" });
            GameEventManager.EnableEvent(EventEnum.PlayerControlGroup, false);
            GameEventManager.EnableEvent(EventEnum.OnPlayerKilled, false);
            GameEventManager.EnableEvent(EventEnum.OnPlayerRespawnCountDownStart, true);
        }     

    }

/// <summary>
    /// 【事件回应】玩家达成了击杀 根据击杀者id增加一次杀敌数
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerKill(object[] args)
    {
        int actorNumber;
        if(args.Length == 1)
        {
            actorNumber = (int)args[0];

            FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            charBase.Kill++;

            GameEventManager.EnableEvent(EventEnum.OnPlayerKill, false);
            Toast(new object[2] { actorNumber, "击杀了敌人" });
        }
    }

    /// <summary>
    /// 【事件回应】玩家回血
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerRestoreing(object[] args)
    {
        int actorNumber;

        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            charBase.CurrentHealth += charBase.Restore * Time.deltaTime;

            

            if (charBase.CurrentHealth == charBase.MaxHealth)
            {
                GameEventManager.EnableEvent(EventEnum.OnPlayerRestoreing, false);
                Toast(new object[2] { actorNumber, "回血停止" });
            }
            else
            {
                //---
                Toast(new object[2] { actorNumber, "正在回血" });
                //---
            }
        }     
    }

    /// <summary>
    /// 【事件回应】当玩家回血值变化时
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerRestoreChanged(object[] args)
    {
        int actorNumber;

        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            Toast(new object[2] { actorNumber, "回复值改变" });

            GameEventManager.EnableEvent(EventEnum.OnPlayerRestoreChanged, false);
        }
    }

    /// <summary>
    /// 【事件回应】玩家处于无核状态下彻底死亡
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerDead(object[] args)
    {
        int actorNumber;
        int lastOneHurtActorNumber;
        if (args.Length == 2)
        {
            actorNumber = (int)args[0];
            lastOneHurtActorNumber = (int)args[1];

            FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            FindPlayerModel(actorNumber, out GameObject playerModel);
            if(playerModel == null) { return; }

            //if (actorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            //{
            //    FindPlayerInfoBar(actorNumber, out GameObject playerInfoBar);
            //    if (playerInfoBar == null) { return; }

            //    Destroy(playerInfoBar);
            //    playerInfoBarList.Remove(actorNumber);
            //}

            charBase.Death++;

            PhotonNetwork.Destroy(playerModel);
            playerModelList.Remove(actorNumber);
            playerInfoBarList.Remove(actorNumber);

            charBase.State = StateEnum.Dead;
            GameEventManager.EnableEvent(EventEnum.PlayerControlGroup, false);

            GameEventManager.EnableEvent(EventEnum.OnPlayerDead, false);
            Toast(new object[2] { actorNumber, "彻底死亡" });
        }
    }

    /// <summary>
    /// 【事件回应】玩家重生倒计时开始
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerRespawnCountDownStart(object[] args)
    {
        int actorNumber;
        if(args.Length == 1)
        {
            actorNumber= (int)args[0];

            FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            charBase.State = StateEnum.Respawning;
            charBase.RespawnCountDown = charBase.RespawnTime;

            GameEventManager.EnableEvent(EventEnum.OnPlayerRespawnCountDownStart, false);
            GameEventManager.EnableEvent(EventEnum.OnPlayerRespawning,true);
            Toast(new object[2] { actorNumber, "开始重生倒计时" });
        }

    }

    /// <summary>
    /// 【事件回应】玩家正在复活
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerRespawning(object[] args)
    {
        int actorNumber;
        float respawnCountDown;
        if ( args.Length == 2)
        {
            actorNumber = (int)args[0];
            respawnCountDown = (float)args[1];

            FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            if (charBase.State == StateEnum.Respawning)
            {
                //----
                charBase.RespawnCountDown -= Time.deltaTime;
                //---
                Toast(new object[2] { actorNumber, "正在复活" });
            }

            if (charBase.RespawnCountDown <= 0)
            {
                
                GameEventManager.EnableEvent(EventEnum.OnPlayerRespawning,false);
                GameEventManager.EnableEvent(EventEnum.OnPlayerRespawnCountDownEnd, true);
            }
        }

    }

    /// <summary>
    /// 【事件回应】玩家复活倒计时结束
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerRespawnCountDownEnd(object[] args)
    {
        int actorNumber;

        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            charBase.RespawnCountDown = 0;
            charBase.State = StateEnum.Alive;

            Toast(new object[2] { actorNumber, "重生倒计时结束" });
            GameEventManager.EnableEvent(EventEnum.OnPlayerRespawn, true);
            GameEventManager.EnableEvent(EventEnum.OnPlayerRespawnCountDownEnd, false);
            
        }
    }

    /// <summary>
    /// 【事件回应】玩家复活
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerRespawn(object[] args)
    {
        int actorNumber;
        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            if (actorNumber != PhotonNetwork.LocalPlayer.ActorNumber) return;

            FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            charBase.State = StateEnum.Alive;

            SpawnPlayer(PhotonNetwork.LocalPlayer.ActorNumber);

            charBase.CurrentHealth = charBase.MaxHealth;

            Toast(new object[] { actorNumber, "已复活" });


            GameEventManager.EnableEvent(EventEnum.PlayerControlGroup, true);

            GameEventManager.EnableEvent(EventEnum.OnPlayerRespawn, false);
        }
       
    }

    /// <summary>
    /// 【事件回应】当玩家当前经验变化时
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerCurrentExpChanged(object[] args)
    {
        int actorNumber;
        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
            if (charBase == null || recorder == null) { return; }


            Toast(new object[] { actorNumber, "当前经验已改变" });

            GameEventManager.EnableEvent(EventEnum.OnPlayerCurrentExpChanged, false);
        }
    }

    /// <summary>
    /// 【事件回应】给与/扣除玩家护盾值 数值可为负数
    /// </summary>
    /// <param name="actorNumber"></param>
    public void OnPlayerShieldChanged(object[] args)
    {
        int actorNumber;

        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            FindPlayerRecorder(actorNumber, out GameObject recorder, out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            GameEventManager.EnableEvent(EventEnum.OnPlayerShieldChanged, false);

            Toast(new object[2] { actorNumber, "盾值变化了" });

        }
    }

    /// <summary>
    /// 【事件回应】当玩家爆伤变化时
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerCriticalHitChanged(object[] args)
    {
        int actorNumber;

        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            FindPlayerRecorder(actorNumber, out GameObject recorder, out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            GameEventManager.EnableEvent(EventEnum.OnPlayerCriticalHitChanged, false);

            Toast(new object[2] { actorNumber, "的爆伤变化了" });

        }
    }

    /// <summary>
    /// 【事件回应】当玩家爆击率变化时
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerCriticalHitRateChanged(object[] args)
    {
        int actorNumber;
        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            FindPlayerRecorder(actorNumber, out GameObject recorder, out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            GameEventManager.EnableEvent(EventEnum.OnPlayerCriticalHitRateChanged, false);
            Toast(new object[2] { actorNumber, "的暴击率变化了" });
        }
    }

    /// <summary>
    /// 【事件回应】当玩家移速变化时
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerMoveSpeedChanged(object[] args)
    {
        int actorNumber;

        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            FindPlayerRecorder(actorNumber, out GameObject recorder, out CharBase charBase);
            if (charBase == null || recorder == null) { return; }


            GameEventManager.EnableEvent(EventEnum.OnPlayerMoveSpeedChanged, false);
            Toast(new object[2] { actorNumber, "的移速变化了" });
        }

    }

    /// <summary>
    /// 【事件回应】当玩家攻速变化时
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerAttackSpeedChanged(object[] args)
    {
        int actorNumber;

        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            FindPlayerRecorder(actorNumber, out GameObject recorder, out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            Toast(new object[2] { actorNumber, "的攻速变化了" });
        }

    }

    /// <summary>
    /// 【事件回应】当玩家金钱变化时
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerMoneyChanged(object[] args)
    {
        int actorNumber;

        if( args.Length == 1)
        {
            actorNumber = (int)args[0];
            
            FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            GameEventManager.EnableEvent(EventEnum.OnPlayerMoneyChanged, false);
            Toast(new object[2] { actorNumber, "金钱改变了"});
        }

    }

    /// <summary>
    /// 【事件回应】当玩家当前血量改变时
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="health"></param>
    public void OnPlayerCurrentHealthChanged(object[] args)
    {
        int actorNumber;

        if(args.Length == 1)
        {
            actorNumber = (int)args[0];

            FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            GameEventManager.EnableEvent(EventEnum.OnPlayerCurrentHealthChanged, false);
            Toast(new object[2] { actorNumber, "血量改变了" });
           
        }
    }

    /// <summary>
    /// 【事件回应】当玩家最大血量变化时
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerMaxHealthChanged(object[] args)
    {
        int actorNumber;
        if(args.Length == 1)
        {
            actorNumber = (int)args[0];

            FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            GameEventManager.EnableEvent(EventEnum.OnPlayerMaxHealthChanged, false);
            Toast(new object[2] { actorNumber, "最大血量改变了"  });
        }
    }

    #endregion

    #region Non Event Response

    /// <summary>
    /// 【非事件回应】给玩家经验
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minExp"></param>
    /// <param name="maxExp"></param>
    public void ToGivePlayerCurrentExp(int actorNumber, int minExp, int maxExp)
    {
        if (minExp < 0 || maxExp < 0)
        {
            Toast(new object[2] { actorNumber, "的经验值变动不能为负数" });
            return;
        }

        FindPlayerRecorder(actorNumber, out GameObject recorder, out CharBase charBase);
        if (charBase == null || recorder == null) { return; }

        float current = ToGivePlayerSomething(new object[] { minExp, maxExp });
        charBase.CurrentExp += current;

        GameEventManager.EnableEvent(EventEnum.OnPlayerCurrentExpChanged, false);
        Toast(new object[2] { actorNumber, "获得了" + current + "经验" });

    }

    /// <summary>
    /// 【非事件回应】设置玩家升级数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="count"></param>
    public void SetPlayerLevel(int actorNumber, int count)
    {
        if (count < 0)
        {
            Toast(new object[2] { actorNumber, "等级变动不能为负数" });
            return;
        }

        FindPlayerRecorder(actorNumber, out GameObject recorder, out CharBase charBase);
        if (charBase == null || recorder == null) { return; }

        for (int i = 0; i < count; i++)
        {
            if (charBase.CurrentExp >= charBase.MaxExp)
            {
                charBase.CurrentExp -= charBase.MaxExp;
            }
            charBase.MaxExp += charBase.Level * 50;
            charBase.Level += 1;
            charBase.Attack += 100;
            charBase.MaxHealth += 1000;
            charBase.CurrentHealth += 1000;
            charBase.RespawnTime += 5;
            charBase.Restore += 2;
        }
        
        
        Toast(new object[2] { actorNumber, "等级设置为了" + count + "级" });
    }

    /// <summary>
    /// 【非事件回应】设置玩家金钱
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minMoney"></param>
    /// <param name="maxMoney"></param>
    public void SetPlayerMoney(int actorNumber, int minMoney, int maxMoney)
    {
        FindPlayerRecorder(actorNumber, out GameObject recorder, out CharBase charBase);
        if (charBase == null || recorder == null) { return; }


        int current = ToGivePlayerSomething(new object[] { minMoney, maxMoney });
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

   /// <summary>
    /// 【非事件回应】给与/扣除玩家最大血量 数值可为负数
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minMaxHealth"></param>
    /// <param name="maxMaxHealth"></param>
    public void SetPlayerMaxHealth(int actorNumber, int minMaxHealth, int maxMaxHealth)
    {

        FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
        if (charBase == null || recorder == null) { return; }

        int current = ToGivePlayerSomething(new object[]{ minMaxHealth,maxMaxHealth});
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

    /// <summary>
    /// 【非事件回应】设置玩家当前血量
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minHealth"></param>
    /// <param name="maxHealth"></param>
    public void SetPlayerCurrentHealth(int actorNumber, int currentHealth)
    {
        FindPlayerRecorder(actorNumber, out GameObject recorder, out CharBase charBase);
        if (charBase == null || recorder == null) { return; }

        charBase.CurrentHealth = currentHealth;

        Toast(new object[2] { actorNumber, "设置了血量为" + currentHealth });
    }

    /// <summary>
    /// 【非事件回应】通过id使玩家强制立即【彻底死亡 】
    /// </summary>
    /// <param name="actorNumber"></param>
    public void SetPlayerDead(int actorNumber)
    {
        FindPlayerRecorder(actorNumber, out GameObject recorder, out CharBase charBase);
        if (charBase == null || recorder == null) { return; }

        charBase.CurrentHealth = 0;
        charBase.State = StateEnum.Dead;

        GameEventManager.EnableEvent(EventEnum.OnPlayerDead, false);
        Toast(new object[2] { actorNumber, "彻底死亡" });

    }

    /// <summary>
    /// 【非事件回应】通过id修改玩家移动速度
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="moveSpeed"></param>
    public void SetPlayerMoveSpeed(int actorNumber, int moveSpeed)
    {
        FindPlayerRecorder(actorNumber, out GameObject recorder, out CharBase charBase);
        if (charBase == null || recorder == null) { return; }

        charBase.MoveSpeed = moveSpeed;

        Toast(new object[2] { actorNumber, "设置了移速为" + moveSpeed });
    }
    #endregion
    
    #region PUN Callbacks

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        FindPlayerRecorder(otherPlayer.ActorNumber,out GameObject recorder,out CharBase charBase);
        FindPlayerInfoBar(otherPlayer.ActorNumber, out GameObject playerInfoBar);
        recorders.Remove(otherPlayer.ActorNumber);
        playerInfoBarList.Remove(otherPlayer.ActorNumber);
        Destroy(playerInfoBar);
        Destroy(recorder);
    }

    #endregion
}
