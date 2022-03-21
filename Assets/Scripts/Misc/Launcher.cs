using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.UI;


public class Launcher : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields
    
    /// <summary>
    /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
    /// </summary>
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 10;
    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject controlPanel;
    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabel;

    bool isConnecting;

    [SerializeField]
    GameObject mainMenu;
    [SerializeField]
    GameObject roomMenu;
    [SerializeField]
    Text playerNameText;
    [SerializeField]
    Transform content;

    public static Launcher instance;

    #endregion


    #region Private Fields


    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// </summary>
    string gameVersion = "1.0.0";


    #endregion


    #region MonoBehaviour CallBacks

    private void Start()
    {
        progressLabel.SetActive(false);
        mainMenu.SetActive(true);
        roomMenu.SetActive(false);
    }
    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
        instance = this;
    }


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
 

    #endregion


    #region Public Methods


    /// <summary>
    /// Start the connection process.
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            
           
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRoom("test");
        }
        else
        {
            progressLabel.SetActive(true);
            
            // #Critical, we must first and foremost connect to Photon Online Server.
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnLeftRoom()
    {
        roomMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
            
        if (isConnecting)
        {
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinOrCreateRoom("test", new RoomOptions { MaxPlayers = maxPlayersPerRoom },default);
            isConnecting = false;
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        }
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    //public override void OnJoinRandomFailed(short returnCode, string message)
    //{
    //    Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

    //    // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
    //    PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    //}

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 1)
        {
            // #Critical
            // Load the Room Level.

            //PhotonNetwork.LoadLevel("Room");
            progressLabel.SetActive(false);
            roomMenu.SetActive(true);
            mainMenu.SetActive(false);

            Player[] players = PhotonNetwork.PlayerList;
            for (int i = 0; i < players.Length; i++)
            {
                Text text = Instantiate(playerNameText, content);
                text.text = players[i].NickName;
                
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Text text = Instantiate(playerNameText, content);
        text.text = other.NickName;
        
        //Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        //}
        //Debug.LogWarning(PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        for (int i = 0; i < content.childCount; i++)
        {
            if (content.GetChild(i).GetComponent<Text>().text == otherPlayer.NickName)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }

    }
    #endregion

    public void LeaveRoom()
    {
        PhotonNetwork.Disconnect();
    }

}
