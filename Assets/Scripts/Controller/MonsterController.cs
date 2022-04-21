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

    /// <summary>
    /// 初始位置 Back状态时会返回此处
    /// </summary>
    private Vector3 initPos;

    private GameObject myMonsterInfoBar;

    private PhotonView photonView;

    bool isGround;
    public Transform groundCheck;
    public LayerMask layerMask;
    float checkRadius = 0.2f;
    Vector3 velocity = Vector3.zero;
    public float gravity;
    CharacterController controller;
    Transform targetCanvas;

    // Start is called before the first frame update
    void Start()
    {
        InitMonsterData();

        initPos = transform.position;

        photonView = GetComponent<PhotonView>();

        controller = GetComponent<CharacterController>();

        targetCanvas = GameObject.Find("PlayerInfoCanvas").transform;

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

        states.Add(MonsterStateType.Idle, new IdleState(this));
        states.Add(MonsterStateType.Attacking, new AttackingState(this));
        states.Add(MonsterStateType.Back, new BackState(this));

        TransitionState(MonsterStateType.Idle);

        paramater.animator = GetComponent<Animator>();

        myMonsterInfoBar = Instantiate(monsterInfoBarPrefab);
        myMonsterInfoBar.transform.SetParent(targetCanvas);
        myMonsterInfoBar.GetComponent<MonsterInfoBar>().InitMonsterInfoBar(this.transform, this.paramater);
    }

    // Update is called once per frame
    void Update()
    {

        PlayerGravity();

        if (paramater.health <= 0)
        {
            Destroy(myMonsterInfoBar);
            Destroy(gameObject);
        }
    }

    void PlayerGravity()
    {
        if (photonView.IsMine)
        {

            isGround = Physics.CheckSphere(groundCheck.position, checkRadius, layerMask);
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
        currentState.OnExit();
    }

    public void MonsterTowardChanged(Transform target)
    {
        var angle = Mathf.Atan2(target.position.x, target.position.y) * Mathf.Rad2Deg;
        gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
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
        patrolMonster.moveSpeed = 10f;

        worldMonster = new Paramater();

        infiniteCore = new Paramater();
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
    private Paramater paramater;

    public IdleState(MonsterController monsterController)
    {
        this.monsterController = monsterController;
        this.paramater = monsterController.paramater;

    }

    public void OnUpdate()
    {
        //球形检测到有tag为PlayerModel时设为target 进入Attacking
    }

    public void OnExit()
    {

    }

    public void OnEnter()
    {

    }
}

public class AttackingState : IState
{
    private MonsterController monsterController;
    private Paramater paramater;

    public AttackingState(MonsterController monsterController)
    {
        this.monsterController = monsterController;
        this.paramater = monsterController.paramater;
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }

    public void OnEnter()
    {

    }
}

public class BackState : IState
{
    private MonsterController monsterController;
    private Paramater paramater;

    public BackState(MonsterController monsterController)
    {
        this.monsterController = monsterController;
        this.paramater = monsterController.paramater;
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }

    public void OnEnter()
    {

    }
}