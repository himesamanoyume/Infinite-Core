using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class PlayerInfo : MonoBehaviourPunCallbacks
{

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
