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
                m_args = new object[2] { actorNumber, "ɱ�������Ϸ�" };
                GameEventManager.EnableEvent(EventName.onToast, true);
                //CharManager.Instance.Toast(actorNumber, "ɱ�������Ϸ�");
            }
            else
            {
                if (kill == value) return;

                kill = value;
                GameEventManager.EnableEvent(EventName.onPlayerKill, true);
            }
     } }
    public int Death 
    { 
        get => death;
        set {
            if (value<0)
            {
                m_args = new object[2] { actorNumber, "���������Ϸ�" };
                GameEventManager.EnableEvent(EventName.onToast,true);
                //CharManager.Instance.Toast(actorNumber, "���������Ϸ�");
            }
            else
            {
                if(death == value) return;

                death = value;
                GameEventManager.EnableEvent(EventName.onPlayerKilled, true);
            }
        } }
    public int Money 
    { 
        get => money;
        set {
            if (value<0 || value > 999999)
            {
                m_args = new object[2] { actorNumber, "��Ǯ���Ϸ�" };
                GameEventManager.EnableEvent(EventName.onToast, true);
                //CharManager.Instance.Toast(actorNumber, "��Ǯ���Ϸ�");
            }
            else
            {
                if(money == value) return;

                money = value;
                GameEventManager.EnableEvent(EventName.onPlayerMoneyChanged, true);
            }
    }}
    public float CurrentExp 
    { 
        get => currentExp;
        set {
            if (value <0)
            {
                m_args = new object[2] { actorNumber, "���鲻�Ϸ�" };
                GameEventManager.EnableEvent(EventName.onToast, true);
                //CharManager.Instance.Toast(actorNumber, "���鲻�Ϸ�");
            }
            else
            {
                if(currentExp == value) return;

                currentExp = value;
                if (currentExp>=maxExp && currentExp!=0)
                {

                    GameEventManager.EnableEvent(EventName.onPlayerLevelUp, true);

                }
                else
                {
                    GameEventManager.EnableEvent(EventName.onPlayerLevelUp, false);
                }
            }
        }}
    public float MaxExp
    {
        get => maxExp;
        set
        {
            if (value < 0)
            {
                m_args = new object[2] { actorNumber, "�����ֵ���Ϸ�" };
                GameEventManager.EnableEvent(EventName.onToast, true);
                //CharManager.Instance.Toast(actorNumber, "�����ֵ���Ϸ�");
            }
            else
            {
                if(maxExp == value) return;

                maxExp = value;
                GameEventManager.EnableEvent(EventName.onPlayerMaxExpChanged, true);
            }
        }
    }
    public StateEnum State { 
        get => state;
        set{
            if(state == value) return;

            state = value;
            GameEventManager.EnableEvent(EventName.onPlayerStateChanged, true);
        }  
    }
    public Dictionary<int,BuffEnum> Buff { 
        get => buff;
        set {
            if(buff == value) return;

            buff = value;
            GameEventManager.EnableEvent(EventName.onPlayerBuffChanged, true);
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
                m_args = new object[2] { actorNumber, "�ȼ����Ϸ�" };
                GameEventManager.EnableEvent(EventName.onToast, true);
                //CharManager.Instance.Toast(actorNumber, "�ȼ����Ϸ�");
            }
            else
            {
                if(level == value) return;

                level = value;
                GameEventManager.EnableEvent(EventName.onPlayerLevelChanged, true);
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
                m_args = new object[2] { actorNumber, "���������Ϸ�" };
                GameEventManager.EnableEvent(EventName.onToast, true);
                //CharManager.Instance.Toast(actorNumber, "���������Ϸ�");
            }
            else
            {
                if(attack == value) return;

                attack = value;
                GameEventManager.EnableEvent(EventName.onPlayerAttackChanged, true);
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
                m_args = new object[2] { actorNumber, "���ܲ��Ϸ�,��ʹ����Ϊ0" };
                GameEventManager.EnableEvent(EventName.onToast, true);
                //CharManager.Instance.Toast(actorNumber, "���ܲ��Ϸ�,��ʹ����Ϊ0");
            }
            else
            {
                if(shield == value) return;

                shield = value;
                GameEventManager.EnableEvent(EventName.onPlayerShieldChanged, true);
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
                if (State == StateEnum.Alive)
                {
                    if (Buff.TryGetValue((int)BuffEnum.Coreless,out BuffEnum buffEnum))
                    {
                        GameEventManager.EnableEvent(EventName.onPlayerDead, true);
                    }
                    else
                    {
                        m_args = new object[2] { actorNumber, "Ѫ�����Ϸ�,��ʹ�������" };

                        GameEventManager.EnableEvent(EventName.onPlayerKilled, true);
                        GameEventManager.EnableEvent(EventName.onToast, true);
                        //CharManager.Instance.Toast(actorNumber, "Ѫ�����Ϸ�,��ʹ�������");
                    }
                }

            }
            else if (value >MaxHealth)
            {
                currentHealth = MaxHealth;
                m_args = new object[2] { actorNumber, "Ѫ�����ó������Ѫ��" };
                GameEventManager.EnableEvent(EventName.onToast, true);
                //CharManager.Instance.Toast(actorNumber, "Ѫ�����ó������Ѫ��");
            }
            else
            {
                if (currentHealth == value) return;

                currentHealth = value;
                State = StateEnum.Alive;

                GameEventManager.EnableEvent(EventName.onPlayerCurrentHealthChanged, true);
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
                m_args = new object[2] { actorNumber, "���Ѫ�����Ϸ�,�Ѵﵽ���ֵ" };
                GameEventManager.EnableEvent(EventName.onToast, true);
                //CharManager.Instance.Toast(actorNumber, "���Ѫ�����Ϸ�,�Ѵﵽ���ֵ");
            }
            else
            {
                if (maxHealth == value) return;

                maxHealth = value;
                GameEventManager.EnableEvent(EventName.onPlayerMaxHealthChanged, true);
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
                GameEventManager.EnableEvent(EventName.onPlayerCriticalHitChanged, true);
            }
            else
            {
                if(criticalHit == value) return;

                criticalHit = value;
                GameEventManager.EnableEvent(EventName.onPlayerCriticalHitChanged, true);
            }

        }
    }
    public float CriticalHitRate
    {
        get => criticalHitRate;
        set
        {
            if(criticalHitRate == value) return;

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

            GameEventManager.EnableEvent(EventName.onPlayerCriticalHitRateChanged, true);
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
                m_args = new object[2] { actorNumber, "�������Ϸ�,��Ϊ���ֵ" };
                GameEventManager.EnableEvent(EventName.onToast, true);
                //CharManager.Instance.Toast(actorNumber, "�������Ϸ�,��Ϊ���ֵ");
            }
            else
            {
                if(defence == value) return;

                defence = value;
                GameEventManager.EnableEvent(EventName.onPlayerDefenceChanged, true);
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
                m_args = new object[2] { actorNumber, "���ٲ��Ϸ�,��Ϊ���ֵ" };
                GameEventManager.EnableEvent(EventName.onToast, true);
                //CharManager.Instance.Toast(actorNumber, "���ٲ��Ϸ�,��Ϊ���ֵ");
            }
            else
            {
                if(attackSpeed == value) return;

                attackSpeed = value;
                GameEventManager.EnableEvent(EventName.onPlayerAttackSpeedChanged, true);
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
                m_args = new object[2] { actorNumber, "��Ѫ�ٶȲ��Ϸ�" };
                GameEventManager.EnableEvent(EventName.onToast, true);
                //CharManager.Instance.Toast(actorNumber, "��Ѫ�ٶȲ��Ϸ�");
            }
            else
            {
                if(restore == value) return;

                restore = value;
                GameEventManager.EnableEvent(EventName.onPlayerRestoreChanged, true);
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
                m_args = new object[2] { actorNumber, "���ٲ��Ϸ�" };
                GameEventManager.EnableEvent(EventName.onToast, true);
                //CharManager.Instance.Toast(actorNumber, "���ٲ��Ϸ�");
            }
            else
            {
                if (moveSpeed == value) return;

                moveSpeed = value;
                GameEventManager.EnableEvent(EventName.onPlayerMoveSpeedChanged, true);
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
                m_args = new object[2] { actorNumber, "������Χ���Ϸ�" };
                GameEventManager.EnableEvent(EventName.onToast, true);
                //CharManager.Instance.Toast(actorNumber, "������Χ���Ϸ�");
            }
            else
            {
                if(attackRange == value) return;

                attackRange = value;
                GameEventManager.EnableEvent(EventName.onPlayerAttackRangeChanged, true);
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
                m_args = new object[2] { actorNumber, "����ʱ�䲻�Ϸ�" };
                GameEventManager.EnableEvent(EventName.onToast, true);
                //CharManager.Instance.Toast(actorNumber, "����ʱ�䲻�Ϸ�");
            }
            else if (respawnTime+value>60f)
            {
                respawnTime = 60f;
                GameEventManager.EnableEvent(EventName.onPlayerRespawnTimeChanged, true);
            }
            else
            {
                if(respawnTime == value) return;

                respawnTime = value;
                GameEventManager.EnableEvent(EventName.onPlayerRespawnTimeChanged, true);
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
                m_args = new object[2] { actorNumber, "��������ʱ���Ϸ�" };
                GameEventManager.EnableEvent(EventName.onToast, true);
                //CharManager.Instance.Toast(actorNumber, "��������ʱ���Ϸ�");
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
    #endregion

    #region Unity Functions

    void Awake()
    {

    }

    void Start()
    {
        GameEventManager.RegisterEvent(EventName.onPlayerLevelUp, OnPlayerLevelUpCheck);

        GameEventManager.RegisterEvent(EventName.onToast, OnToastCheck);

        GameEventManager.RegisterEvent(EventName.onPlayerKill, OnPlayerKillCheck);

        GameEventManager.RegisterEvent(EventName.onPlayerLevelChanged, OnPlayerLevelChangedCheck);

        GameEventManager.RegisterEvent(EventName.onPlayerRespawnTimeChanged, OnPlayerRespawnTimeChangedCheck);

        GameEventManager.RegisterEvent(EventName.onPlayerRespawnCountDownStart, OnPlayerRespawnCountDownStartCheck);

        GameEventManager.RegisterEvent(EventName.onPlayerRespawnCountDownEnd, OnPlayerRespawnCountDownEndCheck);

    }

    #endregion

    #region Event Check

    bool OnPlayerKillCheck(out object[] args)
    {
        args = null;
        return true;
    }

    bool OnPlayerLevelChangedCheck(out object[] args)
    {
        args = null;
        return true;
    }

    bool OnPlayerKilledCheck(out object[] args)
    {
        args = null;
        return true;
    }

    bool OnPlayerDeadCheck(out object[] args)
    {
        args = null;
        return true;
    }

    bool OnPlayerRespawnCheck(out object[] args)
    {
        args = null;
        return true;
    }

    bool OnPlayerRespawnTimeChangedCheck(out object[] args)
    {
        args=null;
        return true;
    }

    bool OnPlayerStateChangedCheck(out object[] args)
    {
        args = null;
        return true;
    }

    bool OnPlayerRespawnCountDownStartCheck(out object[] args)
    {
        args = new object[0];
        return true;
    }

    bool OnPlayerRespawnCountDownEndCheck(out object[] args)
    {
        args = new object[0];
        return true;
    }

    bool OnPlayerRestoreCheck(out object[] args)
    {
        args = null;
        return true;
    }

    bool OnPlayerLevelUpCheck(out object[] args)
    {
        args = new object[2] { actorNumber, 1 };
        return true;
    }

    bool OnToastCheck(out object[] args)
    {
        args = m_args;
        return true;
    }

    #endregion

    
}
