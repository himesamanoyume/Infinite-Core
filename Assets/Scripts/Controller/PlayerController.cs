using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    private CharacterController controller;
    GameObject playerModel;
    float moveSpeed = 8f;
    float gravity = -19.8f;
    public Transform groundCheck;
    public LayerMask layerMask;
    float checkRadius = 0.2f;
    bool isGround;
    Vector3 velocity = Vector3.zero;
    void Start()
    {

        controller = transform.GetComponent<CharacterController>();

        playerModel = CharManager.Instance.FindChildObjWithTag("Player", this.gameObject);
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

    private void PlayerMove()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(hor, 0, ver).normalized;

        Vector3 move = direction * moveSpeed * Time.deltaTime;
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
        //else
        //{
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        //}

        //if (controller.isGrounded)
        //{
        //    Debug.Log("isGrounded");
        //    velocity = Vector3.zero;
        //}
        //else
        //{
        //    velocity.y += gravity * Time.deltaTime;
        //    controller.Move(velocity * Time.deltaTime);
        //}

        //Vector3 modelButtom = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        // Physics.Raycast(modelButtom, Vector3.down,out RaycastHit hit, 0.1f);

        //try
        //{
        //    if (hit.collider.CompareTag("Ground"))
        //    {
        //        velocity = Vector3.zero;
        //    }
        //    else
        //    {
        //        velocity.y += gravity * Time.deltaTime;
        //        controller.Move(velocity * Time.deltaTime);
        //    }
        //}
        //catch (System.Exception)
        //{
        //    velocity.y += gravity * Time.deltaTime;
        //    controller.Move(velocity * Time.deltaTime);

        //}

    }
}
