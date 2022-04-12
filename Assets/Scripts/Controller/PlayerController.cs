using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
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

    bool isAttack;

    Vector3 velocity = Vector3.zero;

    void Start()
    {

        controller = this.transform.GetComponent<CharacterController>();
        photonView = this.gameObject.GetComponent<PhotonView>();
        animator = this.gameObject.GetComponent<Animator>();
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
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) return;


        //PlayerMove();
        //PlayerTowardChanged(;
        PlayerGravity();




    }
    #region Player Control

    void PlayerAnimationControl()
    {

    }

    private void PlayerMove(object[] args)
    {
        if (photonView.IsMine)
        {
            var hor = Input.GetAxis("Horizontal");
            var ver = Input.GetAxis("Vertical");


            Vector3 direction = new Vector3(hor, 0, ver).normalized;

            Vector3 move = direction * m_moveSpeed * Time.deltaTime;
            controller.Move(move);
        }
    }

    private void PlayerTowardChanged(object[] args)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) return;

        var playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        var point = Input.mousePosition - playerScreenPoint;
        var angle = Mathf.Atan2(point.x, point.y) * Mathf.Rad2Deg;
        playerModel.transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
        
        
    }

    private void PlayerGravity()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) return;

        isGround = Physics.CheckSphere(groundCheck.position, checkRadius, layerMask);
        if (isGround && velocity.y < 0)
        {
            velocity.y = 0;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        

    }

    public void PlayerAttack(object[] args)
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) return;

        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                animator.SetTrigger("isAttack");
                //animator.SetBool("isAttack", true);
                //isAttack = true;
            }
        }
    }

    public void AnimationEventOnPlayerAttackEnd()
    {
        if (photonView.IsMine)
        {
            //animator.SetBool("isAttack", false);
        }
    }

    public void AnimationEventOnPlayerAttack()
    {
        if (photonView.IsMine)
        {
            Debug.LogWarning("Attack Release");
        }
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (stream.IsWriting)
        //{
        //    stream.SendNext(isAttack);
        //}
        //else
        //{
        //    this.isAttack = (bool)stream.ReceiveNext();
        //}
    }
}
