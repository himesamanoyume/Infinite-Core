// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerNumbering.cs" company="Exit Games GmbH">
//   Part of: Asteroid Demo,
// </copyright>
// <summary>
//  Player Overview Panel
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

namespace Photon.Pun.Demo.Asteroids
{
    public class PlayerOverviewPanel : MonoBehaviourPunCallbacks
    {
        //玩家总览记录预制体
        public GameObject PlayerOverviewEntryPrefab;

        //字典 玩家列表记录
        [SerializeField]
        private Dictionary<int, GameObject> playerListEntries;

        #region UNITY

        public void Awake()
        {
            //实例化
            playerListEntries = new Dictionary<int, GameObject>();

            //对列表内每个玩家进行
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                //生成一个玩家信息文字预制体
                GameObject entry = Instantiate(PlayerOverviewEntryPrefab);
                //设定父物体
                entry.transform.SetParent(gameObject.transform);
                //设定初始大小
                entry.transform.localScale = Vector3.one;
                //获取玩家对应的颜色
                entry.GetComponent<Text>().color = AsteroidsGame.GetColor(p.GetPlayerNumber());
                //获取玩家信息
                entry.GetComponent<Text>().text = string.Format("{0}\nScore: {1}\nLives: {2}", p.NickName, p.GetScore(), AsteroidsGame.PLAYER_MAX_LIVES);
                //把玩家id和GameObject添加到字典
                playerListEntries.Add(p.ActorNumber, entry);
            }
        }

        #endregion

        #region PUN CALLBACKS

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            //定义一个物体
            GameObject go = null;
            //尝试获取字典里的退出房间玩家的id 如果取到了则返回这个玩家的物体
            if (this.playerListEntries.TryGetValue(otherPlayer.ActorNumber, out go))
            {
                //销毁这个字典里的对应id的所指向的物体
                Destroy(playerListEntries[otherPlayer.ActorNumber]);
                //再消除字典里对应的值
                playerListEntries.Remove(otherPlayer.ActorNumber);
            }
        }

        //当玩家属性更新时
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            GameObject entry;
            //尝试获取字典里的退出房间玩家的id 如果取到了则返回这个玩家的物体
            if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
            {
                //再生成一次玩家信息的文字
                entry.GetComponent<Text>().text = string.Format("{0}\nScore: {1}\nLives: {2}", targetPlayer.NickName, targetPlayer.GetScore(), targetPlayer.CustomProperties[AsteroidsGame.PLAYER_LIVES]);
            }
        }

        #endregion
    }
}