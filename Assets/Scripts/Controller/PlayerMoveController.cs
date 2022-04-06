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

        Vector3 direction = new Vector3(hor, 0, ver).normalized;

        Vector3 move = direction * Speed * Time.deltaTime;
        controller.Move(move);
    }
}
