using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CharBase : MonoBehaviour
{

    
    [SerializeField]
    /// <summary>
    /// ����ʱ�Ÿ���Ķ���ID ���ڿ���������Ϊָ��ID��ӻ�ɾ���ض�Ч��
    /// </summary>
    private int runId = -1;
    [SerializeField]
    /// <summary>
    /// �������
    /// </summary>
    private string playerName;
    [SerializeField]
    /// <summary>
    /// ����
    /// </summary>
    private TeamEnum.playerTeam playerTeam;
    [SerializeField]
    /// <summary>
    /// ��ɱ��
    /// </summary>
    private int kill = 0;
    [SerializeField]
    /// <summary>
    /// ������
    /// </summary>
    private int death = 0;
    [SerializeField]
    /// <summary>
    /// ��ɫ��ӵ�еĽ��
    /// </summary>
    private int money = 0;
    [SerializeField]
    /// <summary>
    /// ��ǰ����
    /// </summary>
    private float exp = 0;
    [SerializeField]
    /// <summary>
    /// ���辭��
    /// </summary>
    private float maxExp = 500;
    [SerializeField]
    /// <summary>
    /// ��ɫ״̬
    /// </summary>
    private CharEnum.StateEnum state;
    [SerializeField]
    /// <summary>
    /// ��ǰ��ɫ����������buff
    /// </summary>
    private List<CharEnum.BuffEnum> buff;
    [SerializeField]
    /// <summary>
    /// ��ɫְҵ
    /// </summary>
    private CharEnum.ProEnum pro;
    [SerializeField]
    /// <summary>
    /// �ȼ�
    /// </summary>
    private int level = 1;
    [SerializeField]
    /// <summary>
    /// ����
    /// </summary>
    private float attack = 100;
    [SerializeField]
    /// <summary>
    /// �������
    /// </summary>
    private float maxHealth = 1000;
    [SerializeField]
    /// <summary>
    /// ��ǰ����
    /// </summary>
    private float health = 1000;
    [SerializeField]
    /// <summary>
    /// ����
    /// </summary>
    private float criticalHit = 0.5f;
    [SerializeField]
    /// <summary>
    /// ����(��)
    /// </summary>
    private float criticalHitRate = 0.05f;
    [SerializeField]
    /// <summary>
    /// ����
    /// </summary>
    private float defence = 1000;
    [SerializeField]
    /// <summary>
    /// ����
    /// </summary>
    private float attackSpeed = 1f;
    [SerializeField]
    /// <summary>
    /// �����ָ�
    /// </summary>
    private float restore = 20f;
    [SerializeField]
    /// <summary>
    /// Q����
    /// </summary>
    private SkillEnum.skillID skillQ;
    [SerializeField]
    /// <summary>
    /// E����
    /// </summary>
    private SkillEnum.skillID skillE;
    [SerializeField]
    /// <summary>
    /// R����
    /// </summary>
    private SkillEnum.skillID skillR;
    [SerializeField]
    /// <summary>
    /// Space����
    /// </summary>
    private SkillEnum.skillBurst skillBurst;
    [SerializeField]
    /// <summary>
    /// ͷ��
    /// </summary>
    private EquipEnum.EquipID headID;
    [SerializeField]
    /// <summary>
    /// ����
    /// </summary>
    private EquipEnum.EquipID armorID;
    [SerializeField]
    /// <summary>
    /// ����
    /// </summary>
    private EquipEnum.EquipID handID;
    [SerializeField]
    /// <summary>
    /// ��ϥ
    /// </summary>
    private EquipEnum.EquipID kneeID;
    [SerializeField]
    /// <summary>
    /// ����
    /// </summary>
    private EquipEnum.EquipID trousersID;
    [SerializeField]
    /// <summary>
    /// Ь��
    /// </summary>
    private EquipEnum.EquipID shoesID;
    [SerializeField]
    /// <summary>
    /// �ƶ��ٶ�
    /// </summary>
    private float moveSpeed;
    [SerializeField]
    /// <summary>
    /// ������Χ
    /// </summary>
    private float attackRange;
    [SerializeField]
    /// <summary>
    /// ����ʱ��
    /// </summary>
    private float respawnTime;
    [SerializeField]
    /// <summary>
    /// ��������ʱ
    /// </summary>
    private float respawnCountDown;
    [SerializeField]
    /// <summary>
    /// �Ƿ�������������ʱ
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
                CharManager.instance.Log(runId, "ɱ�������Ϸ�");
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
                CharManager.instance.Log(runId, "���������Ϸ�");
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
                CharManager.instance.Log(runId, "��Ǯ���Ϸ�");
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
                CharManager.instance.Log(runId, "���鲻�Ϸ�");
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
                CharManager.instance.Log(runId, "�����ֵ���Ϸ�");
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
                CharManager.instance.Log(runId, "�ȼ����Ϸ�");
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
                CharManager.instance.Log(runId, "���������Ϸ�");
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
                CharManager.instance.Log(runId, "���Ѫ�����Ϸ�");
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
                CharManager.instance.Log(runId, "Ѫ�����Ϸ�");
            }
            else if (value >maxHealth)
            {
                CharManager.instance.Log(runId, "Ѫ�����ó������Ѫ��");
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
                CharManager.instance.Log(runId, "�������Ϸ�");
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
                CharManager.instance.Log(runId, "���ٲ��Ϸ�");
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
                CharManager.instance.Log(runId, "��Ѫ�ٶȲ��Ϸ�");
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
                CharManager.instance.Log(runId, "���ٲ��Ϸ�");
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
                CharManager.instance.Log(runId, "������Χ���Ϸ�");
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
                CharManager.instance.Log(runId, "����ʱ�䲻�Ϸ�");
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
                CharManager.instance.Log(runId, "��������ʱ���Ϸ�");
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
