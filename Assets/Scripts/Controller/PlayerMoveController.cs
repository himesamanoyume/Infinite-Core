using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMoveController : MonoBehaviourPunCallbacks
{
    private CharacterController controller;
    public float Speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        controller = transform.GetComponent<CharacterController>();

    }

    // Update is called once per frame
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

        
    }

    private void PlayerMove()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");

        //Vector3 wDirection = new Vector3(1, 0, 0);
        //Vector3 sDirection = new Vector3(-1, 0, 0);
        //Vector3 aDirection = new Vector3(0, 0, 1);
        //Vector3 dDirection = new Vector3(0, 0, -1);

        Vector3 direction = new Vector3(hor, 0, ver).normalized;

        //if (Input.GetKey(KeyCode.W))
        //{
        //    Vector3 move = wDirection * Speed * Time.deltaTime;
        //    controller.Move(move);
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    Vector3 move = sDirection * Speed * Time.deltaTime;
        //    controller.Move(move);
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    Vector3 move = aDirection * Speed * Time.deltaTime;
        //    controller.Move(move);
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    Vector3 move = dDirection * Speed * Time.deltaTime;
        //    controller.Move(move);
        //}

        Vector3 move = direction * Speed * Time.deltaTime;
        controller.Move(move);
    }
}
