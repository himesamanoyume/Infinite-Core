using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using ExitGames.Client.Photon;

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
    GameObject loadingMenu;
    [SerializeField]
    GameObject playerNameEntry;
    [SerializeField]
    Transform content;
    
    [SerializeField]
    GameObject startButton;

    private Dictionary<int, GameObject> playerNameTextEntries;

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
        SetActiveMenu(mainMenu.name);
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
            SetActiveMenu(loadingMenu.name);
            
            // #Critical, we must first and foremost connect to Photon Online Server.
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnLeftRoom()
    {
        SetActiveMenu(mainMenu.name);
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
        SetActiveMenu(mainMenu.name);
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
            SetActiveMenu(roomMenu.name);

            if (playerNameTextEntries ==null)
            {
                playerNameTextEntries = new Dictionary<int, GameObject>();
            }

            Player[] players = PhotonNetwork.PlayerList;
            
            foreach(Player p in players)
            {
                GameObject textEntry = Instantiate(playerNameEntry, content);

                PlayerInfo component = textEntry.GetComponent<PlayerInfo>();

                component.InitPlayerTextEntryInfo(p.ActorNumber, p.NickName);

                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("Master");
                    if ((players.Length % 2) == 1)//Red
                    {
                        component.InitPlayerProps(p, TeamEnum.Red);
                        
                    }
                    else//Blue
                    {
                        component.InitPlayerProps(p, TeamEnum.Blue);
                        
                    }
                    
                }
                else
                {
                    Debug.Log("Not Master");
                    object team;
                    if (p.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_TEAM, out team))
                    {//这部分都是之前在房间里的玩家 已有了队伍属性 能读取到team则直接改色
                        Debug.Log("haha");
                        if ((TeamEnum)team==TeamEnum.Red)
                        {
                            component.SetTeam(TeamEnum.Red);
                        }
                        else
                        {
                            component.SetTeam(TeamEnum.Blue);
                        }
                    }
                    else if (p.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                    {//到最后一个玩家也就是本人加入房间时 没有属性 因此初始化

                        if ((players.Length % 2) == 1)//Red
                        {
                            component.InitPlayerProps(p, TeamEnum.Red);
                            //component.SetTeam(TeamEnum.Red);
                        }
                        else//Blue
                        {
                            component.InitPlayerProps(p, TeamEnum.Blue);
                            //component.SetTeam(TeamEnum.Blue);
                        }
                    }
                    
                }

                object isPlayerReady;
                if (p.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_READY, out isPlayerReady))
                {
                    component.SetReadyButton((bool)isPlayerReady);
                }

                playerNameTextEntries.Add(p.ActorNumber, textEntry);
            }

            startButton.SetActive(PhotonNetwork.IsMasterClient);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (playerNameTextEntries == null)
        {
            playerNameTextEntries = new Dictionary<int, GameObject>();
        }

        GameObject entry;
        if (playerNameTextEntries.TryGetValue(targetPlayer.ActorNumber,out entry))
        {
            object isPlayerReady;
            if (changedProps.TryGetValue(InfiniteCoreGame.PLAYER_READY,out isPlayerReady))
            {
                entry.GetComponent<PlayerInfo>().SetReadyImage((bool)isPlayerReady);
            }

            object team;
            if (changedProps.TryGetValue(InfiniteCoreGame.PLAYER_TEAM,out team))
            {
                entry.GetComponent<PlayerInfo>().SetTeam((TeamEnum)team);
            }
        }

        

        //此处检查是否全部准备
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        GameObject textEntry = Instantiate(playerNameEntry, content);
        PlayerInfo component = textEntry.GetComponent<PlayerInfo>();
        component.InitPlayerTextEntryInfo(other.ActorNumber, other.NickName);

        Player[] players = PhotonNetwork.PlayerList;
        if ((players.Length % 2) == 1)//Red
        {
            component.InitPlayerProps(other, TeamEnum.Red);
            //component.SetTeam(TeamEnum.Red);
        }
        else//Blue
        {
            component.InitPlayerProps(other, TeamEnum.Blue);
            //component.SetTeam(TeamEnum.Blue);
        }

        playerNameTextEntries.Add(other.ActorNumber, textEntry);
        

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerNameTextEntries[otherPlayer.ActorNumber].gameObject);

        playerNameTextEntries.Remove(otherPlayer.ActorNumber);

        //此处要检查是否全部准备

    }
    #endregion

    public void LeaveRoom()
    {
        PhotonNetwork.Disconnect();
    }

    /// <summary>
    /// 激活菜单
    /// </summary>
    /// <param name="menuName"></param>
    private void SetActiveMenu(string menuName)
    {
        mainMenu.SetActive(menuName.Equals(mainMenu.name));
        loadingMenu.SetActive(menuName.Equals(loadingMenu.name));
        roomMenu.SetActive(menuName.Equals(roomMenu.name));
    }

    
}
