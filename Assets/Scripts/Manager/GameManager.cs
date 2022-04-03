using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;


public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    #region Public Methods


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    //public void LoadArena()
    //{
    //    if (!PhotonNetwork.IsMasterClient)
    //    {
    //        Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
    //    }
    //    else
    //    {
    //        PhotonNetwork.LoadLevel("Test");
    //        Debug.LogFormat("PhotonNetwork : Loading Level : Test");
    //    }
    //}

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Game");
        }
        else
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
    }
    #endregion

    #region Private Methods

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    #region Photon Callbacks

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    #endregion

    public void ExitGame()
    {
        Application.Quit();
    }


}
