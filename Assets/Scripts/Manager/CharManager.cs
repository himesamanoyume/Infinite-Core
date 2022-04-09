using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// �ṩ�Խ�ɫ��ֵ�Ŀ��ƺ��� �����������������
/// </summary>
public class CharManager : MonoBehaviour
{

    public GameObject total;

    public GameObject archer;
    public GameObject doctor;
    public GameObject solider;
    public GameObject tanker;
    public GameObject recorder;

    //public GameObject spawnRedPos1;
    //public GameObject spawnRedPos2;
    //public GameObject spawnRedPos3;
    //public GameObject spawnRedPos4;
    //public GameObject spawnRedPos5;

    //public GameObject spawnBluePos1;
    //public GameObject spawnBluePos2;
    //public GameObject spawnBluePos3;
    //public GameObject spawnBluePos4;
    //public GameObject spawnBluePos5;

    public GameObject[] redPosList = new GameObject[5];
    public GameObject[] bluePosList = new GameObject[5];

    public Dictionary<int, GameObject> playerModelList;
    public Dictionary<int, GameObject> recorders;

    public static CharManager Instance;

    private PhotonView photonView;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        playerModelList = new Dictionary<int, GameObject>();
        recorders = new Dictionary<int, GameObject>();

        SpawnPlayer(PhotonNetwork.LocalPlayer.ActorNumber);

        GameEventManager.SubscribeEvent(EventEnum.OnPlayerLevelUp, OnPlayerLevelUp);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerCurrentHealthChanged, OnPlayerCurrentHealthChanged);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerKilled, OnPlayerKilled);
        GameEventManager.SubscribeEvent(EventEnum.OnToast, Toast);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerRespawnCountDownStart, OnPlayerRespawnCountDownStart);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerRespawning, OnPlayerRespawning);  
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerRespawnCountDownEnd, OnPlayerRespawnCountDownEnd);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerRespawn,OnPlayerRespawn);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerRestoreing, OnPlayerRestoreing);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerRestoreChanged, OnPlayerRestoreChanged);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerLevelChanged, OnPlayerLevelChanged);
    }

    private void Update()
    {
        GetPlayerModelList();
        GetRecorderList();

       
    }

    /// <summary>
    /// �״��������
    /// </summary>
    /// <param name="player"></param>
    public void SpawnPlayer(int actorNumber)
    {
        //playerֻ�����Լ�
        
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if(player.ActorNumber == actorNumber)
            {
                if (player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_TEAM, out object team))
                {
                    if ((TeamEnum)team == TeamEnum.Null) return;

                    object pro;

                    switch ((TeamEnum)team)
                    {
                        case TeamEnum.Red:

                            player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_PRO, out pro);

                            if (recorders.TryGetValue(player.ActorNumber, out GameObject playerRecorder))
                            {

                            }
                            else
                            {
                                playerRecorder = PhotonNetwork.Instantiate("PlayerDataRecorder", new Vector3(100, 100, 100), Quaternion.identity);
                            }

                            GameObject playerModel = PhotonNetwork.Instantiate(((ProEnum)pro).ToString(), redPosList[Random.Range(0, 5)].transform.position, Quaternion.identity);


                            playerModel.name = player.NickName + " (My)";
                            playerRecorder.name = player.NickName + " Recorder {My}";

                            GetPlayerInfo(playerRecorder.GetComponent<CharBase>(), player);


                            break;
                        case TeamEnum.Blue:

                            player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_PRO, out pro);

                            if (recorders.TryGetValue(player.ActorNumber, out playerRecorder))
                            {

                            }
                            else
                            {
                                playerRecorder = PhotonNetwork.Instantiate("PlayerDataRecorder", new Vector3(100, 100, 100), Quaternion.identity);
                            }

                            playerModel = PhotonNetwork.Instantiate(((ProEnum)pro).ToString(), bluePosList[Random.Range(0, 5)].transform.position, Quaternion.identity);

                            playerModel.name = player.NickName + " (My)";
                            playerRecorder.name = player.NickName + " Recorder {My}";

                            GetPlayerInfo(playerRecorder.GetComponent<CharBase>(), player);


                            break;
                    }
                }
            }
        }
        
    }

    /// <summary>
    /// ����������Ի�ȡ������Ϣ
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

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_CRITICALHIT_HIT, out object criticalHit);
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
    /// ��ȡ�����������ģ��
    /// </summary>
    void GetPlayerModelList()
    {
        if (PhotonNetwork.PlayerList.Length == playerModelList.Count)
        {
            return;
        }
        else
        {
            int alivePlayerCount = 0;
            foreach (var recorder in recorders)
            {
                if (recorder.Value.GetComponent<CharBase>().State == StateEnum.Alive)
                {
                    alivePlayerCount++;
                }
            }

            if(alivePlayerCount == playerModelList.Count) return;

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
            if (alivePlayerCount == playerModelList.Count)
            {
                Debug.LogWarning("��������ȫ���������ģ��");
            }
            else
            {
                Debug.LogError("��������");
            }

        }
    }

    /// <summary>
    /// ��ȡ����������Ҽ�¼��
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

            Debug.LogWarning("����Ӽ�¼��");
        }

    }

    /// <summary>
    /// ͨ��id��ȡ�������
    /// </summary>
    /// <param name="actorNumber"></param>
    public void GetPlayerName(int actorNumber)
    {
        FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
        if (charBase == null || recorder == null) { return; }

        FindPlayerModel(actorNumber, out GameObject playerModel);
        if (playerModel == null) { return; }

        Toast(new object[2] { actorNumber, "��Ϊ" + charBase.PlayerName });
    }

    #region �¼���Ӧ

    /// <summary>
    /// ���¼���Ӧ������ҵȼ��䶯ʱ�ٽ���һ�μ��
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
    /// ���¼���Ӧ���������
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
            charBase.MaxExp += charBase.Level * 500;
            charBase.Level += 1;
            charBase.Attack += 100;
            charBase.MaxHealth += 1000;
            charBase.CurrentHealth += 1000;
            charBase.RespawnTime += 5;   
            charBase.Restore += 5;
            
            GameEventManager.EnableEvent(EventEnum.OnPlayerLevelUp, false);
            Toast(new object[2] { actorNumber, "level�Ѿ�����1��" });
        }
    }

    /// <summary>
    /// ���¼���Ӧ��ͨ��idʹ��ұ���ɱ
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerKilled(object[] args)
    {
        int actorNumber;

        if(args.Length == 1)
        {
            actorNumber = (int)args[0];

            FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            FindPlayerModel(actorNumber,out GameObject playerModel);
            if (playerModel == null) { return; }

            charBase.Death++;



            //recorder.GetComponent<PhotonView>().RPC("DestroyPlayerModel", RpcTarget.All, actorNumber );

            playerModelList.Remove(actorNumber);
            PhotonNetwork.Destroy(playerModel);

            Toast(new object[2] { actorNumber, "����ɱ" });
            GameEventManager.EnableEvent(EventEnum.OnPlayerKilled, false);
            GameEventManager.EnableEvent(EventEnum.OnPlayerRespawnCountDownStart, true);
        }     

    }

/// <summary>
    /// ���¼���Ӧ����Ҵ���˻�ɱ ���ݻ�ɱ��id����һ��ɱ����
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
            Toast(new object[2] { actorNumber, "��ɱ�˵���" });
        }
    }

    /// <summary>
    /// ���¼���Ӧ����һ�Ѫ
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
                Toast(new object[2] { actorNumber, "��Ѫֹͣ" });
            }
            else
            {
                //---
                Toast(new object[2] { actorNumber, "���ڻ�Ѫ" });
                //---
            }
        }     
    }

    /// <summary>
    /// ���¼���Ӧ������һ�Ѫֵ�仯ʱ
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

            Toast(new object[2] { actorNumber, "�ظ�ֵ�ı�" });

            GameEventManager.EnableEvent(EventEnum.OnPlayerRestoreChanged, false);
        }
    }

    /// <summary>
    /// ���¼���Ӧ����Ҵ����޺�״̬�³�������
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerDead(object[] args)
    {
        int actorNumber;
        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            FindPlayerModel(actorNumber, out GameObject playerModel);
            if(playerModel == null) { return; }

            playerModel.SetActive(false);

            charBase.State = StateEnum.Dead;
            GameEventManager.EnableEvent(EventEnum.OnPlayerDead, false);
            Toast(new object[2] { actorNumber, "��������" });
        }
    }

    /// <summary>
    /// ���¼���Ӧ�������������ʱ��ʼ
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
            Toast(new object[2] { actorNumber, "��ʼ��������ʱ" });
        }

    }

    /// <summary>
    /// ���¼���Ӧ��������ڸ���
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerRespawning(object[] args)
    {
        int actorNumber;

        if( args.Length == 1)
        {
            actorNumber = (int)args[0];

            FindPlayerRecorder(actorNumber,out GameObject recorder,out CharBase charBase);
            if (charBase == null || recorder == null) { return; }

            if (charBase.State == StateEnum.Respawning)
            {
                //----
                charBase.RespawnCountDown -= Time.deltaTime;
                //---
                Toast(new object[2] { actorNumber, "���ڸ���" });
            }

            if (charBase.RespawnCountDown <= 0)
            {
                
                GameEventManager.EnableEvent(EventEnum.OnPlayerRespawning,false);
                GameEventManager.EnableEvent(EventEnum.OnPlayerRespawnCountDownEnd, true);
            }
        }

    }

    /// <summary>
    /// ���¼���Ӧ����Ҹ����ʱ����
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

            Toast(new object[2] { actorNumber, "��������ʱ����" });
            GameEventManager.EnableEvent(EventEnum.OnPlayerRespawn, true);
            GameEventManager.EnableEvent(EventEnum.OnPlayerRespawnCountDownEnd, false);
            
        }
    }

    /// <summary>
    /// ���¼���Ӧ����Ҹ���
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

            charBase.CurrentHealth = charBase.MaxHealth;

            SpawnPlayer(PhotonNetwork.LocalPlayer.ActorNumber);

            Toast(new object[] { actorNumber, "�Ѹ���" });
            GameEventManager.EnableEvent(EventEnum.OnPlayerRespawn, false);
        }
       
    }

    /// <summary>
    /// ���¼���Ӧ������ҵ�ǰ����仯ʱ
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


            Toast(new object[] { actorNumber, "��ǰ�����Ѹı�" });

            GameEventManager.EnableEvent(EventEnum.OnPlayerCurrentExpChanged, false);
        }
    }

    /// <summary>
    /// ���¼���Ӧ������/�۳���һ���ֵ ��ֵ��Ϊ����
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

            Toast(new object[2] { actorNumber, "��ֵ�仯��" });

        }
    }

    /// <summary>
    /// ���¼���Ӧ������ұ��˱仯ʱ
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

            Toast(new object[2] { actorNumber, "�ı��˱仯��" });

        }
    }

    /// <summary>
    /// ���¼���Ӧ������ұ����ʱ仯ʱ
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
            Toast(new object[2] { actorNumber, "�ı����ʱ仯��" });
        }
    }

    /// <summary>
    /// ���¼���Ӧ����������ٱ仯ʱ
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
            Toast(new object[2] { actorNumber, "�����ٱ仯��" });
        }

    }

    /// <summary>
    /// ���¼���Ӧ������ҹ��ٱ仯ʱ
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

            Toast(new object[2] { actorNumber, "�Ĺ��ٱ仯��" });
        }

    }

    /// <summary>
    /// ���¼���Ӧ������ҽ�Ǯ�仯ʱ
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
            Toast(new object[2] { actorNumber, "��Ǯ�ı���"});
        }

    }

    /// <summary>
    /// ���¼���Ӧ������ҵ�ǰѪ���ı�ʱ
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
            Toast(new object[2] { actorNumber, "Ѫ���ı���" });
           
        }
    }

    /// <summary>
    /// ���¼���Ӧ����������Ѫ���仯ʱ
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
            Toast(new object[2] { actorNumber, "���Ѫ���ı���"  });
        }
    }

    #endregion

    #region ���¼���Ӧ

    /// <summary>
    /// �����¼���Ӧ������Ҿ���
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minExp"></param>
    /// <param name="maxExp"></param>
    public void ToGivePlayerCurrentExp(int actorNumber, int minExp, int maxExp)
    {
        if (minExp < 0 || maxExp < 0)
        {
            Toast(new object[2] { actorNumber, "�ľ���ֵ�䶯����Ϊ����" });
            return;
        }

        FindPlayerRecorder(actorNumber, out GameObject recorder, out CharBase charBase);
        if (charBase == null || recorder == null) { return; }

        float current = ToGivePlayerSomething(new object[] { minExp, maxExp });
        charBase.CurrentExp += current;

        GameEventManager.EnableEvent(EventEnum.OnPlayerCurrentExpChanged, false);
        Toast(new object[2] { actorNumber, "�����" + current + "����" });

    }

    /// <summary>
    /// �����¼���Ӧ���������������
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="count"></param>
    public void SetPlayerLevel(int actorNumber, int count)
    {
        if (count < 0)
        {
            Toast(new object[2] { actorNumber, "�ȼ��䶯����Ϊ����" });
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
            charBase.MaxExp += charBase.Level * 500;
            charBase.Level += 1;
            charBase.Attack += 100;
            charBase.MaxHealth += 1000;
            charBase.CurrentHealth += 1000;

            charBase.Restore += 5;
        }
        
        
        Toast(new object[2] { actorNumber, "�ȼ�����Ϊ��" + count + "��" });
    }

    /// <summary>
    /// �����¼���Ӧ��������ҽ�Ǯ
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
            Toast(new object[2] { actorNumber, "�����" + current + "��Ǯ" });
        }
        else
        {
            Toast(new object[2] { actorNumber, "�۳���" + current + "��Ǯ" });
        }
    }

   /// <summary>
    /// �����¼���Ӧ������/�۳�������Ѫ�� ��ֵ��Ϊ����
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
            Toast(new object[2] { actorNumber, "�����" + current + "���������ֵ" });
        }
        else
        {
            Toast(new object[2] { actorNumber, "�۳���" + current + "���������ֵ" });
        }
    }

    /// <summary>
    /// �����¼���Ӧ��������ҵ�ǰѪ��
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minHealth"></param>
    /// <param name="maxHealth"></param>
    public void SetPlayerCurrentHealth(int actorNumber, int currentHealth)
    {
        FindPlayerRecorder(actorNumber, out GameObject recorder, out CharBase charBase);
        if (charBase == null || recorder == null) { return; }

        charBase.CurrentHealth = currentHealth;

        Toast(new object[2] { actorNumber, "������Ѫ��Ϊ" + currentHealth });
    }

    /// <summary>
    /// �����¼���Ӧ��ͨ��idʹ���ǿ���������������� ��
    /// </summary>
    /// <param name="actorNumber"></param>
    public void SetPlayerDead(int actorNumber)
    {
        FindPlayerRecorder(actorNumber, out GameObject recorder, out CharBase charBase);
        if (charBase == null || recorder == null) { return; }

        charBase.CurrentHealth = 0;
        charBase.State = StateEnum.Dead;

        GameEventManager.EnableEvent(EventEnum.OnPlayerDead, false);
        Toast(new object[2] { actorNumber, "��������" });

    }

    #endregion
    /// <summary>
    /// (int)��һ�ȡ����,��ͨ��װ�����������ֵ�Ȼ�üӳ�ʱ��ͨ�ú���
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="lowerLimit"></param>
    /// <param name="upperLimit"></param>
    /// <returns>����ֵ���ڸ�װ����ʾ������ֵ</returns>
    public int ToGivePlayerSomething(object[] args)
    {
        int lowerLimit, upperLimit;

        if(args.Length == 2)
        {
            lowerLimit = (int)args[0];
            upperLimit = (int)args[1];

            int value = Random.Range(lowerLimit, upperLimit);
            return value;
        }
        else
        {
            Toast(new object[1] { "�����ֵ��������" });
            return 0;
        }
        
    }

    /// <summary>
    /// ��˿
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
            Debug.Log("ActorNumberΪ" + actorNumber + "�����" + text);
        }
        if (args.Length == 1)
        {
            text = args[0].ToString();
            Debug.Log(text);
        }
        GameEventManager.EnableEvent(EventEnum.OnToast, false);
    }

    /// <summary>
    /// ͨ��ActorNumber��ȡ�ض���ҵļ�¼��,�����ڽ�Ҫ�����ض����Խ����޸�ʱ
    /// </summary>
    /// <param name="actorNumber"></param>
    public void FindPlayerRecorder(int actorNumber,out GameObject playerRecorder, out CharBase charBase)
    {

        if (recorders == null)
        {
            Toast(new object[2] { actorNumber, "��Ҽ�¼����ȡʧ��, recorders" });
            playerRecorder = null;
            charBase = null;
        }

        if(recorders.TryGetValue(actorNumber, out GameObject recorder))
        {
            charBase = recorder.GetComponent<CharBase>();
            playerRecorder = recorder;
            Toast(new object[2] { actorNumber, "��Ҽ�¼����ȡ�ɹ�" });
        }
        else
        {
            Toast(new object[2] { actorNumber, "��Ҽ�¼����ȡʧ��" });
            charBase = null;
            playerRecorder = null;
        }
        
    }

    /// <summary>
    /// ��ȡ��Ӧ���ģ��
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="playerModel"></param>
    public void FindPlayerModel(int actorNumber, out GameObject playerModel)
    {
        if (playerModelList == null)
        {
            Toast(new object[2] { actorNumber, "���ģ�ͻ�ȡʧ��, playerModelListΪ��" });

            playerModel = null;
        }

        if (playerModelList.TryGetValue(actorNumber, out GameObject model))
        {
            
            Toast(new object[2] { actorNumber, "���ģ�ͻ�ȡ�ɹ�" });
            playerModel = model;
        }
        else
        {
            Toast(new object[2] { actorNumber, "���ģ�ͻ�ȡʧ��" });
            playerModel = null;
        }

    }

    /// <summary>
    /// ͨ��tag�Ҹ������ĳ������������
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
    /// ����CharBase�����ȡ������Ϣ
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

    #region PUN Callbacks



    #endregion
}
