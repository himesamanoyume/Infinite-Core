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

public class MonsterController : MonoBehaviour
{
    

    private IState currentState;

    private Dictionary<MonsterStateType, IState> states = new Dictionary<MonsterStateType, IState>();

    public Paramater paramater;

    public MonsterType monsterType;
    public GameObject monsterInfoBarPrefab;

    

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

    // Start is called before the first frame update
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

        paramater.sphereCollider = GetComponent<SphereCollider>();

        myMonsterInfoBar = Instantiate(monsterInfoBarPrefab);
        myMonsterInfoBar.transform.SetParent(targetCanvas);
        myMonsterInfoBar.GetComponent<MonsterInfoBar>().InitMonsterInfoBar(this.transform, this.paramater);

        TransitionState(MonsterStateType.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            MonsterGravity();

            if (paramater.health <= 0)
            {
                Destroy(myMonsterInfoBar);
                Destroy(gameObject);
            }

            currentState.OnUpdate();

            if (Vector3.Distance(transform.position, new Vector3(paramater.initPos.x, transform.position.y, paramater.initPos.z)) <= 0.1f && !paramater.isIdle && paramater.isBack)
            {
                TransitionState(MonsterStateType.Idle);
            }

            //if (paramater.target && paramater.isTarget)
            if (paramater.isChase)
            {
                //×·»÷×´Ì¬
                controller.Move((paramater.target.transform.position - transform.position).normalized * Time.deltaTime * paramater.moveSpeed);

                MonsterTowardChanged(paramater.target.transform.position);
            }
            //else if (!paramater.isTarget && !paramater.target && !paramater.isIdle)
            else if (paramater.isBack)
            {
                //·µ»Ø×´Ì¬
                controller.Move((new Vector3(paramater.initPos.x, transform.position.y, paramater.initPos.z) - transform.position).normalized * Time.deltaTime * paramater.moveSpeed);

                MonsterTowardChanged(paramater.initPos);
            }
            //else if (!paramater.isIdle)
            else if (paramater.isIdle)
            {

            }
        }
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

    Paramater patrolMonster;
    Paramater worldMonster;
    Paramater infiniteCore;

    void InitMonsterData()
    {
        patrolMonster = new Paramater();
        patrolMonster.health = 500f;
        patrolMonster.attack = 50;
        patrolMonster.attackCd = 2f;
        patrolMonster.defense = 300;
        patrolMonster.moveSpeed = 4f;

        worldMonster = new Paramater();

        infiniteCore = new Paramater();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("PlayerModel"))
        {
            //Debug.LogWarning("Enter");
            paramater.target = other.gameObject.transform;
            paramater.isTarget = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerModel"))
        {
            paramater.isTarget = false;

            paramater.target = null;

            //paramater.target.position = paramater.initPos;
        }
    }
}

[SerializeField]
public class Paramater
{
    public int id;
    public float health;
    public int attack;
    public float attackCd;
    public int defense;
    public float moveSpeed;
    public float awardExp;
    public float awardMoney;
    public Animator animator;
    public Transform target;
    public Transform selfTransform;
    public Vector3 initPos;
    public SphereCollider sphereCollider;
    public bool isTarget;
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

        if (paramater.isTarget)
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
        Debug.LogWarning("Idle Enter");
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
        Debug.LogWarning("Attack Enter");
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

        if (!paramater.isTarget)
        {
            mController.TransitionState(MonsterStateType.Back);
        }

        if (Vector3.Distance(paramater.target.position, new Vector3(paramater.selfTransform.position.x, paramater.target.position.y, paramater.selfTransform.position.z)) <= 1.8f && paramater.isChase && attackCdCountDown <= 0)
        {
            mController.TransitionState(MonsterStateType.Attack);
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
        Debug.LogWarning("Chase Enter");
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
        paramater.sphereCollider.radius = 0.01f;
        Debug.LogWarning("Back Enter");
    }
}