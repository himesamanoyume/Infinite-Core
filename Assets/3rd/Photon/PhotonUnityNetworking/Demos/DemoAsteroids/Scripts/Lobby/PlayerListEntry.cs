// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerListEntry.cs" company="Exit Games GmbH">
//   Part of: Asteroid Demo,
// </copyright>
// <summary>
//  Player List Entry
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

namespace Photon.Pun.Demo.Asteroids
{
    public class PlayerListEntry : MonoBehaviour
    {
        [Header("UI References")]
        public Text PlayerNameText;

        public Image PlayerColorImage;
        public Button PlayerReadyButton;
        public Image PlayerReadyImage;

        private int ownerId;
        private bool isPlayerReady;

        #region UNITY

        public void OnEnable()
        {
            PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
        }

        public void Start()
        {
            //生成后检测自身的拥有者是否是本地玩家
            if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
            {
                //如果不是 则不显示准备按钮 因为你没权替别人准备
                PlayerReadyButton.gameObject.SetActive(false);
            }
            else
            {
                //此刻给玩家本机玩家一个初始化属性的哈希表 包含是否准备 生命数
                Hashtable initialProps = new Hashtable() {{AsteroidsGame.PLAYER_READY, isPlayerReady}, {AsteroidsGame.PLAYER_LIVES, AsteroidsGame.PLAYER_MAX_LIVES}};
                PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
                PhotonNetwork.LocalPlayer.SetScore(0);

                //给按钮被点击时的事件添加监听
                PlayerReadyButton.onClick.AddListener(() =>
                {
                    //只要被点击 bool值相反
                    isPlayerReady = !isPlayerReady;
                    SetPlayerReady(isPlayerReady);
                    //再设属性为是否准备 因为已含有相同的属性所以这次等同于更新属性
                    Hashtable props = new Hashtable() {{AsteroidsGame.PLAYER_READY, isPlayerReady}};
                    PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                    //如果是房主
                    if (PhotonNetwork.IsMasterClient)
                    {
                        //寻找LobbyMainPanel类的物体 相当于挂载了这个脚本的物体？
                        FindObjectOfType<LobbyMainPanel>().LocalPlayerPropertiesUpdated();
                    }
                });
            }
        }

        public void OnDisable()
        {
            PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
        }

        #endregion

        /// <summary>
        /// 把id和名字赋给拥有者id和名字文本
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="playerName"></param>
        public void Initialize(int playerId, string playerName)
        {
            ownerId = playerId;
            PlayerNameText.text = playerName;
        }

        /// <summary>
        /// 当玩家数改变时 本机玩家自动获取对应颜色
        /// </summary>
        private void OnPlayerNumberingChanged()
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.ActorNumber == ownerId)
                {
                    PlayerColorImage.color = AsteroidsGame.GetColor(p.GetPlayerNumber());
                }
            }
        }

        /// <summary>
        /// 给文字赋值字符串
        /// </summary>
        /// <param name="playerReady"></param>
        public void SetPlayerReady(bool playerReady)
        {
            PlayerReadyButton.GetComponentInChildren<Text>().text = playerReady ? "Ready!" : "Ready?";
            PlayerReadyImage.enabled = playerReady;
        }
    }
}