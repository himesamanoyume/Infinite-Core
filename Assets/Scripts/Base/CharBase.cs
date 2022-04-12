using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon.Realtime;
using Photon.Pun;

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
    private Dictionary<int,BuffEnum> buff;
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
    private EquipHead headID;
    [SerializeField]
    /// <summary>
    /// 护甲
    /// </summary>
    private EquipArmor armorID;
    [SerializeField]
    /// <summary>
    /// 护手
    /// </summary>
    private EquipHand handID;
    [SerializeField]
    /// <summary>
    /// 护膝
    /// </summary>
    private EquipKnee kneeID;
    [SerializeField]
    /// <summary>
    /// 护腿
    /// </summary>
    private EquipTrousers trousersID;
    [SerializeField]
    /// <summary>
    /// 鞋子
    /// </summary>
    private EquipBoots bootsID;



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
                    GameEventManager.EnableEvent(EventEnum.OnPlayerKill, true);
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
    public EquipHead HeadID { get => headID; set => headID = value; }
    public EquipArmor ArmorID { get => armorID; set => armorID = value; }
    public EquipHand HandID { get => handID; set => handID = value; }
    public EquipKnee KneeID { get => kneeID; set => kneeID = value; }
    public EquipTrousers TrousersID { get => trousersID; set => trousersID = value; }
    public EquipBoots BootsID { get => bootsID; set => bootsID = value; }

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
            stream.SendNext(Buff);
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
            stream.SendNext(HeadID);
            stream.SendNext(ArmorID);
            stream.SendNext(HandID);
            stream.SendNext(KneeID);
            stream.SendNext(TrousersID);
            stream.SendNext(BootsID);
            stream.SendNext(MoveSpeed);
            stream.SendNext(AttackRange);
            stream.SendNext(RespawnTime);
            stream.SendNext(RespawnCountDown);

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
            Buff = (Dictionary<int,BuffEnum>)stream.ReceiveNext();
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
            HeadID = (EquipHead)stream.ReceiveNext();
            ArmorID = (EquipArmor)stream.ReceiveNext();
            HandID = (EquipHand)stream.ReceiveNext();
            KneeID = (EquipKnee)stream.ReceiveNext();
            TrousersID = (EquipTrousers)stream.ReceiveNext();
            BootsID = (EquipBoots)stream.ReceiveNext();
            MoveSpeed = (float)stream.ReceiveNext();
            AttackRange = (float)stream.ReceiveNext();
            RespawnTime = (float)stream.ReceiveNext();
            RespawnCountDown = (float)stream.ReceiveNext();

        }
    }

    [PunRPC]
    public void DestroyPlayerModel(int actorNumber)
    {
        //if (photonView == PhotonNetwork.IsMasterClient)
        //{
            CharManager.Instance.FindPlayerModel(actorNumber, out GameObject playerModel);
            if (playerModel == null) { return; }
            
            Debug.LogWarning(playerModel.name+"已网络销毁玩家模型");

            CharManager.Instance.playerModelList.Remove(actorNumber);
            PhotonNetwork.Destroy(playerModel);
        //}

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

            GameEventManager.RegisterEvent(EventEnum.OnPlayerKilled, OnPlayerKilledCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerMaxHealthChanged, OnPlayerMaxHealthChangedCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerCurrentHealthChanged, OnPlayerCurrentHealthChangedCheck);

            GameEventManager.RegisterEvent(EventEnum.OnPlayerLevelChanged, OnPlayerLevelChangedCheck);

            GameEventManager.RegisterEvent(EventEnum.SendPlayerMoveSpeed, SendPlayerMoveSpeedCheck);
            //GameEventManager.EnableEvent(EventEnum.SendPlayerMoveSpeed, true);
            
            GameEventManager.RegisterEvent(EventEnum.AllowPlayerMove, PlayerMoveCheck);

            GameEventManager.RegisterEvent(EventEnum.AllowPlayerAttack, PlayerAttackCheck);

            GameEventManager.RegisterEvent(EventEnum.AllowPlayerTowardChanged, PlayerTowardChangedCheck);
            
        }

        #endregion
    }

    #endregion

    #region Event Check

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
        args = new object[] { ActorNumber };
        return true;
    }

    bool OnPlayerMaxHealthChangedCheck(out object[] args)
    {
        args = new object[] {ActorNumber};
        return true;
    }

    bool OnPlayerKillCheck(out object[] args)
    {
        args = null;
        return true;
    }

    bool OnPlayerRestoreingCheck(out object[] args)
    {
        args = new object[] { ActorNumber};
        return true;
    }

    bool OnPlayerKilledCheck(out object[] args)
    {
        args = new object[] { ActorNumber };
        return true;
    }

    bool OnPlayerDeadCheck(out object[] args)
    {
        args = new object[] { ActorNumber };
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
        args =new object[] { ActorNumber } ;
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
