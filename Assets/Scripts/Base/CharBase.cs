using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CharBase : MonoBehaviour
{

    
    [SerializeField]
    /// <summary>
    /// 运行时才赋予的独特ID 用于控制器快速为指定ID添加或删除特定效果
    /// </summary>
    private int runId = -1;
    [SerializeField]
    /// <summary>
    /// 玩家姓名
    /// </summary>
    private string playerName;
    [SerializeField]
    /// <summary>
    /// 队伍
    /// </summary>
    private TeamEnum.playerTeam playerTeam;
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
    private float exp = 0;
    [SerializeField]
    /// <summary>
    /// 所需经验
    /// </summary>
    private float maxExp = 500;
    [SerializeField]
    /// <summary>
    /// 角色状态
    /// </summary>
    private CharEnum.StateEnum state;
    [SerializeField]
    /// <summary>
    /// 当前角色身上所带的buff
    /// </summary>
    private List<CharEnum.BuffEnum> buff;
    [SerializeField]
    /// <summary>
    /// 角色职业
    /// </summary>
    private CharEnum.ProEnum pro;
    [SerializeField]
    /// <summary>
    /// 等级
    /// </summary>
    private int level = 1;
    [SerializeField]
    /// <summary>
    /// 攻击
    /// </summary>
    private float attack = 100;
    [SerializeField]
    /// <summary>
    /// 最大生命
    /// </summary>
    private float maxHealth = 1000;
    [SerializeField]
    /// <summary>
    /// 当前生命
    /// </summary>
    private float health = 1000;
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
    private float defence = 1000;
    [SerializeField]
    /// <summary>
    /// 攻速
    /// </summary>
    private float attackSpeed = 1f;
    [SerializeField]
    /// <summary>
    /// 生命恢复
    /// </summary>
    private float restore = 20f;
    [SerializeField]
    /// <summary>
    /// Q技能
    /// </summary>
    private SkillEnum.skillID skillQ;
    [SerializeField]
    /// <summary>
    /// E技能
    /// </summary>
    private SkillEnum.skillID skillE;
    [SerializeField]
    /// <summary>
    /// R技能
    /// </summary>
    private SkillEnum.skillID skillR;
    [SerializeField]
    /// <summary>
    /// Space技能
    /// </summary>
    private SkillEnum.skillBurst skillBurst;
    [SerializeField]
    /// <summary>
    /// 头盔
    /// </summary>
    private EquipEnum.EquipID headID;
    [SerializeField]
    /// <summary>
    /// 护甲
    /// </summary>
    private EquipEnum.EquipID armorID;
    [SerializeField]
    /// <summary>
    /// 护手
    /// </summary>
    private EquipEnum.EquipID handID;
    [SerializeField]
    /// <summary>
    /// 护膝
    /// </summary>
    private EquipEnum.EquipID kneeID;
    [SerializeField]
    /// <summary>
    /// 护腿
    /// </summary>
    private EquipEnum.EquipID trousersID;
    [SerializeField]
    /// <summary>
    /// 鞋子
    /// </summary>
    private EquipEnum.EquipID shoesID;
    [SerializeField]
    /// <summary>
    /// 移动速度
    /// </summary>
    private float moveSpeed;
    [SerializeField]
    /// <summary>
    /// 攻击范围
    /// </summary>
    private float attackRange;
    [SerializeField]
    /// <summary>
    /// 重生时间
    /// </summary>
    private float respawnTime;
    [SerializeField]
    /// <summary>
    /// 重生倒计时
    /// </summary>
    private float respawnCountDown;
    [SerializeField]
    /// <summary>
    /// 是否正在重生倒计时
    /// </summary>
    private bool isRespawn;

    public CharBase(int runId, string playerName, CharEnum.ProEnum pro, SkillEnum.skillID skillQ, SkillEnum.skillID skillE, SkillEnum.skillID skillR, SkillEnum.skillBurst skillBurst, TeamEnum.playerTeam playerTeam)
    {
        this.runId = runId;
        this.playerName = playerName;
        this.pro = pro;
        this.skillQ = skillQ;
        this.skillE = skillE;
        this.skillR = skillR;
        this.skillBurst = skillBurst;
        this.playerTeam = playerTeam;
    }

    public CharBase()
    {

    }

    public int RunId { get => runId; set => runId = value; }
    public string PlayerName { get => playerName; set => playerName = value; }
    public int Kill 
    { 
        get => kill;
        set {
            if (value<0)
            {
                CharManager.instance.Log(runId, "杀敌数不合法");
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
                CharManager.instance.Log(runId, "死亡数不合法");
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
                CharManager.instance.Log(runId, "金钱不合法");
            }
            else
            {
                money = value;
            }
    }}
    public float Exp 
    { 
        get => exp;
        set {
            if (value <0)
            {
                CharManager.instance.Log(runId, "经验不合法");
            }
            else
            {
                exp = value;
            }
        }}
    public float MaxExp
    {
        get => maxExp;
        set
        {
            if (value < 0)
            {
                CharManager.instance.Log(runId, "最大经验值不合法");
            }
            else
            {
                maxExp = value;
            }
        }
    }
    public CharEnum.StateEnum State { get => state; set => state = value; }
    public List<CharEnum.BuffEnum> Buff { get => buff; set => buff = value; }
    public CharEnum.ProEnum Pro { get => pro; set => pro = value; }
    public int Level
    {
        get => level;
        set
        {
            if (value < 0)
            {
                CharManager.instance.Log(runId, "等级不合法");
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
                CharManager.instance.Log(runId, "攻击力不合法");
            }
            else
            {
                attack = value;
            }
        }
    }
    public float MaxHealth
    {
        get => maxHealth;
        set
        {
            if (value < 0)
            {
                CharManager.instance.Log(runId, "最大血量不合法");
            }
            else
            {
                maxHealth = value;
            }
        }
    }
    public float Health
    {
        get => health;
        set
        {
            if (value < 0)
            {
                CharManager.instance.Log(runId, "血量不合法");
            }
            else if (value >maxHealth)
            {
                CharManager.instance.Log(runId, "血量不得超过最大血量");
            }
            else
            {
                health = value;
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
            if (value < 0)
            {
                criticalHitRate = 0;
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
                CharManager.instance.Log(runId, "防御不合法");
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
            if (value < 0)
            {
                CharManager.instance.Log(runId, "攻速不合法");
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
                CharManager.instance.Log(runId, "回血速度不合法");
            }
            else
            {
                restore = value;
            }
        }
    }
    public SkillEnum.skillID SkillQ { get => skillQ; set => skillQ = value; }
    public SkillEnum.skillID SkillE { get => skillE; set => skillE = value; }
    public SkillEnum.skillID SkillR { get => skillR; set => skillR = value; }
    public SkillEnum.skillBurst SkillBurst { get => skillBurst; set => skillBurst = value; }
    public EquipEnum.EquipID HeadID { get => headID; set => headID = value; }
    public EquipEnum.EquipID ArmorID { get => armorID; set => armorID = value; }
    public EquipEnum.EquipID HandID { get => handID; set => handID = value; }
    public EquipEnum.EquipID KneeID { get => kneeID; set => kneeID = value; }
    public EquipEnum.EquipID TrousersID { get => trousersID; set => trousersID = value; }
    public EquipEnum.EquipID ShoesID { get => shoesID; set => shoesID = value; }
    public float MoveSpeed
    {
        get => moveSpeed;
        set
        {
            if (value < 0)
            {
                CharManager.instance.Log(runId, "移速不合法");
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
                CharManager.instance.Log(runId, "攻击范围不合法");
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
                CharManager.instance.Log(runId, "重生时间不合法");
            }
            else
            {
                attackRange = value;
            }
        }
    }
    public float RespawnCountDown
    {
        get => respawnCountDown;
        set
        {
            if (value < 0)
            {
                CharManager.instance.Log(runId, "重生倒计时不合法");
            }
            else
            {
                attackRange = value;
            }
        }
    }
    public bool IsRespawn { get => isRespawn; set => isRespawn = value; }
    public TeamEnum.playerTeam PlayerTeam { get => playerTeam; set => playerTeam = value; }
}
