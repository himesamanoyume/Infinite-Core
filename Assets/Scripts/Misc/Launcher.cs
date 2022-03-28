using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using ExitGames.Client.Photon;
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
    GameObject loadingMenu;
    [SerializeField]
    InputField TextInputField;
    [SerializeField]
    GameObject playerNameEntry;
    [SerializeField]
    Transform content;
    [SerializeField]
    GameObject redButton;
    [SerializeField]
    GameObject blueButton;
    [SerializeField]
    int redCount = 0;
    [SerializeField]
    int blueCount = 0;
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
        TextInputField.text = "Player " + Random.Range(1000, 10000);
    }


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>


    #endregion


    #region Public Methods


    public int RedCount {
        get => redCount;
        set
        {
            if (value<=0)
            {
                redCount = 0;
            }
            else
            {
                redCount = value;
            }
        }
    }

    public int BlueCount
    {
        get => blueCount;
        set
        {
            if (value <= 0)
            {
                blueCount = 0;
            }
            else
            {
                blueCount = value;
            }
        }
    }

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
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public void OnRedButtonClicked()
    {
        Hashtable props = new Hashtable
        {
            {InfiniteCoreGame.PLAYER_TEAM,TeamEnum.Red }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        redButton.SetActive(false);
        blueButton.SetActive(true);
    }

    public void OnBlueButtonClicked()
    {
        Hashtable props = new Hashtable
        {
            {InfiniteCoreGame.PLAYER_TEAM,TeamEnum.Blue }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        blueButton.SetActive(false);
        redButton.SetActive(true);
    }

    public void OnTeamChanged()
    {
        RedCount = 0;
        BlueCount = 0;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object playerTeam;
            if (p.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_TEAM,out playerTeam))
            {
                if ((TeamEnum)playerTeam == TeamEnum.Red)
                {
                    RedCount++;
                }
                else if ((TeamEnum)playerTeam == TeamEnum.Blue)
                {
                    BlueCount++;
                }
                else
                {
                   
                }
            }

        }
    }
    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnLeftRoom()
    {
        SetActiveMenu(mainMenu.name);
        playerNameTextEntries.Clear();
    }

    public override void OnConnectedToMaster()
    {

        if (isConnecting)
        {
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinOrCreateRoom("test", new RoomOptions { MaxPlayers = maxPlayersPerRoom }, default);
            isConnecting = false;
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SetActiveMenu(mainMenu.name);
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 1)
        {

            SetActiveMenu(roomMenu.name);

            if (playerNameTextEntries == null)
            {
                playerNameTextEntries = new Dictionary<int, GameObject>();
            }

            Player[] players = PhotonNetwork.PlayerList;

            foreach (Player p in players)
            {
                GameObject textEntry = Instantiate(playerNameEntry, content);

                PlayerInfo component = textEntry.GetComponent<PlayerInfo>();

                component.InitPlayerTextEntryInfo(p.ActorNumber, p.NickName);

                
                if (p.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_NAME,out object name))
                {
                    
                }
                else
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        Debug.Log("OnJoinedRoom " + p.NickName + " Init");
                        component.InitPlayerProps(p);
                    }
                    
                }
                
                object isPlayerReady;
                if (p.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_READY, out isPlayerReady))
                {
                    Debug.LogWarning(p.NickName+ " gerReady "+(bool)isPlayerReady);
                    component.SetReadyImage((bool)isPlayerReady);
                }

                object playerTeam;
                if (p.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_TEAM,out playerTeam))
                {
                    Debug.LogWarning(p.NickName+" gerTeam "+(TeamEnum)playerTeam);
                    component.GetComponent<PlayerInfo>().SetTeam((TeamEnum)playerTeam);

                    if ((TeamEnum)playerTeam == TeamEnum.Red)
                    {
                        RedCount++;
                    }
                    else if ((TeamEnum)playerTeam == TeamEnum.Blue)
                    {
                        BlueCount++;
                    }
                    else
                    {
                        
                    }
                }
               
                playerNameTextEntries.Add(p.ActorNumber, textEntry);
            }

            startButton.SetActive(PhotonNetwork.IsMasterClient);
        }
    }

    /// <summary>
    /// 当玩家属性更新时
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="changedProps"></param>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        Debug.Log(targetPlayer.ActorNumber + " Update");
        if (playerNameTextEntries == null)
        {
            playerNameTextEntries = new Dictionary<int, GameObject>();
        }

        GameObject entry;
        if (playerNameTextEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
        {
            
            object isPlayerReady;
            if (changedProps.TryGetValue(InfiniteCoreGame.PLAYER_READY, out isPlayerReady))
            {
                Debug.Log(targetPlayer.ActorNumber + " SetReadyImage "+ (bool)isPlayerReady);
                entry.GetComponent<PlayerInfo>().SetReadyImage((bool)isPlayerReady);
                 
            }

            object playerTeam;
            if (changedProps.TryGetValue(InfiniteCoreGame.PLAYER_TEAM, out playerTeam))
            {
                Debug.Log(targetPlayer.ActorNumber + " SetTeam "+ (TeamEnum)playerTeam);
                entry.GetComponent<PlayerInfo>().SetTeam((TeamEnum)playerTeam);

                OnTeamChanged();

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

        playerNameTextEntries.Add(other.ActorNumber, textEntry);

        //---
        if (other.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_ACTOR_NUMBER, out object actorNumber))
        {

        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //Debug.LogError("Are you Master?");
                Debug.Log("OnPlayerEnteredRoom " + other.NickName + " Init");
                component.InitPlayerProps(other);
            }

        }
        //---
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerNameTextEntries[otherPlayer.ActorNumber].gameObject);

        playerNameTextEntries.Remove(otherPlayer.ActorNumber);

        OnTeamChanged();

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