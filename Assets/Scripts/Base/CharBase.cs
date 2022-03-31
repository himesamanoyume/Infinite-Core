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
    private TeamEnum playerTeam = TeamEnum.Null;
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
    private float currentExp = 0f;
    [SerializeField]
    /// <summary>
    /// ���辭��
    /// </summary>
    private float maxExp = 500f;
    [SerializeField]
    /// <summary>
    /// ��ɫ״̬
    /// </summary>
    private StateEnum state = StateEnum.NonLife;
    [SerializeField]
    /// <summary>
    /// ��ǰ��ɫ����������buff
    /// </summary>
    private List<BuffEnum> buff;
    [SerializeField]
    /// <summary>
    /// ��ɫְҵ
    /// </summary>
    private ProEnum pro;
    [SerializeField]
    /// <summary>
    /// �ȼ�
    /// </summary>
    private int level = 1;
    [SerializeField]
    /// <summary>
    /// ����
    /// </summary>
    private float attack = 100f;
    [SerializeField]
    /// <summary>
    /// ����ֵ
    /// </summary>
    private float shield = 0f;
    [SerializeField]
    /// <summary>
    /// �������
    /// </summary>
    private float maxHealth = 1000f;
    [SerializeField]
    /// <summary>
    /// ��ǰ����
    /// </summary>
    private float currentHealth = 1000f;
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
    private float defence = 1000f;
    [SerializeField]
    /// <summary>
    /// ����
    /// </summary>
    private float attackSpeed = 1f;
    [SerializeField]
    /// <summary>
    /// �����ָ�
    /// </summary>
    private float restore = 10f;
    [SerializeField]
    /// <summary>
    /// Q����
    /// </summary>
    private SkillQ skillQ;
    [SerializeField]
    /// <summary>
    /// E����
    /// </summary>
    private SkillE skillE;
    [SerializeField]
    /// <summary>
    /// R����
    /// </summary>
    private SkillR skillR;
    [SerializeField]
    /// <summary>
    /// Space����
    /// </summary>
    private SkillBurst skillBurst;
    [SerializeField]
    /// <summary>
    /// ͷ��
    /// </summary>
    private EquipHead headID;
    [SerializeField]
    /// <summary>
    /// ����
    /// </summary>
    private EquipArmor armorID;
    [SerializeField]
    /// <summary>
    /// ����
    /// </summary>
    private EquipHand handID;
    [SerializeField]
    /// <summary>
    /// ��ϥ
    /// </summary>
    private EquipKnee kneeID;
    [SerializeField]
    /// <summary>
    /// ����
    /// </summary>
    private EquipTrousers trousersID;
    [SerializeField]
    /// <summary>
    /// Ь��
    /// </summary>
    private EquipBoots shoesID;
    [SerializeField]
    /// <summary>
    /// �ƶ��ٶ�
    /// </summary>
    private float moveSpeed = 10f;
    [SerializeField]
    /// <summary>
    /// ������Χ
    /// </summary>
    private float attackRange = 10f;
    [SerializeField]
    /// <summary>
    /// ��������ʱ��
    /// </summary>
    private float respawnTime = 10f;
    [SerializeField]
    /// <summary>
    /// ��������ʱ
    /// </summary>
    private float respawnCountDown = 10f;

    public CharBase(int runId, string playerName, ProEnum pro, SkillQ skillQ, SkillE skillE, SkillR skillR, SkillBurst skillBurst, TeamEnum playerTeam)
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
                CharManager.Instance.Log(runId, "ɱ�������Ϸ�");
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
                CharManager.Instance.Log(runId, "���������Ϸ�");
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
                CharManager.Instance.Log(runId, "��Ǯ���Ϸ�");
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
                CharManager.Instance.Log(runId, "���鲻�Ϸ�");
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
                CharManager.Instance.Log(runId, "�����ֵ���Ϸ�");
            }
            else
            {
                maxExp = value;
            }
        }
    }
    public StateEnum State { get => state; set => state = value; }
    public List<BuffEnum> Buff { get => buff; set => buff = value; }
    public ProEnum Pro { get => pro; set => pro = value; }
    public int Level
    {
        get => level;
        set
        {
            if (value < 0)
            {
                CharManager.Instance.Log(runId, "�ȼ����Ϸ�");
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
                CharManager.Instance.Log(runId, "���������Ϸ�");
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
                CharManager.Instance.Log(runId, "���ܲ��Ϸ�,��ʹ����Ϊ0");
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
                CharManager.Instance.Log(runId, "Ѫ�����Ϸ�,��ʹ�������");
            }
            else if (value >maxHealth)
            {
                currentHealth = maxHealth;
                CharManager.Instance.Log(runId, "Ѫ�����ó������Ѫ��");
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
                CharManager.Instance.Log(runId, "���Ѫ�����Ϸ�,�Ѵﵽ���ֵ");
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
                CharManager.Instance.Log(runId, "�������Ϸ�,��Ϊ���ֵ");
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
                CharManager.Instance.Log(runId, "���ٲ��Ϸ�,��Ϊ���ֵ");
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
                CharManager.Instance.Log(runId, "��Ѫ�ٶȲ��Ϸ�");
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
    public EquipBoots ShoesID { get => shoesID; set => shoesID = value; }
    public float MoveSpeed
    {
        get => moveSpeed;
        set
        {
            if (value < 0)
            {
                CharManager.Instance.Log(runId, "���ٲ��Ϸ�");
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
                CharManager.Instance.Log(runId, "������Χ���Ϸ�");
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
                CharManager.Instance.Log(runId, "����ʱ�䲻�Ϸ�");
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
                CharManager.Instance.Log(runId, "��������ʱ���Ϸ�");
            }
            else
            {
                respawnCountDown = value;
            }
        }
    }
    public TeamEnum PlayerTeam { get => playerTeam; set => playerTeam = value; }


    
}
