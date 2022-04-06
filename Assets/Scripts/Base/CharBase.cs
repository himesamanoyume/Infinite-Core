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
    [SerializeField]
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
    [SerializeField]
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
    [SerializeField]
    /// <summary>
    /// 当前经验
    /// </summary>
    private float currentExp = 0f;
    [SerializeField]
    /// <summary>
    /// 所需经验
    /// </summary>
    private float maxExp = 500f;
    [SerializeField]
    /// <summary>
    /// 角色状态
    /// </summary>
    private StateEnum state = StateEnum.NonLife;
    [SerializeField]
    /// <summary>
    /// 当前角色身上所带的buff
    /// </summary>
    private Dictionary<int,BuffEnum> buff;
    [SerializeField]
    /// <summary>
    /// 角色职业
    /// </summary>
    private ProEnum pro;
    [SerializeField]
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
    [SerializeField]
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
    [SerializeField]
    /// <summary>
    /// 移动速度
    /// </summary>
    private float moveSpeed = 10f;
    [SerializeField]
    /// <summary>
    /// 攻击范围
    /// </summary>
    private float attackRange = 10f;
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

    #endregion

    #region Public Functions

    public int ActorNumber { get => actorNumber; set => actorNumber = value; }
    public string PlayerName { get => playerName; set => playerName = value; }
    public int Kill 
    { 
        get => kill;
        set {
            if (value<0)
            {
                CharManager.Instance.Log(actorNumber, "杀敌数不合法");
            }
            else
            {
                kill = value;
            }
     } }
    public int Death 
    { 
        get => death;
        set {
            if (value<0)
            {
                CharManager.Instance.Log(actorNumber, "死亡数不合法");
            }
            else
            {
                death = value;
            }
        } }
    public int Money 
    { 
        get => money;
        set {
            if (value<0 || value > 999999)
            {
                CharManager.Instance.Log(actorNumber, "金钱不合法");
            }
            else
            {
                money = value;
            }
    }}
    public float CurrentExp 
    { 
        get => currentExp;
        set {
            if (value <0)
            {
                CharManager.Instance.Log(actorNumber, "经验不合法");
            }
            else
            {
                currentExp = value;
            }
        }}
    public float MaxExp
    {
        get => maxExp;
        set
        {
            if (value < 0)
            {
                CharManager.Instance.Log(actorNumber, "最大经验值不合法");
            }
            else
            {
                maxExp = value;
            }
        }
    }
    public StateEnum State { get => state; set => state = value; }
    public Dictionary<int,BuffEnum> Buff { get => buff; set => buff = value; }
    public ProEnum Pro { get => pro; set => pro = value; }
    public int Level
    {
        get => level;
        set
        {
            if (value < 0)
            {
                CharManager.Instance.Log(actorNumber, "等级不合法");
            }
            else
            {
                level = value;
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
                CharManager.Instance.Log(actorNumber, "攻击力不合法");
            }
            else
            {
                attack = value;
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
                CharManager.Instance.Log(actorNumber, "护盾不合法,已使护盾为0");
            }
            else
            {
                shield = value;
            }
        }
    }
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            if (value < 0)
            {
                currentHealth = 0;
                CharManager.Instance.Log(actorNumber, "血量不合法,已使玩家死亡");
            }
            else if (value >maxHealth)
            {
                currentHealth = maxHealth;
                CharManager.Instance.Log(actorNumber, "血量不得超过最大血量");
            }
            else
            {
                currentHealth = value;
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
                CharManager.Instance.Log(actorNumber, "最大血量不合法,已达到最低值");
            }
            else
            {
                maxHealth = value;
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
            }
            else
            {
                criticalHit = value;
            }
        }
    }
    public float CriticalHitRate
    {
        get => criticalHitRate;
        set
        {
            if (value < 0.05f)
            {
                criticalHitRate = 0.05f;
            }
            else if (value>1)
            {
                criticalHitRate = 1;
            }
            else
            {
                criticalHitRate = value;
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
                CharManager.Instance.Log(actorNumber, "防御不合法,已为最低值");
            }
            else
            {
                defence = value;
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
                CharManager.Instance.Log(actorNumber, "攻速不合法,已为最低值");
            }
            else
            {
                attackSpeed = value;
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
                CharManager.Instance.Log(actorNumber, "回血速度不合法");
            }
            else
            {
                restore = value;
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
                CharManager.Instance.Log(actorNumber, "移速不合法");
            }
            else
            {
                moveSpeed = value;
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
                CharManager.Instance.Log(actorNumber, "攻击范围不合法");
            }
            else
            {
                attackRange = value;
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
                CharManager.Instance.Log(actorNumber, "重生时间不合法");
            }
            else if (respawnTime+value>60f)
            {
                //respawnTime = 60f;
            }
            else
            {
                respawnTime = value;
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
                CharManager.Instance.Log(actorNumber, "重生倒计时不合法");
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

    public void OnCharBaseUpdate()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
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
            stream.SendNext(Pro);
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
            // Network player, receive datastream.SendNext(ActorNumber);
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
            Pro = (ProEnum)stream.ReceiveNext();
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
    #endregion

    #region Unity Functions

    void Awake()
    {

    }

    void Start()
    {
        
    }

    #endregion
}
