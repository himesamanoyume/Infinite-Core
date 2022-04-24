using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public enum MonsterStateType
{
    Idle, Chase, Attack, Back
}

public enum MonsterType
{
    PatrolMonster, WorldMonster, InfiniteCore
}

public class MonsterController : MonoBehaviour, IPunObservable
{
    #region Props
    private IState currentState;

    private Dictionary<MonsterStateType, IState> states = new Dictionary<MonsterStateType, IState>();

    public Paramater paramater;
    Paramater patrolMonster;
    Paramater worldMonster;
    Paramater infiniteCore;

    public float currentHealth;
    public float maxHealth;

    public int m_groupId = -1;
    public MonsterType monsterType;
    public GameObject monsterInfoBarPrefab;

    public GameObject monsterAttackCubePrefab;
    public GameObject equipItemPrefab;

    private GameObject myMonsterInfoBar;

    private PhotonView photonView;

    bool isGround;
    public Transform groundCheck;
    public LayerMask groundLayerMask;
    public LayerMask checkPlayerLayerMask;
    float groundCheckRadius = 0.2f;

    Vector3 velocity = Vector3.zero;
    public float gravity;

    [HideInInspector]
    public CharacterController controller;

    Transform targetCanvas;
    CharManager charManager;

    #endregion

    #region Unity Lifecycle

    void Start()
    {
        InitMonsterData();
        //--
        switch (monsterType)
        {
            case MonsterType.PatrolMonster:
                GetMonsterData(patrolMonster);
                break;
            case MonsterType.WorldMonster:
                GetMonsterData(worldMonster);
                break;
            case MonsterType.InfiniteCore:
                GetMonsterData(infiniteCore);
                break;
        }
        //-- init end

        photonView = GetComponent<PhotonView>();

        controller = GetComponent<CharacterController>();

        targetCanvas = GameObject.Find("PlayerInfoCanvas").transform;

        paramater.initPos = transform.position;

        states.Add(MonsterStateType.Idle, new IdleState(this));
        states.Add(MonsterStateType.Chase, new ChaseState(this));
        states.Add(MonsterStateType.Attack, new AttackState(this));
        states.Add(MonsterStateType.Back, new BackState(this));

        paramater.animator = GetComponent<Animator>();

        paramater.sphereCollider = transform.GetChild(0).GetComponent<SphereCollider>();

        maxHealth = paramater.health;
        currentHealth = paramater.health;

        myMonsterInfoBar = Instantiate(monsterInfoBarPrefab);
        myMonsterInfoBar.transform.SetParent(targetCanvas);
        myMonsterInfoBar.GetComponent<MonsterInfoBar>().InitMonsterInfoBar(transform, this);

        TransitionState(MonsterStateType.Idle);

        charManager = GameObject.Find("CharManager").GetComponent<CharManager>();
    }

    void Update()
    {
        if (m_groupId != -1)
        {
            transform.SetParent(GameObject.Find(monsterType.ToString() + "Group" + m_groupId).transform);
        }

        if (photonView.IsMine)
        {

            MonsterGravity();

            if (currentHealth < maxHealth && currentHealth > 0)
            {
                currentHealth += paramater.restore * Time.deltaTime;
            }
            else if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }
            else if (currentHealth < 0)
            {
                currentHealth = 0;
                RewardTheKiller(paramater.lastOneHurtActorNumber);
                PhotonNetwork.Destroy(gameObject);
            }

            currentState.OnUpdate();

            if (Vector3.Distance(transform.position, new Vector3(paramater.initPos.x, transform.position.y, paramater.initPos.z)) <= 0.1f && !paramater.isIdle && paramater.isBack)
            {
                TransitionState(MonsterStateType.Idle);
            }

            if (paramater.isChase && paramater.isChaseTarget && paramater.currentTarget)
            {
                //追击状态  有Bug
                controller.Move((paramater.currentTarget.transform.position - transform.position).normalized * Time.deltaTime * paramater.moveSpeed);

                MonsterTowardChanged(paramater.currentTarget.transform.position);
            }
            else if (paramater.isBack)
            {
                //返回状态
                controller.Move((new Vector3(paramater.initPos.x, transform.position.y, paramater.initPos.z) - transform.position).normalized * Time.deltaTime * paramater.moveSpeed);

                MonsterTowardChanged(paramater.initPos);
            }
            else if (paramater.isIdle)
            {

            }
        }
    }

    private void OnDestroy()
    {
        TransitionState(MonsterStateType.Idle);
        Destroy(myMonsterInfoBar);
    }

    #endregion

    #region Public Functions

    public void InitController(int id)
    {
        m_groupId = id;
    }

    public void AnimationEventOnMonsterAttack()
    {
        if (photonView.IsMine)
        {
            //if (this.photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber) return;

            float finalAttack = GetFinalAttack(paramater.attack, 0.8f, 0.05f, 1);

            InitMonsterAttackCubeData(transform.position, transform.rotation, finalAttack);
        }
    }

    public void TransitionState(MonsterStateType type)
    {
        if (currentState! != null)
            currentState.OnExit();
        currentState = states[type];
        currentState.OnEnter();
    }

    public void MonsterTowardChanged(Vector3 target)
    {

        gameObject.transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
    }

    public void GetMonsterData(Paramater presetParamater)
    {
        paramater = presetParamater;
        paramater.selfTransform = transform;
    }

    #endregion

    #region Private Functions

    IEnumerator ResetLastOneHurtActorNumber()
    {
        yield return new WaitForSeconds(10);
        paramater.lastOneHurtActorNumber = -2;
    }

    void RewardTheKiller(int killerActorNumber)
    {
        if (killerActorNumber >= 1)
        {
            PhotonView otherPhotonView = charManager.recorders[killerActorNumber].GetPhotonView();

            float awardExp = Random.Range(paramater.awardMinExp, paramater.awardMaxExp);
            int awardMoney = (int)Random.Range(paramater.awardMinMoney, paramater.awardMaxMoney);

            if (Random.Range(0,1f) <= paramater.awardEquipProb)
            {
                //Debug.LogWarning("Get Equip!");
                Instantiate(equipItemPrefab, GameObject.FindGameObjectWithTag("HeadContent").transform).GetComponent<EquipBase>().InitEquip();
            }

            otherPhotonView.RPC("GetMonsterAward",RpcTarget.AllViaServer, killerActorNumber, awardExp,  awardMoney);
        }
    }

    float GetFinalAttack(float t_attack, float t_criticalHit, float t_criticalHitRate, float t_ratio)
    {
        if (t_criticalHitRate == 1) return t_attack * 0.5f * t_criticalHit * t_ratio;

        if (Random.Range(0, 1f) <= t_criticalHitRate)
        {
            return t_attack * 0.5f * (1 + t_criticalHit) * t_ratio;
        }
        else
        {
            return t_attack * 0.5f * t_ratio;
        }

    }

    void InitMonsterAttackCubeData(Vector3 selfPos, Quaternion modelRotation, float finalAttack)
    {
        Vector3[] attackCubeData = SetAttackCubeData(
                       modelRotation * Vector3.forward,
                       new Vector3(1.5f, 1.5f, 2) * paramater.attackRange * 0.1f,
                       modelRotation * Vector3.forward,
                       new Vector3(1.5f, 1.5f, 2) * paramater.attackRange * 0.1f
                       );

        float[] attackCubeData2 = SetAttackCubeData2(
            0.3f,
            paramater.finalDamage,
            0,
            finalAttack
            );

        photonView.RPC("SpawnAttackCube", RpcTarget.All, selfPos, modelRotation, attackCubeData, attackCubeData2);
    }

    Vector3[] SetAttackCubeData(Vector3 initOffset, Vector3 initScale, Vector3 finalOffset, Vector3 finalScale)
    {
        Vector3[] data = new Vector3[4]
        {
            initOffset,
            initScale,
            finalOffset,
            finalScale
        };
        return data;
    }

    float[] SetAttackCubeData2(float initToFinalTime, float finalDamage, float activeTime, float finalAttack)
    {
        float[] data = new float[4]
        {
            initToFinalTime,
            finalDamage,
            activeTime,
            finalAttack
        };

        return data;
    }

    void MonsterGravity()
    {
        isGround = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayerMask);
        if (isGround && velocity.y <= 0)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    /// <summary>
    /// 初始化不同类型怪物的数据
    /// </summary>
    void InitMonsterData()
    {
        patrolMonster = new Paramater();
        patrolMonster.health = 500;
        patrolMonster.attack = 50;
        patrolMonster.restore = 5;
        patrolMonster.attackRange = 10;
        patrolMonster.finalDamage = 1;
        patrolMonster.attackCd = 2;
        patrolMonster.defense = 300;
        patrolMonster.moveSpeed = 4;
        patrolMonster.awardMinExp = 100;
        patrolMonster.awardMaxExp = 200;
        patrolMonster.awardMinMoney = 15;
        patrolMonster.awardMaxMoney = 25;
        patrolMonster.awardEquipProb = 0.2f;

        worldMonster = new Paramater();

        infiniteCore = new Paramater();
    }

    #endregion

    #region Pun Function

    [PunRPC]
    public IEnumerator SpawnAttackCube(Vector3 position, Quaternion rotation, Vector3[] attackCubeData, float[] attackCubeData2, PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

        GameObject attackCube = Instantiate(monsterAttackCubePrefab, position, Quaternion.identity);

        //传入的attackCubeData应该为碰撞体的信息
        attackCube.GetComponent<MonsterAttackCube>().InitAttackCube(rotation * Vector3.forward, Mathf.Abs(lag), attackCubeData, attackCubeData2);

        yield return new WaitForSeconds(0.5f);

    }

    [PunRPC]
    public void MonsterDamaged(int playerActorNumber, float finalAttack, float finalDamage)
    {

        StopCoroutine(ResetLastOneHurtActorNumber());
        paramater.lastOneHurtActorNumber = playerActorNumber;
        float hurt = finalAttack * (1 - paramater.defense * 0.00001f) * finalDamage;

        currentHealth -= hurt;
            
        StartCoroutine(ResetLastOneHurtActorNumber());
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
            stream.SendNext(maxHealth);
            stream.SendNext(m_groupId);
        }
        else
        {
            currentHealth = (float)stream.ReceiveNext();
            maxHealth = (float)stream.ReceiveNext();
            m_groupId = (int)stream.ReceiveNext();
        }
    }

    #endregion

}

#region 偷懒 (State Machine)

public class Paramater
{
    public int lastOneHurtActorNumber;
    public float health;
    public int attack;
    public float restore;
    public float attackCd;
    public float attackRange;
    public float finalDamage;
    public int defense;
    public float moveSpeed;
    public float awardMinExp;
    public float awardMaxExp;
    public float awardMinMoney;
    public float awardMaxMoney;
    public float awardEquipProb;
    public Animator animator;
    public Transform currentTarget;
    public Transform selfTransform;
    public Vector3 initPos;
    public SphereCollider sphereCollider;
    public bool isChaseTarget;
    public bool isIdle;
    public bool isChase;
    public bool isAttack;
    public bool isBack;
}

public interface IState
{
    void OnUpdate();

    void OnExit();

    void OnEnter();
}

public class IdleState : IState
{
    private MonsterController mController;
    private Paramater paramater;

    public IdleState(MonsterController monsterController)
    {
        mController = monsterController;
        paramater = mController.paramater;
    }

    public void OnUpdate()
    {

        if (paramater.isChaseTarget)
        {
            mController.TransitionState(MonsterStateType.Chase);
        }
    }

    public void OnExit()
    {
        paramater.isIdle = false;
    }

    public void OnEnter()
    {
        paramater.isIdle = true;
        paramater.sphereCollider.radius = 5f;
    }
}

public class AttackState :  IState
{
    private MonsterController mController;
    private Paramater paramater;

    private AnimatorStateInfo info;

    public AttackState(MonsterController monsterController)
    {
        this.mController = monsterController;
        this.paramater = monsterController.paramater;
    }

    public void OnUpdate()
    {
        info = paramater.animator.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime >= .95f)
        {
            mController.TransitionState(MonsterStateType.Chase);
        }
    }

    public void OnExit()
    {
        paramater.isAttack = false;
    }

    public void OnEnter()
    {
        paramater.isAttack = true;
        paramater.animator.Play("PatrolMonsterAttack");
    }
}

public class ChaseState : IState
{
    private MonsterController mController;
    private Paramater paramater;

    public ChaseState(MonsterController monsterController)
    {
        this.mController = monsterController;
        this.paramater = monsterController.paramater;
    }

    float chaseTimer;
    float attackCdCountDown;

    public void OnUpdate()
    {
        chaseTimer += Time.deltaTime;

        if (attackCdCountDown >= 0)
        {
            attackCdCountDown -= Time.deltaTime;
        }

        if (chaseTimer >= 4.5f)
        {
            mController.TransitionState(MonsterStateType.Back);
        }

        if (!paramater.isChaseTarget && paramater.currentTarget == null)
        {
            mController.TransitionState(MonsterStateType.Back);
        }

        if (paramater.isChaseTarget && paramater.isChase && attackCdCountDown <= 0 && paramater.currentTarget)
        {
            if (Vector3.Distance(paramater.currentTarget.position, new Vector3(paramater.selfTransform.position.x, paramater.currentTarget.position.y, paramater.selfTransform.position.z)) <= 1.8f)
            {
                mController.TransitionState(MonsterStateType.Attack);
            }
            
        }

        if (Vector3.Distance(paramater.initPos, paramater.selfTransform.position) >= 25f)
        {
            mController.TransitionState(MonsterStateType.Back);
        }

    }

    public void OnExit()
    {
        chaseTimer = 0;
        attackCdCountDown = paramater.attackCd;
        paramater.isChase = false;
    }

    public void OnEnter()
    {
        paramater.isChase = true;
        attackCdCountDown = paramater.attackCd;
        paramater.sphereCollider.radius = 8f;
    }
}

public class BackState : IState
{
    private MonsterController mController;
    private Paramater paramater;

    public BackState(MonsterController monsterController)
    {
        mController = monsterController;
        paramater = monsterController.paramater;
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        paramater.isBack = false;
    }

    public void OnEnter()
    {
        paramater.isBack = true;
        paramater.isChaseTarget = false;
        paramater.currentTarget = null;
        paramater.sphereCollider.radius = 0.01f;
    }
}

#endregion