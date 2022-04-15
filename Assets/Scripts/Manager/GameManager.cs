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
        GameEventManager.EnableEvent(EventEnum.CharMgrGroup, false);
        GameEventManager.EnableEvent(EventEnum.PlayerGroup, false);
        PhotonNetwork.LeaveRoom();
    }

    public void StartGame()
    {
        GameEventManager.EnableEvent(EventEnum.CharMgrGroup, true);
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

    private void Update()
    {
        GameEventManager.Update();
    }

    private void Start()
    {
        #region Register Event
        //if (photonView.IsMine)
        //{
        GameEventManager.RegisterEvent(EventEnum.AllowGetPlayerModelList, GetPlayerModelListCheck);
        GameEventManager.RegisterEvent(EventEnum.AllowGetRecorderList, GetRecorderListCheck);

        //}
        #endregion
    }

    #endregion

    #region Photon Callbacks

    public override void OnLeftRoom()
    {
        
        PhotonNetwork.LocalPlayer.CustomProperties.Clear();
        SceneManager.LoadScene(0);
    }



    #endregion

    public void ExitGame()
    {
        Application.Quit();
    }

    #region Event Check

    bool GetPlayerModelListCheck(out object[] args)
    {
        args = null;
        return true;
    }

    bool GetRecorderListCheck(out object[] args)
    {
        args = null;
        return true;
    }

    #endregion

    

}
