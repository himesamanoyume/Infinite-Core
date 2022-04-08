using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

/// <summary>
/// 附在玩家物体上
/// </summary>
public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{


    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

        }
        else
        {

        }
    }

    void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }
    }

    void Start()
    {
        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();
        

        if (_cameraWork != null)
        {
            if (photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
