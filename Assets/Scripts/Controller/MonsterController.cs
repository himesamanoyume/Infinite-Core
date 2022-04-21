using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;



public class MonsterController : MonoBehaviour
{
    public enum MonsterStateType
    {
        Idle, Attacking, Back
    }

    public enum MonsterType
    {
        PatrolMonster, WorldMonster, InfiniteCore
    }

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

        photonView = GetComponent<PhotonView>();

        controller = GetComponent<CharacterController>();

        targetCanvas = GameObject.Find("PlayerInfoCanvas").transform;

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

        paramater.initPos = transform.position;

        states.Add(MonsterStateType.Idle, new IdleState(this));
        states.Add(MonsterStateType.Attacking, new AttackingState(this));
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

            if (Vector3.Distance(transform.parent.transform.position, paramater.initPos) <= 0.1f && !paramater.isTarget)
            {
                TransitionState(MonsterStateType.Idle);
            }

            if (paramater.target)
            {
                controller.Move((paramater.target.transform.position - transform.position).normalized * Time.deltaTime * paramater.moveSpeed);

                MonsterTowardChanged(paramater.target.transform);
            }
        }
    }


    void MonsterGravity()
    {
        if (photonView.IsMine)
        {

            isGround = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayerMask);
            if (isGround && velocity.y < 0)
            {
                velocity.y = 0;
            }

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

    public void MonsterTowardChanged(Transform target)
    {
        var angle = Mathf.Atan2(target.position.x, target.position.y) * Mathf.Rad2Deg;
        gameObject.transform.localEulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
    }

    public void GetMonsterData(Paramater presetParamater)
    {
        paramater = presetParamater;
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
            Debug.LogWarning("Enter");
            paramater.target = other.gameObject.transform;
            paramater.isTarget = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerModel"))
        {
            paramater.isTarget = false;
            //paramater.target = null;

            paramater.target.transform.position = paramater.initPos;
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
    public Vector3 initPos;
    public SphereCollider sphereCollider;
    public bool isTarget;
}

public interface IState
{
    void OnUpdate();

    void OnExit();

    void OnEnter();
}

public class IdleState : IState
{
    private MonsterController monsterController;

    public IdleState(MonsterController monsterController)
    {
        this.monsterController = monsterController;
    }

    public void OnUpdate()
    {
        //球形检测到有tag为PlayerModel时设为target 进入Attacking

        if (monsterController.paramater.isTarget)
        {
            monsterController.TransitionState(MonsterController.MonsterStateType.Attacking);
        }
    }

    public void OnExit()
    {

    }

    public void OnEnter()
    {
        Debug.LogWarning("Idle");
        monsterController.paramater.sphereCollider.radius = 10f;
    }
}

public class AttackingState :  IState
{
    private MonsterController monsterController;

    public AttackingState(MonsterController monsterController)
    {
        this.monsterController = monsterController;
    }

    public void OnUpdate()
    {
        if (!monsterController.paramater.isTarget)
        {
            monsterController.TransitionState(MonsterController.MonsterStateType.Back);
        }
    }

    public void OnExit()
    {

    }

    public void OnEnter()
    {
        monsterController.paramater.sphereCollider.radius = 15f;
        Debug.LogWarning("Attack Enter");
    }

}

public class BackState : IState
{
    private MonsterController monsterController;

    public BackState(MonsterController monsterController)
    {
        this.monsterController = monsterController;
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }

    public void OnEnter()
    {
        

        monsterController.paramater.sphereCollider.radius = 0.01f;
        Debug.LogWarning("Back Enter");
    }
}