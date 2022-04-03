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
    GameObject connectMenu;
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
        SetActiveMenu(connectMenu.name);
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
            SetActiveMenu(mainMenu.name);
            //PhotonNetwork.JoinRoom("test");
        }
        else
        {
            SetActiveMenu(loadingMenu.name);
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public void LoadRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinOrCreateRoom("test", new RoomOptions { MaxPlayers = maxPlayersPerRoom }, default);
            SetActiveMenu(loadingMenu.name);
        }
    }

    public void OnRedButtonClicked()
    {
        Hashtable props = new Hashtable
        {
            {InfiniteCoreGame.PLAYER_TEAM,TeamEnum.Red }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        //redButton.GetComponent<Button>().interactable = false;
        //blueButton.GetComponent<Button>().interactable = true;
        //redButton.SetActive(false);
        //blueButton.SetActive(true);
    }

    public void OnBlueButtonClicked()
    {
        Hashtable props = new Hashtable
        {
            {InfiniteCoreGame.PLAYER_TEAM,TeamEnum.Blue }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        //blueButton.SetActive(false);
        //redButton.SetActive(true);
    }

    /// <summary>
    /// 如果本地玩家没有选择阵容时 红蓝都可按 但某阵营满员时不能按，已选择阵营时 己方阵营不能按 对方阵营可按  但如果对方阵营满员 也不能按
    /// </summary>
    public void CheckTeamCount()
    {
        object myTeam;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_TEAM,out myTeam))
        {

            //Debug.Log("自身为Null阵营");

            if ((TeamEnum)myTeam==TeamEnum.Red)
            {
                redButton.GetComponent<Button>().interactable = false ;
                blueButton.GetComponent<Button>().interactable = (BlueCount<5) ? true : false;
            }
            else if ((TeamEnum)myTeam == TeamEnum.Blue)
            {
                blueButton.GetComponent<Button>().interactable = false;
                redButton.GetComponent<Button>().interactable = (RedCount < 5) ? true : false;
            }
            Debug.Log("CheckTeamCount");
        }
        else
        {
            Debug.Log("CheckTeamCount else");
            redButton.GetComponent<Button>().interactable = (RedCount == 5) ? false : true;
            blueButton.GetComponent<Button>().interactable = (BlueCount == 5) ? false : true;
        }
        
    }

    /// <summary>
    ///  检查玩家是否全部准备
    /// </summary>
    /// <returns></returns>
    public void CheckPlayerReady()
    {
        object isReady, team;
        Debug.Log("CheckPlayerReady");
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_READY, out isReady) && p.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_TEAM,out team))
            {
                if ((bool)isReady &&((TeamEnum)team == TeamEnum.Blue || (TeamEnum)team == TeamEnum.Red))
                {
                    continue;
                }
                Debug.LogFormat("{0} 玩家未准备好", p.NickName);
                startButton.GetComponent<Button>().interactable = false;
                return;
            }

            Debug.LogFormat("{0} 玩家未准备好", p.NickName);
            startButton.GetComponent<Button>().interactable = false;
            return;
        }
        startButton.GetComponent<Button>().interactable = true;
    }

    /// <summary>
    /// 当阵营变动时
    /// </summary>
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
            //CheckTeamCount();
            Debug.LogFormat("RedCount: {0}, BlueCount: {1}" ,RedCount, BlueCount);
            
        }
        
        CheckTeamCount();
        CheckPlayerReady();
    }
    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnLeftRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            SetActiveMenu(mainMenu.name);
            PhotonNetwork.LocalPlayer.CustomProperties.Clear();
            playerNameTextEntries.Clear();
            Debug.Log("OnLeftRoom");
        }
        
    }

    public override void OnConnectedToMaster()
    {

        if (isConnecting)
        {
            SetActiveMenu(mainMenu.name);
            TextInputField.text = "Player " + Random.Range(1000, 10000);
            isConnecting = false;
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SetActiveMenu(connectMenu.name);
        TextInputField.text = "Player " + Random.Range(1000, 10000);
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinedRoom()
    {
       
        SetActiveMenu(roomMenu.name);

        if (playerNameTextEntries == null)
        {
            playerNameTextEntries = new Dictionary<int, GameObject>();
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject textEntry = Instantiate(playerNameEntry, content);

            PlayerInfo component = textEntry.GetComponent<PlayerInfo>();

            component.InitPlayerTextEntryInfo(p.ActorNumber, p.NickName);

            object isPlayerReady;
            if (p.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_READY, out isPlayerReady))
            {
                Debug.LogWarning(p.NickName+ " getReady "+(bool)isPlayerReady);
                component.SetReadyImage((bool)isPlayerReady);
            }

            object playerTeam;
            if (p.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_TEAM,out playerTeam))
            {
                Debug.LogWarning(p.NickName+" getTeam "+(TeamEnum)playerTeam);
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
        CheckTeamCount();
        startButton.SetActive(PhotonNetwork.IsMasterClient);
        CheckPlayerReady();
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

                if (PhotonNetwork.IsMasterClient)
                {
                    CheckPlayerReady();
                }

            }

            object playerTeam;
            if (changedProps.TryGetValue(InfiniteCoreGame.PLAYER_TEAM, out playerTeam))
            {
                Debug.Log(targetPlayer.ActorNumber + " SetTeam "+ (TeamEnum)playerTeam);
                entry.GetComponent<PlayerInfo>().SetTeam((TeamEnum)playerTeam);

                OnTeamChanged();

            }
        }

        //CheckPlayerReady();
        
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

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerNameTextEntries[otherPlayer.ActorNumber].gameObject);

        playerNameTextEntries.Remove(otherPlayer.ActorNumber);

        OnTeamChanged();

        if (PhotonNetwork.IsMasterClient)
        {
            CheckPlayerReady();
        }
    }
    #endregion

    /// <summary>
    /// 离开房间
    /// </summary>
    public void LeaveRoom()
    {
        redButton.GetComponent<Button>().interactable=true;
        blueButton.GetComponent<Button>().interactable=true;
        PhotonNetwork.LeaveRoom();
        SetActiveMenu(mainMenu.name);
    }

    /// <summary>
    /// 激活菜单
    /// </summary>
    /// <param name="menuName"></param>
    private void SetActiveMenu(string menuName)
    {
        connectMenu.SetActive(menuName.Equals(connectMenu.name));
        mainMenu.SetActive(menuName.Equals(mainMenu.name));
        loadingMenu.SetActive(menuName.Equals(loadingMenu.name));
        roomMenu.SetActive(menuName.Equals(roomMenu.name));
    }


}