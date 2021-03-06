using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using ExitGames.Client.Photon;


public class CharBase : MonoBehaviourPunCallbacks, IPunObservable
{

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    #region Private Fields
    private object[] m_args;

    [SerializeField, Header("基本信息")]
    /// <summary>
    /// 运行时才赋予的独特ID 用于控制器快速为指定ID添加或删除特定效果
    /// </summary>
    private int actorNumber = -1;
    [SerializeField]
    /// <summary>
    /// 玩家姓名
    /// </summary>
    private string playerName;
    [SerializeField]
    /// <summary>
    /// 队伍
    /// </summary>
    private TeamEnum playerTeam = TeamEnum.Null;
    [SerializeField, Header("击杀死亡金币数")]
    /// <summary>
    /// 击杀数
    /// </summary>
    private int kill = 0;
    [SerializeField]
    /// <summary>
    /// 死亡数
    /// </summary>
    private int death = 0;
    [SerializeField]
    /// <summary>
    /// 角色所拥有的金币
    /// </summary>
    private int money = 0;
    [SerializeField, Header("经验")]
    /// <summary>
    /// 当前经验
    /// </summary>
    private float currentExp = 0f;
    [SerializeField]
    /// <summary>
    /// 所需经验
    /// </summary>
    private float maxExp = 500f;
    [SerializeField, Header("当前状态")]
    /// <summary>
    /// 角色状态
    /// </summary>
    private StateEnum state = StateEnum.NonLife;
    [SerializeField]
    /// <summary>
    /// 当前角色身上所带的buff
    /// </summary>
    private Dictionary<int,BuffEnum> buff = new Dictionary<int, BuffEnum>();
    [SerializeField, Header("职业")]
    /// <summary>
    /// 角色职业
    /// </summary>
    private ProEnum pro;
    [SerializeField, Header("角色属性")]
    /// <summary>
    /// 等级
    /// </summary>
    private int level = 1;
    [SerializeField]
    /// <summary>
    /// 攻击
    /// </summary>
    private float attack = 100f;
    [SerializeField]
    /// <summary>
    /// 护盾值
    /// </summary>
    private float shield = 0f;
    [SerializeField]
    /// <summary>
    /// 最大生命
    /// </summary>
    private float maxHealth = 1000f;
    [SerializeField]
    /// <summary>
    /// 当前生命
    /// </summary>
    private float currentHealth = 1000f;
    [SerializeField]
    /// <summary>
    /// 爆伤
    /// </summary>
    private float criticalHit = 0.5f;
    [SerializeField]
    /// <summary>
    /// 暴击(率)
    /// </summary>
    private float criticalHitRate = 0.05f;
    [SerializeField]
    /// <summary>
    /// 防御
    /// </summary>
    private float defence = 1000f;
    [SerializeField]
    /// <summary>
    /// 攻速
    /// </summary>
    private float attackSpeed = 1f;
    [SerializeField]
    /// <summary>
    /// 生命恢复
    /// </summary>
    private float restore = 10f;
    [SerializeField]
    /// <summary>
    /// 移动速度
    /// </summary>
    private float moveSpeed = 8f;
    [SerializeField]
    /// <summary>
    /// 重生所需时间
    /// </summary>
    private float respawnTime = 10f;
    [SerializeField]
    /// <summary>
    /// 重生倒计时
    /// </summary>
    private float respawnCountDown = 10f;
    [SerializeField]
    /// <summary>
    /// 攻击范围
    /// </summary>
    private float attackRange = 10f;
    /// <summary>
    /// 最终伤害倍率
    /// </summary>
    private float finalDamage = 1f;
    [SerializeField, Header("技能")]
    /// <summary>
    /// Q技能
    /// </summary>
    private SkillQ skillQ;
    [SerializeField]
    /// <summary>
    /// E技能
    /// </summary>
    private SkillE skillE;
    [SerializeField]
    /// <summary>
    /// R技能
    /// </summary>
    private SkillR skillR;
    [SerializeField]
    /// <summary>
    /// Space技能
    /// </summary>
    private SkillBurst skillBurst;
    [SerializeField, Header("装备")]
    /// <summary>
    /// 头盔
    /// </summary>
    private EquipHead headSuit;
    [SerializeField]
    /// <summary>
    /// 护甲
    /// </summary>
    private EquipArmor armorSuit;
    [SerializeField]
    /// <summary>
    /// 护手
    /// </summary>
    private EquipHand handSuit;
    [SerializeField]
    /// <summary>
    /// 护膝
    /// </summary>
    private EquipKnee kneeSuit;
    [SerializeField]
    /// <summary>
    /// 护腿
    /// </summary>
    private EquipTrousers trousersSuit;
    [SerializeField]
    /// <summary>
    /// 鞋子
    /// </summary>
    private EquipBoots bootsSuit;
    [SerializeField]
    /// <summary>
    /// 普攻倍率
    /// </summary>
    private float normalAttackRatio = 1f;

    #endregion

    #region Public Functions

    public int ActorNumber { get => actorNumber; set => actorNumber = value; }
    public string PlayerName { get => playerName; set => playerName = value; }
    public int Kill 
    {
        get => kill;
        set
        {
            if (value < 0)
            {
                m_args = new object[2] { actorNumber, "杀敌数不合法" };
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnToast, true);
                }

            }
            else
            {
                if (kill == value) return;

                kill = value;
                if (photonView.IsMine)
                {
                    //GameEventManager.EnableEvent(EventEnum.OnPlayerKill, true);
                }

            }
        }
    }
    public int Death
    {
        get => death;
        set
        {
            if (value < 0)
            {
                m_args = new object[2] { actorNumber, "死亡数不合法" };
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnToast, true);
                }

            }
            else
            {
                if (death == value) return;

                death = value;
            }
        }
    }
    public int Money
    {
        get => money;
        set
        {
            if (value < 0 || value > 999999)
            {
                m_args = new object[2] { actorNumber, "金钱不合法" };
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnToast, true);
                }

            }
            else
            {
                if (money == value) return;

                money = value;
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnPlayerMoneyChanged, true);
                }

            }
        }
    }
    public float CurrentExp
    {
        get => currentExp;
        set
        {
            if (value < 0)
            {
                m_args = new object[2] { actorNumber, "经验不合法" };
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnToast, true);
                }

            }
            else
            {
                if (currentExp == value) return;
                if (value >= MaxExp && Level == 15)
                {
                    //最大等级时只能达到最大经验限制
                    currentExp = MaxExp;
                    return;
                }
                currentExp = value;
                if (currentExp >= MaxExp && currentExp != 0)
                {
                    if (photonView.IsMine)
                    {
                        GameEventManager.EnableEvent(EventEnum.OnPlayerLevelUp, true);
                    }


                }
                else
                {
                    if (photonView.IsMine)
                    {
                        GameEventManager.EnableEvent(EventEnum.OnPlayerLevelUp, false);
                    }

                }
            }
        }
    }
    public float MaxExp
    {
        get => maxExp;
        set
        {
            if (value < 0)
            {
                m_args = new object[2] { actorNumber, "最大经验值不合法" };
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnToast, true);
                }

            }
            else
            {
                if (maxExp == value) return;

                maxExp = value;
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnPlayerMaxExpChanged, true);
                }

            }
        }
    }
    public StateEnum State
    {
        get => state;
        set
        {
            if (state == value) return;

            state = value;
            if (photonView.IsMine)
            {
                GameEventManager.EnableEvent(EventEnum.OnPlayerStateChanged, true);
            }

        }
    }
    public Dictionary<int, BuffEnum> Buff
    {
        get => buff;
        set
        {
            if (buff == value) return;

            buff = value;
            if (photonView.IsMine)
            {
                GameEventManager.EnableEvent(EventEnum.OnPlayerBuffChanged, true);
            }

        }
    }
    public ProEnum Pro { get => pro; set => pro = value; }
    public int Level
    {
        get => level;
        set
        {
            if (value < 0)
            {
                m_args = new object[2] { actorNumber, "等级不合法" };
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnToast, true);
                }

            }
            else
            {
                if (level == value) return;
                if (value > 15)
                {
                    m_args = new object[2] { actorNumber, "等级已达到最大" };
                    return;
                }

                level = value;
                if (photonView.IsMine)
                {
                    GameObject prefab = (GameObject)Resources.Load("Prefabs/UI/MiscInfo/MiscInfoPrefab");
                    Instantiate(prefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, "你的等级提升了");
                    GameEventManager.EnableEvent(EventEnum.OnPlayerLevelChanged, true);
                }

            }
        }
    }
    public float Attack
    {
        get => attack;
        set
        {
            if (value < 0)
            {
                attack = 0;
                m_args = new object[2] { actorNumber, "攻击力不合法" };
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnToast, true);
                }

            }
            else
            {
                if (attack == value) return;

                attack = value;
                if (photonView.IsMine)
                {
                    
                    GameEventManager.EnableEvent(EventEnum.OnPlayerAttackChanged, true);
                }

            }
        }
    }
    public float Shield
    {
        get => shield;
        set
        {
            if (value < 0)
            {
                shield = 0;
                m_args = new object[2] { actorNumber, "护盾不合法,已使护盾为0" };
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnToast, true);
                }


            }
            else
            {
                if (shield == value) return;

                shield = value;
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnPlayerShieldChanged, true);
                }

            }
        }
    }
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            if (value <= 0)
            {
                currentHealth = 0;
                
                if (State == StateEnum.Alive)
                {
                    if (Buff == null)
                    {
                        
                        if (photonView.IsMine)
                        {
                            
                            GameEventManager.EnableEvent(EventEnum.OnPlayerKilled, true);
                            GameEventManager.EnableEvent(EventEnum.OnPlayerRestoreing, false);
                            m_args = new object[2] { actorNumber, "血量不合法,已使玩家死亡" };
                            GameEventManager.EnableEvent(EventEnum.OnToast, true);
                        }

                    }
                    else
                    {
                        

                        if (Buff.TryGetValue((int)BuffEnum.Coreless, out BuffEnum buffEnum))
                        {
                            
                            if (photonView.IsMine)
                            {
                                
                                GameEventManager.EnableEvent(EventEnum.OnPlayerRestoreing, false);
                                GameEventManager.EnableEvent(EventEnum.OnPlayerDead, true);
                            }

                        }
                        else
                        {
                            
                            if (photonView.IsMine)
                            {
                                
                                GameEventManager.EnableEvent(EventEnum.OnPlayerKilled, true);
                                GameEventManager.EnableEvent(EventEnum.OnPlayerRestoreing, false);
                                m_args = new object[2] { actorNumber, "血量不合法,已使玩家死亡" };
                                GameEventManager.EnableEvent(EventEnum.OnToast, true);
                            }

                        }
                    }

                }

            }
            else if (value >= MaxHealth)
            {
                currentHealth = MaxHealth;
                m_args = new object[2] { actorNumber, "血量不得超过最大血量" };
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnToast, true);
                    GameEventManager.EnableEvent(EventEnum.OnPlayerRestoreing, false);
                    GameEventManager.EnableEvent(EventEnum.OnPlayerCurrentHealthChanged, true);
                }

            }
            else
            {
                
                if (currentHealth == value) return;

                currentHealth = value;

                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnPlayerRestoreing, true);
                    GameEventManager.EnableEvent(EventEnum.OnPlayerCurrentHealthChanged, true);
                }

            }
        }
    }
    public float MaxHealth
    {
        get => maxHealth;
        set
        {
            if (value < 1)
            {
                maxHealth = 1;
                m_args = new object[2] { actorNumber, "最大血量不合法,已达到最低值" };
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnToast, true);
                }

            }
            else
            {
                if (maxHealth == value) return;

                maxHealth = value;
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnPlayerMaxHealthChanged, true);
                }
            }
        }
    }
    public float CriticalHit
    {
        get => criticalHit;
        set
        {
            if (value < 0)
            {
                if (criticalHit == value) return;

                criticalHit = 0;
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnPlayerCriticalHitChanged, true);
                }

            }
            else
            {
                if (criticalHit == value) return;

                criticalHit = value;
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnPlayerCriticalHitChanged, true);
                }

            }

        }
    }
    public float CriticalHitRate
    {
        get => criticalHitRate;
        set
        {
            if (criticalHitRate == value) return;

            if (value < 0.05f)
            {
                criticalHitRate = 0.05f;

            }
            else if (value > 1)
            {
                criticalHitRate = 1;
            }
            else
            {
                criticalHitRate = value;
            }

            if (photonView.IsMine)
            {
                GameEventManager.EnableEvent(EventEnum.OnPlayerCriticalHitRateChanged, true);
            }

        }
    }
    public float Defence
    {
        get => defence;
        set
        {
            if (value < 0)
            {
                defence = 0;
                m_args = new object[2] { actorNumber, "防御不合法,已为最低值" };
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnToast, true);

                }
            }
            else
            {
                if (defence == value) return;

                defence = value;
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnPlayerDefenceChanged, true);

                }
            }
        }
    }
    public float AttackSpeed
    {
        get => attackSpeed;
        set
        {
            if (value < 0.1f)
            {
                attackSpeed = 0.1f;
                m_args = new object[2] { actorNumber, "攻速不合法,已为最低值" };
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnToast, true);

                }
            }
            else
            {
                if (attackSpeed == value) return;

                attackSpeed = value;
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnPlayerAttackSpeedChanged, true);

                }
            }
        }
    }
    public float Restore
    {
        get => restore;
        set
        {
            if (value < 0)
            {
                restore = 0;
                m_args = new object[2] { actorNumber, "回血速度不合法" };
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnToast, true);

                }
            }
            else
            {
                if (restore == value) return;

                restore = value;
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnPlayerRestoreChanged, true);

                }
            }
        }
    }

    public SkillQ SkillQ { get => skillQ; set => skillQ = value; }
    public SkillE SkillE { get => skillE; set => skillE = value; }
    public SkillR SkillR { get => skillR; set => skillR = value; }
    public SkillBurst SkillBurst { get => skillBurst; set => skillBurst = value; }
    public EquipHead HeadSuit { 
        get => headSuit; 
        set
        {
            suitController.RemoveSuitCount((EquipSuit)headSuit);
            headSuit = value;
            suitController.AddSuitCount((EquipSuit)headSuit);
            suitController.SuitUpdate();
        } 
    }
    public EquipArmor ArmorSuit { 
        get => armorSuit;
        set 
        {
            suitController.RemoveSuitCount((EquipSuit)armorSuit);
            armorSuit = value;
            suitController.AddSuitCount((EquipSuit)armorSuit);
            suitController.SuitUpdate();
        } 
    }
    public EquipHand HandSuit { 
        get => handSuit;
        set
        {
            suitController.RemoveSuitCount((EquipSuit)handSuit);
            handSuit = value;
            suitController.AddSuitCount((EquipSuit)handSuit);
            suitController.SuitUpdate();
        }
     }
    public EquipKnee KneeSuit { 
        get => kneeSuit;
        set {
            suitController.RemoveSuitCount((EquipSuit)kneeSuit);
            kneeSuit = value;
            suitController.AddSuitCount((EquipSuit)kneeSuit);
            suitController.SuitUpdate();
        }
    }
    public EquipTrousers TrousersSuit { 
        get => trousersSuit;
        set {
            suitController.RemoveSuitCount((EquipSuit)trousersSuit);
            trousersSuit = value;
            suitController.AddSuitCount((EquipSuit)trousersSuit);
            suitController.SuitUpdate();
        }
    }
    public EquipBoots BootsSuit { 
        get => bootsSuit;
        set {
            suitController.RemoveSuitCount((EquipSuit)bootsSuit);
            bootsSuit = value;
            suitController.AddSuitCount((EquipSuit)bootsSuit);
            suitController.SuitUpdate();
        }
    }

    public float MoveSpeed
    {
        get => moveSpeed;
        set
        {
            if (value < 0)
            {
                m_args = new object[2] { actorNumber, "移速不合法" };
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnToast, true);

                }
            }
            else
            {
                if (moveSpeed == value) return;

                moveSpeed = value;
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.SendPlayerMoveSpeed, true);
                    GameEventManager.EnableEvent(EventEnum.OnPlayerMoveSpeedChanged, true);

                }
            }
        }
    }
    public float AttackRange
    {
        get => attackRange;
        set
        {
            if (value < 0)
            {
                m_args = new object[2] { actorNumber, "攻击范围不合法" };
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnToast, true);

                }
            }
            else
            {
                if (attackRange == value) return;

                attackRange = value;
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnPlayerAttackRangeChanged, true);

                }
            }
        }
    }
    public float RespawnTime
    {
        get => respawnTime;
        set
        {
            if (value < 0)
            {
                m_args = new object[2] { actorNumber, "重生时间不合法" };
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnToast, true);
                }

            }
            else if (respawnTime + value > 60f)
            {
                respawnTime = 60f;
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnPlayerRespawnTimeChanged, true);
                }

            }
            else
            {
                if (respawnTime == value) return;

                respawnTime = value;
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnPlayerRespawnTimeChanged, true);
                }

            }
        }
    }
    public float RespawnCountDown
    {
        get => respawnCountDown;
        set
        {
            if (value < -1f)
            {
                m_args = new object[2] { actorNumber, "重生倒计时不合法" };
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnToast, true);
                }

            }
            else
            {
                respawnCountDown = value;
            }

        }
    }
    public TeamEnum PlayerTeam { get => playerTeam; set => playerTeam = value; }
    public float FinalDamage
    {
        get => finalDamage;
        set
        {
            if (value<=1f)
            {
                return;
            }
            else
            {
                if (finalDamage == value) return;

                finalDamage = value;
                if (photonView.IsMine)
                {
                    GameEventManager.EnableEvent(EventEnum.OnPlayerFinalDamageChanged, true);
                }
            }
        }
    }
    public float NormalAttackRatio
    {
        get=> normalAttackRatio;
        set
        {
            normalAttackRatio = value;
        }
    }
    #endregion

    #region Photon Callbacks

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(ActorNumber);
            stream.SendNext(PlayerName);
            stream.SendNext(PlayerTeam);
            stream.SendNext(Kill);
            stream.SendNext(Death);
            stream.SendNext(Money);
            stream.SendNext(CurrentExp);
            stream.SendNext(MaxExp);
            stream.SendNext(State);
            //stream.SendNext(Buff);
            //stream.SendNext(Pro);
            stream.SendNext(Level);
            stream.SendNext(Attack);
            stream.SendNext(Shield);
            stream.SendNext(MaxHealth);
            stream.SendNext(CurrentHealth);
            stream.SendNext(CriticalHit);
            stream.SendNext(CriticalHitRate);
            stream.SendNext(Defence);
            stream.SendNext(AttackSpeed);
            stream.SendNext(Restore);
            stream.SendNext(SkillQ);
            stream.SendNext(SkillE);
            stream.SendNext(SkillR);
            stream.SendNext(SkillBurst);
            stream.SendNext(HeadSuit);
            stream.SendNext(ArmorSuit);
            stream.SendNext(HandSuit);
            stream.SendNext(KneeSuit);
            stream.SendNext(TrousersSuit);
            stream.SendNext(BootsSuit);
            stream.SendNext(MoveSpeed);
            stream.SendNext(AttackRange);
            stream.SendNext(RespawnTime);
            stream.SendNext(RespawnCountDown);
            stream.SendNext(NormalAttackRatio);
        }
        else
        {
            ActorNumber = (int)stream.ReceiveNext();
            PlayerName = (string)stream.ReceiveNext();
            PlayerTeam = (TeamEnum)stream.ReceiveNext();
            Kill = (int)stream.ReceiveNext();
            Death = (int)stream.ReceiveNext();
            Money = (int)stream.ReceiveNext();
            CurrentExp = (float)stream.ReceiveNext();
            MaxExp = (float)stream.ReceiveNext();
            State = (StateEnum)stream.ReceiveNext();
            //Buff = (Dictionary<int,BuffEnum>)stream.ReceiveNext();
            //Pro = (ProEnum)stream.ReceiveNext();
            Level = (int)stream.ReceiveNext();
            Attack = (float)stream.ReceiveNext();
            Shield = (float)stream.ReceiveNext();
            MaxHealth = (float)stream.ReceiveNext();
            CurrentHealth = (float)stream.ReceiveNext();
            CriticalHit = (float)stream.ReceiveNext();
            CriticalHitRate = (float)stream.ReceiveNext();
            Defence = (float)stream.ReceiveNext();
            AttackSpeed = (float)stream.ReceiveNext();
            Restore = (float)stream.ReceiveNext();
            SkillQ = (SkillQ)stream.ReceiveNext();
            SkillE = (SkillE)stream.ReceiveNext();
            SkillR = (SkillR)stream.ReceiveNext();
            SkillBurst = (SkillBurst)stream.ReceiveNext();
            HeadSuit = (EquipHead)stream.ReceiveNext();
            ArmorSuit = (EquipArmor)stream.ReceiveNext();
            HandSuit = (EquipHand)stream.ReceiveNext();
            KneeSuit = (EquipKnee)stream.ReceiveNext();
            TrousersSuit = (EquipTrousers)stream.ReceiveNext();
            BootsSuit = (EquipBoots)stream.ReceiveNext();
            MoveSpeed = (float)stream.ReceiveNext();
            AttackRange = (float)stream.ReceiveNext();
            RespawnTime = (float)stream.ReceiveNext();
            RespawnCountDown = (float)stream.ReceiveNext();
            NormalAttackRatio = (float)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void DestroyPlayerModel(int actorNumber)
    {
        CharManager charManager = GameObject.Find("CharManager").GetComponent<CharManager>();
        charManager.FindPlayerModel(actorNumber, out GameObject playerModel);
        if (playerModel == null) { return; }
            
        Debug.LogWarning(playerModel.name+"已网络销毁玩家模型");

        charManager.playerModelList.Remove(actorNumber);
        PhotonNetwork.Destroy(playerModel);
        

    }

    [PunRPC]
    public void BroadcastInfo(string text)
    {
        uiManager.GetImportantInfo(text);
    }

    [PunRPC]
    public void GetKillCount(int killerActorNumber)
    {
        CharBase killerCharBase = charManager.recorders[killerActorNumber].GetComponent<CharBase>();

        killerCharBase.Kill++;
        killerCharBase.CurrentExp += 1000 + Level * 100;
        killerCharBase.Money += 300 + Level * 50;
    }

    [PunRPC]
    public void GetMonsterAward(int killerActorNumber, float awardExp, int awardMoney)
    {
        //Debug.LogWarning("获得杀怪奖赏" + killerActorNumber);
        if (killerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber) return;

        CharBase killerCharBase = charManager.recorders[killerActorNumber].GetComponent<CharBase>();

        killerCharBase.CurrentExp += awardExp;
        killerCharBase.Money += awardMoney;
    }

    /// <summary>
    /// 普通的Buff添加 需要具体时间
    /// </summary>
    /// <param name="receiverActorNumber"></param>
    /// <param name="buff"></param>
    /// <param name="time"></param>
    [PunRPC]
    public void GetBuff(int receiverActorNumber, BuffEnum buff, float time)
    {
        if (receiverActorNumber != actorNumber)
            return;
        if (!Buff.ContainsKey((int)buff))
        {
            Buff.Add((int)buff, buff);
            BuffController buffController = GetComponent<BuffController>();
            if (time >= 0)
            {
                buffController.AddBuff(buff, time);
            }
            
        }
    }

    /// <summary>
    /// 对所有人添加BUFF 通常只用来添加Coreless 所以不限制时间
    /// </summary>
    /// <param name="buff"></param>
    [PunRPC]
    public void AllGetBuff(BuffEnum buff)
    {
        if (!Buff.ContainsKey((int)buff))
        {
            Buff.Add((int)buff, buff);
        }
    }

    [PunRPC]
    public void GameOver()
    {
        GameEventManager.EnableEvent(EventEnum.PlayerGroup, false);
    }

    #endregion

    #region Unity Functions

    void Awake()
    {

    }

    void Start()
    {
        #region Register Event

        if (photonView.IsMine)
        {
            //Debug.LogWarning("CharBase注册事件");
            GameEventManager.RegisterEvent(EventEnum.OnPlayerFinalDamageChanged, OnPlayerFinalDamageChangedCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerCriticalHitRateChanged, OnPlayerCriticalHitRateChangedCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerCriticalHitChanged, OnPlayerCriticalHitChangedCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerAttackRangeChanged, OnPlayerAttackRangeChangedCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerLevelUp, OnPlayerLevelUpCheck);

            GameEventManager.RegisterEvent(EventEnum.OnToast, OnToastCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerKill, OnPlayerKillCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerRespawn, OnPlayerRespawnCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerRespawning, OnPlayerRespawningCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerRestoreing, OnPlayerRestoreingCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerMoneyChanged, OnPlayerMoneyChangedCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerRespawnTimeChanged, OnPlayerRespawnTimeChangedCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerRespawnCountDownStart, OnPlayerRespawnCountDownStartCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerRespawnCountDownEnd, OnPlayerRespawnCountDownEndCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerMaxHealthChanged, OnPlayerMaxHealthChangedCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerCurrentHealthChanged, OnPlayerCurrentHealthChangedCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerLevelChanged, OnPlayerLevelChangedCheck);

            GameEventManager.RegisterEvent(EventEnum.SendPlayerMoveSpeed, SendPlayerMoveSpeedCheck);
            //GameEventManager.EnableEvent(EventEnum.SendPlayerMoveSpeed, true);
            
            GameEventManager.RegisterEvent(EventEnum.AllowPlayerMove, PlayerMoveCheck);

            GameEventManager.RegisterEvent(EventEnum.AllowPlayerAttack, PlayerAttackCheck);

            GameEventManager.RegisterEvent(EventEnum.AllowPlayerTowardChanged, PlayerTowardChangedCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerAttackChanged, OnPlayerAttackChangedCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerKilled, OnPlayerKilledCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerDead, OnPlayerDeadCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerBuffChanged, OnPlayerBuffChangedCheck);
        }

        #endregion

        #region Subscribe Event

        if (photonView.IsMine)
        {
            GameEventManager.SubscribeEvent(EventEnum.OnPlayerDamaged, OnPlayerDamaged);
        }

        #endregion

        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        charManager = GameObject.Find("CharManager").GetComponent<CharManager>();
        suitController = GetComponent<SuitController>();
    }

    UIManager uiManager;
    CharManager charManager;
    SuitController suitController;

    void UpdateFloatProps(string key, float value)
    {
        Hashtable props = new Hashtable()
        {
            {key, value }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    #endregion

    #region Event Response

    int lastOneHurtActorNumber = -1;

    public void OnPlayerDamaged(object[] args)
    {
        int enemyActorNumber;
        float rFinalAttack, rFinalDamage;
        if (args.Length == 3)
        {
            enemyActorNumber = (int)args[0];
            rFinalAttack = (float)args[1];
            rFinalDamage = (float)args[2];

            lastOneHurtActorNumber = enemyActorNumber;
            //Debug.LogWarning("rFinalAttack: "+ rFinalAttack);

            float hurt = rFinalAttack * (1 - Defence * 0.00001f) * rFinalDamage;

            //Debug.LogWarning("hurt: " + hurt);

            //如果有护盾
            if (Shield>0)
            {
                //如果伤害大于护盾值 护盾清0 再对当前生命造成伤害
                if (hurt>Shield)
                {
                    Shield = 0;
                    CurrentHealth -= (hurt - Shield);
                }
                else
                {
                    //如果盾值大于伤害 只减少盾值
                    Shield -= hurt;
                }
            }
            else
            {
                //无盾 直接扣血
                CurrentHealth -= hurt;
            }
            GameEventManager.EnableEvent(EventEnum.OnPlayerDamaged, false);
        }
    }

    #endregion

    #region Event Check

    bool OnPlayerBuffChangedCheck(out object[] args)
    {
        //Buff.TryGetValue((int)BuffEnum.Coreless, out BuffEnum buffEnum);
        //Debug.LogWarning(buffEnum);
        args = new object[] { ActorNumber};
        return true;
    }

    bool OnPlayerFinalDamageChangedCheck(out object[] args)
    {
        UpdateFloatProps(InfiniteCoreGame.PLAYER_FINAL_DAMAGE, FinalDamage);

        args = new object[] { ActorNumber, FinalDamage };
        return true;
    }

    bool OnPlayerCriticalHitRateChangedCheck(out object[] args)
    {
        UpdateFloatProps(InfiniteCoreGame.PLAYER_CRITICAL_HIT_RATE, CriticalHitRate);
        args = new object[] { ActorNumber, CriticalHitRate };
        return true;
    }

    bool OnPlayerCriticalHitChangedCheck(out object[] args)
    {
        UpdateFloatProps(InfiniteCoreGame.PLAYER_CRITICAL_HIT, CriticalHit);
        args = new object[] { ActorNumber, CriticalHit };
        return true;
    }

    bool OnPlayerAttackRangeChangedCheck(out object[] args)
    {
        UpdateFloatProps(InfiniteCoreGame.PLAYER_ATTACK_RANGE, AttackRange);
        args = new object[] { ActorNumber, AttackRange };
        return true;
    }

    bool OnPlayerAttackChangedCheck(out object[] args)
    {
        UpdateFloatProps(InfiniteCoreGame.PLAYER_ATTACK, Attack);

        args = new object[] { ActorNumber, Attack};
        return true;
    }

    bool PlayerMoveCheck(out object[] args)
    {
        args = null;
        return true;
    }

    bool PlayerAttackCheck(out object[] args)
    {
        args = null;
        return true;
    }

    bool PlayerTowardChangedCheck(out object[] args)
    {
        args = null;
        return true;
    }

    bool SendPlayerMoveSpeedCheck(out object[] args)
    {
        args = new object[] { ActorNumber, MoveSpeed };
        return true;
    }

    bool OnPlayerLevelChangedCheck(out object[] args)
    {
        UpdateFloatProps(InfiniteCoreGame.PLAYER_LEVEL, Level);
        args = new object[] { ActorNumber};
        return true;
    }

    bool OnPlayerCurrentHealthChangedCheck(out object[] args)
    {
        args = new object[] { ActorNumber };
        return true;
    }

    bool OnPlayerMoneyChangedCheck(out object[] args)
    {
        UpdateFloatProps(InfiniteCoreGame.PLAYER_LEVEL, Level);
        args = new object[] { ActorNumber };
        return true;
    }

    bool OnPlayerMaxHealthChangedCheck(out object[] args)
    {
        UpdateFloatProps(InfiniteCoreGame.PLAYER_MAX_HEALTH, MaxHealth);
        args = new object[] {ActorNumber};
        return true;
    }

    bool OnPlayerKillCheck(out object[] args)
    {
        args = new object[] { ActorNumber };
        //args = new object[] { lastOneHurtActorNumber };
        return true;
    }

    bool OnPlayerRestoreingCheck(out object[] args)
    {
        args = new object[] { ActorNumber};
        return true;
    }

    bool OnPlayerKilledCheck(out object[] args)
    {
        if (lastOneHurtActorNumber != -1)
        {
            if (lastOneHurtActorNumber == -2)
            {
                photonView.RPC("BroadcastInfo", RpcTarget.AllViaServer, PlayerName + "竟被野怪击杀!");
            }
            else
            {
                string killerName = charManager.recorders[lastOneHurtActorNumber].GetComponent<CharBase>().PlayerName;

                photonView.RPC("GetKillCount", RpcTarget.All, lastOneHurtActorNumber);
                photonView.RPC("BroadcastInfo", RpcTarget.AllViaServer, killerName + "击杀了" + PlayerName);
            }
           
        }

        args = new object[] { ActorNumber, lastOneHurtActorNumber };
        lastOneHurtActorNumber = -1;
        return true;
    }

    bool OnPlayerDeadCheck(out object[] args)
    {
        if (lastOneHurtActorNumber != -1)
        {
            if (lastOneHurtActorNumber == -2)
            {
                photonView.RPC("BroadcastInfo", RpcTarget.AllViaServer, PlayerName + "竟被野怪击杀!");
            }
            else
            {
                string killerName = charManager.recorders[lastOneHurtActorNumber].GetComponent<CharBase>().PlayerName;

                photonView.RPC("GetKillCount", RpcTarget.All, lastOneHurtActorNumber);
                photonView.RPC("BroadcastInfo", RpcTarget.AllViaServer, killerName + "击杀了" + PlayerName);
            }
        }

        args = new object[] { ActorNumber, lastOneHurtActorNumber };
        lastOneHurtActorNumber = -1;
        return true;
    }

    bool OnPlayerRespawnCheck(out object[] args)
    {
        args = new object[] { ActorNumber };
        return true;
    }

    bool OnPlayerRespawnTimeChangedCheck(out object[] args)
    {
        args=null;
        return true;
    }

    bool OnPlayerRespawningCheck(out object[] args)
    {
        args =new object[] { ActorNumber, respawnCountDown } ;
        return true;
    }

    bool OnPlayerStateChangedCheck(out object[] args)
    {
        args = null;
        return true;
    }

    bool OnPlayerRespawnCountDownStartCheck(out object[] args)
    {
        args = new object[] { ActorNumber};
        return true;
    }

    bool OnPlayerRespawnCountDownEndCheck(out object[] args)
    {
        args = new object[] { ActorNumber };
        return true;
    }

    bool OnPlayerRestoreCheck(out object[] args)
    {
        args = null;
        return true;
    }

    bool OnPlayerLevelUpCheck(out object[] args)
    {
        args = new object[] { actorNumber };
        return true;
    }

    bool OnToastCheck(out object[] args)
    {
        args = m_args;
        return true;
    }

    #endregion

}
