using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    private CharacterController controller;
    GameObject playerModel;
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
        playerModel = CharManager.Instance.FindChildObjWithTag("Player", this.gameObject);
        m_moveSpeed = 8f;
        gravity = -19.8f;
        GameEventManager.SubscribeEvent(EventEnum.SendPlayerMoveSpeed, SendPlayerMoveSpeed);
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
        PlayerMove();
        PlayerToward();
        PlayerGravity();



    }
    #region Player Control

    private void PlayerMove()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(hor, 0, ver).normalized;

        Vector3 move = direction * m_moveSpeed * Time.deltaTime;
        controller.Move(move);
    }

    private void PlayerToward()
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
}
