using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    private CharacterController controller;

    Animator animator;

    GameObject playerModel;

    new PhotonView photonView;

    [SerializeField]
    float m_moveSpeed;

    [SerializeField]
    float gravity;

    public Transform groundCheck;

    public LayerMask layerMask;

    float checkRadius = 0.2f;

    bool isGround;

    Vector3 velocity = Vector3.zero;

    void Start()
    {

        controller = transform.GetComponent<CharacterController>();
        photonView = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        playerModel = CharManager.Instance.FindChildObjWithTag("Player", this.gameObject);
        m_moveSpeed = 8f;
        gravity = -19.8f;

        #region Subscribe Event

        GameEventManager.SubscribeEvent(EventEnum.SendPlayerMoveSpeed, SendPlayerMoveSpeed);

        GameEventManager.SubscribeEvent(EventEnum.AllowPlayerMove, PlayerMove);

        GameEventManager.SubscribeEvent(EventEnum.AllowPlayerAttack, PlayerAttack);

        GameEventManager.SubscribeEvent(EventEnum.AllowPlayerTowardChanged, PlayerTowardChanged);

        #endregion

        #region Register Event

        if (photonView.IsMine)
        {
            GameEventManager.RegisterEvent(EventEnum.OnPlayerDamaged, OnPlayerDamagedCheck);

            
        }

        #endregion
    }

    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        //PlayerMove();
        //PlayerToward();
        PlayerGravity();




    }
    #region Player Control

    void PlayerAnimationControl()
    {

    }

    private void PlayerMove(object[] args)
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            animator.SetBool("isMove", true);
        }
        else
        {
            animator.SetBool("isMove", false);
        }


        Vector3 direction = new Vector3(hor, 0, ver).normalized;

        Vector3 move = direction * m_moveSpeed * Time.deltaTime;
        controller.Move(move);
    }

    private void PlayerTowardChanged(object[] args)
    {

        var playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        var point = Input.mousePosition - playerScreenPoint;
        var angle = Mathf.Atan2(point.x, point.y) * Mathf.Rad2Deg;
        playerModel.transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
    }

    private void PlayerGravity()
    {
        isGround = Physics.CheckSphere(groundCheck.position,checkRadius,layerMask);
        if (isGround && velocity.y<0)
        {
            velocity.y = 0;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    public void PlayerAttack(object[] args)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("isAttack");
        }
        
    }

    public void AnimationEventOnPlayerAttack()
    {
        Debug.LogWarning("Attack Release");
    }

    #endregion

    #region Event Response

    /// <summary>
    /// 接收记录器的移动速度
    /// </summary>
    /// <param name="args"></param>
    public void SendPlayerMoveSpeed(object[] args)
    {
        
        int actorNumber;
        float moveSpeed;
        if (args.Length == 2)
        {
            actorNumber = (int)args[0];
            moveSpeed = (float)args[1];
            m_moveSpeed = moveSpeed;

            GameEventManager.EnableEvent(EventEnum.SendPlayerMoveSpeed, false);
        }
    }

    #endregion

    #region Event Check

    bool OnPlayerDamagedCheck(out object[] args)
    {
        args = null;
        return true;
    }

    #endregion
}
