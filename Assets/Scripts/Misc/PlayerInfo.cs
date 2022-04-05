using ExitGames.Client.Photon;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class PlayerInfo : MonoBehaviourPunCallbacks
{
    [SerializeField]
    int ownerId;
    [SerializeField]
    bool isPlayerReady;
    [SerializeField]
    private Text playerNameText;
    [SerializeField]
    GameObject readyButton;
    [SerializeField]
    Image readyImage;
    [SerializeField]
    TeamEnum currentTeam;

    Color redTeamColor = new Color(1, 0, 0, 0.3f);
    Color blueTeamColor = new Color(0, 0, 1, 0.3f);
    Color nullTeamColor = new Color(1, 1, 1, 0.3f);

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        //if (targetPlayer.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber) return;

        //object ready;
        //if (changedProps.TryGetValue(InfiniteCoreGame.PLAYER_READY,out ready))
        //{
        //    isPlayerReady = (bool)ready;
            
        //}
    }

    public void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
        {
            readyButton.gameObject.SetActive(false);
        }
        else
        {
            InitPlayerProps(PhotonNetwork.LocalPlayer);
            SetReadyImage(false);
            readyButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                isPlayerReady = !isPlayerReady;
                SetReadyButton(isPlayerReady);
            });
        }

    }

    public void InitPlayerTextEntryInfo(int playerId, string playerName)
    {
        ownerId = playerId;
        playerNameText.text = playerName;
    }

    public void SetReadyButton(bool playerReady)
    {
        readyButton.GetComponentInChildren<Text>().text = playerReady ? "Ready!" : "Ready?";
        readyImage.enabled = playerReady;
        Hashtable props = new Hashtable{
            { InfiniteCoreGame.PLAYER_READY,playerReady}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public void SetReadyImage(bool playerReady)
    {
        readyImage.enabled = playerReady;
        isPlayerReady = playerReady;
    }

    public void OnTeamChanged(TeamEnum team)
    {
        Hashtable props = new Hashtable
        {
            {InfiniteCoreGame.PLAYER_TEAM, team}
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        SetTeam(team);


    }

    public void SetTeam(TeamEnum team)
    {
        currentTeam = team;
        //Debug.Log(ownerId + " SetTeam");
        if (team == TeamEnum.Red)
        {
            gameObject.GetComponent<Image>().color = redTeamColor;
        }
        else if(team == TeamEnum.Blue)
        {
            gameObject.GetComponent<Image>().color = blueTeamColor;
        }
        else
        {
            gameObject.GetComponent<Image>().color = nullTeamColor;
        }
    }

    /// <summary>
    /// 在玩家进入房间后初始化自定义属性
    /// </summary>
    /// <param name="p"></param>
    /// <param name="team"></param>
    public void InitPlayerProps(Player p)
    {
        Debug.LogWarning(p.NickName + " Init");
        CharBase c = new CharBase();

        Hashtable initProps = new Hashtable()
        {
            {InfiniteCoreGame.PLAYER_ACTOR_NUMBER, p.ActorNumber},
            {InfiniteCoreGame.PLAYER_READY,  isPlayerReady},
            {InfiniteCoreGame.PLAYER_NAME, p.NickName },
            {InfiniteCoreGame.PLAYER_TEAM, c.PlayerTeam },
            //{InfiniteCoreGame.PLAYER_KILL, c.Kill },
            //{InfiniteCoreGame.PLAYER_DEATH, c.Death },
            //{InfiniteCoreGame.PLAYER_MONEY, c.Money },
            //{InfiniteCoreGame.PLAYER_CURRENT_EXP, c.CurrentExp },
            //{InfiniteCoreGame.PLAYER_MAX_EXP, c.MaxExp },
            {InfiniteCoreGame.PLAYER_STATE, c.State },
            //{InfiniteCoreGame.PLAYER_BUFF, new BuffEnum[20] },
            {InfiniteCoreGame.PLAYER_PRO, ProEnum.Soilder},
            //{InfiniteCoreGame.PLAYER_LEVEL, c.Level},
            {InfiniteCoreGame.PLAYER_ATTACK, c.Attack},
            {InfiniteCoreGame.PLAYER_SHIELD, c.Shield},
            {InfiniteCoreGame.PLAYER_MAX_HEALTH, c.MaxHealth },
            {InfiniteCoreGame.PLAYER_CURRENT_HEALTH, c.CurrentHealth},
            {InfiniteCoreGame.PLAYER_CRITICALHIT_HIT, c.CriticalHit},
            {InfiniteCoreGame.PLAYER_CRITICAL_HIT_RATE, c.CriticalHitRate },
            {InfiniteCoreGame.PLAYER_DEFENCE, c.Defence},
            {InfiniteCoreGame.PLAYER_ATTACK_SPEED, c.AttackSpeed},
            {InfiniteCoreGame.PLAYER_RESTORE, c.Restore},
            {InfiniteCoreGame.PLAYER_SKILL_Q, c.SkillQ},
            {InfiniteCoreGame.PLAYER_SKILL_E, c.SkillE},
            {InfiniteCoreGame.PLAYER_SKILL_R, c.SkillR},
            {InfiniteCoreGame.PLAYER_SKILL_BURST, c.SkillBurst },
            //{InfiniteCoreGame.PLAYER_HEAD_ID, c.HeadID},
            //{InfiniteCoreGame.PLAYER_ARMOR_ID, c.ArmorID},
            //{InfiniteCoreGame.PLAYER_HAND_ID, c.HandID},
            //{InfiniteCoreGame.PLAYER_KNEE_ID, c.KneeID},
            //{InfiniteCoreGame.PLAYER_TROUSERS_ID, c.TrousersID},
            //{InfiniteCoreGame.PLAYER_BOOTS, c.BootsID},
            {InfiniteCoreGame.PLAYER_MOVE_SPEED, c.MoveSpeed},
            {InfiniteCoreGame.PLAYER_ATTACK_RANGE, c.AttackRange},
            //{InfiniteCoreGame.PLAYER_RESPAWN_TIME, c.RespawnTime},
            //{InfiniteCoreGame.PLAYER_RESPAWN_COUNTDOWN, c.RespawnCountDown}

        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(initProps);

    }
}