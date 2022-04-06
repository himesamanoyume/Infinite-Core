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
    /// ����ʱ�Ÿ���Ķ���ID ���ڿ���������Ϊָ��ID��ӻ�ɾ���ض�Ч��
    /// </summary>
    private int actorNumber = -1;
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
    private Dictionary<int,BuffEnum> buff;
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
    private EquipBoots bootsID;
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
                CharManager.Instance.Log(actorNumber, "ɱ�������Ϸ�");
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
                CharManager.Instance.Log(actorNumber, "���������Ϸ�");
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
                CharManager.Instance.Log(actorNumber, "��Ǯ���Ϸ�");
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
                CharManager.Instance.Log(actorNumber, "���鲻�Ϸ�");
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
                CharManager.Instance.Log(actorNumber, "�����ֵ���Ϸ�");
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
                CharManager.Instance.Log(actorNumber, "�ȼ����Ϸ�");
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
                CharManager.Instance.Log(actorNumber, "���������Ϸ�");
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
                CharManager.Instance.Log(actorNumber, "���ܲ��Ϸ�,��ʹ����Ϊ0");
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
                CharManager.Instance.Log(actorNumber, "Ѫ�����Ϸ�,��ʹ�������");
            }
            else if (value >maxHealth)
            {
                currentHealth = maxHealth;
                CharManager.Instance.Log(actorNumber, "Ѫ�����ó������Ѫ��");
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
                CharManager.Instance.Log(actorNumber, "���Ѫ�����Ϸ�,�Ѵﵽ���ֵ");
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
                CharManager.Instance.Log(actorNumber, "�������Ϸ�,��Ϊ���ֵ");
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
                CharManager.Instance.Log(actorNumber, "���ٲ��Ϸ�,��Ϊ���ֵ");
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
                CharManager.Instance.Log(actorNumber, "��Ѫ�ٶȲ��Ϸ�");
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
                CharManager.Instance.Log(actorNumber, "���ٲ��Ϸ�");
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
                CharManager.Instance.Log(actorNumber, "������Χ���Ϸ�");
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
                CharManager.Instance.Log(actorNumber, "����ʱ�䲻�Ϸ�");
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
                CharManager.Instance.Log(actorNumber, "��������ʱ���Ϸ�");
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
