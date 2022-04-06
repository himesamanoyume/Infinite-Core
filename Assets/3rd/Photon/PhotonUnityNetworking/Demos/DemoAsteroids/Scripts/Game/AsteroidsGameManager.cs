// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AsteroidsGameManager.cs" company="Exit Games GmbH">
//   Part of: Asteroid demo
// </copyright>
// <summary>
//  Game Manager for the Asteroid Demo
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Pun.Demo.Asteroids
{
    public class AsteroidsGameManager : MonoBehaviourPunCallbacks
    {
        public static AsteroidsGameManager Instance = null;

        public Text InfoText;

        public GameObject[] AsteroidPrefabs;

        #region UNITY

        public void Awake()
        {
            Instance = this;
        }

        public override void OnEnable()
        {
            base.OnEnable();

            CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
        }

        public void Start()
        {
            Hashtable props = new Hashtable
            {
                {AsteroidsGame.PLAYER_LOADED_LEVEL, true}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        public override void OnDisable()
        {
            base.OnDisable();

            CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
        }

        #endregion

        #region COROUTINES

        //协程生成行星
        private IEnumerator SpawnAsteroid()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(AsteroidsGame.ASTEROIDS_MIN_SPAWN_TIME, AsteroidsGame.ASTEROIDS_MAX_SPAWN_TIME));

                //随机返回一个2维向量
                Vector2 direction = Random.insideUnitCircle;
                Vector3 position = Vector3.zero;

                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    // Make it appear on the left/right side
                    position = new Vector3(Mathf.Sign(direction.x) * Camera.main.orthographicSize * Camera.main.aspect, 0, direction.y * Camera.main.orthographicSize);
                }
                else
                {
                    // Make it appear on the top/bottom
                    position = new Vector3(direction.x * Camera.main.orthographicSize * Camera.main.aspect, 0, Mathf.Sign(direction.y) * Camera.main.orthographicSize);
                }

                // Offset slightly so we are not out of screen at creation time (as it would destroy the asteroid right away)
                position -= position.normalized * 0.1f;


                Vector3 force = -position.normalized * 1000.0f;
                Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);

                //都要传递实例化数据
                object[] instantiationData = {force, torque, true};

                PhotonNetwork.InstantiateRoomObject("BigAsteroid", position, Quaternion.Euler(Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f), 0, instantiationData);
            }
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        /// <param name="winner"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        private IEnumerator EndOfGame(string winner, int score)
        {
            float timer = 5.0f;

            while (timer > 0.0f)
            {
                InfoText.text = string.Format("Player {0} won with {1} points.\n\n\nReturning to login screen in {2} seconds.", winner, score, timer.ToString("n2"));

                //每帧都会结束 所以能够实现小数点后.00几的时间倒数显示
                yield return new WaitForEndOfFrame();

                timer -= Time.deltaTime;
            }

            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region PUN CALLBACKS

        public override void OnDisconnected(DisconnectCause cause)
        {
            //断开连接的时候自身加载到房间场景
            UnityEngine.SceneManagement.SceneManager.LoadScene("DemoAsteroids-LobbyScene");
        }

        public override void OnLeftRoom()
        {
            //离开房间后断开连接
            PhotonNetwork.Disconnect();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            //如果我是主机
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {
                StartCoroutine(SpawnAsteroid());//协程将由主机负责生成小行星
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            //当玩家离开房间时检测游戏是否结束
            CheckEndOfGame();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            //如果哈希表中含有玩家生命数
            if (changedProps.ContainsKey(AsteroidsGame.PLAYER_LIVES))
            {
                //检测游戏是否结束
                CheckEndOfGame();
                return;
            }

            //如果不是房主则返回
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }


            // if there was no countdown yet, the master client (this one) waits until everyone loaded the level and sets a timer start
            //如果还没有倒计时，主客户端将等待所有人加载关卡并设置计时器开始
            int startTimestamp;//开始时间戳
            bool startTimeIsSet = CountdownTimer.TryGetStartTime(out startTimestamp);//尝试获取开始时间 成功则返回startTimestamp 并将成功与否赋给startTimeIsSet

            if (changedProps.ContainsKey(AsteroidsGame.PLAYER_LOADED_LEVEL))//如果含有该key
            {
                //检测所有玩家是否都加载成功了关卡
                if (CheckAllPlayerLoadedLevel())
                {
                    //如果startTimeIsSet为否 说明没开始倒计时
                    if (!startTimeIsSet)
                    {
                        //则开始倒计时
                        CountdownTimer.SetStartTime();
                    }
                }
                else
                {
                    // not all players loaded yet. wait:
                    Debug.Log("setting text waiting for players! ",this.InfoText);
                    InfoText.text = "Waiting for other players...";
                }
            }
        
        }

        #endregion

        
        // called by OnCountdownTimerIsExpired() when the timer ended. 当该方法里的倒计时结束时调用
        private void StartGame()
        {
            Debug.Log("StartGame!");

            // on rejoin, we have to figure out if the spaceship exists or not
            // if this is a rejoin (the ship is already network instantiated and will be setup via event) we don't need to call PN.Instantiate
            //在重新加入时，我们必须弄清楚飞船是否存在，如果这是一个重新加入(飞船已经被网络实例化，并将通过事件设置)，我们不需要调用PUN_Instantiate


            float angularStart = (360.0f / PhotonNetwork.CurrentRoom.PlayerCount) * PhotonNetwork.LocalPlayer.GetPlayerNumber();
            float x = 20.0f * Mathf.Sin(angularStart * Mathf.Deg2Rad);
            float z = 20.0f * Mathf.Cos(angularStart * Mathf.Deg2Rad);
            Vector3 position = new Vector3(x, 0.0f, z);
            Quaternion rotation = Quaternion.Euler(0.0f, angularStart, 0.0f);

            PhotonNetwork.Instantiate("Spaceship", position, rotation, 0);      // avoid this call on rejoin (ship was network instantiated before) 避免在rejoin时调用这个函数(指之前就已经网络实例化的ship)

            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(SpawnAsteroid());
            }
        }

        /// <summary>
        /// 检测是否所有玩家都加载完成
        /// </summary>
        /// <returns></returns>
        private bool CheckAllPlayerLoadedLevel()
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object playerLoadedLevel;

                //当检测到属性含有"玩家已加载完关卡"的字符串时
                if (p.CustomProperties.TryGetValue(AsteroidsGame.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
                {
                    //如果循环到的该玩家加载完成 则直接进入下一轮 如果没加载完成 将跳过if语句进入的return返回false 全都完成则能够返回true
                    if ((bool) playerLoadedLevel)
                    {
                        continue;
                    }
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// 检测游戏是否结束
        /// </summary>
        private void CheckEndOfGame()
        {
            bool allDestroyed = true;

            foreach (Player p in PhotonNetwork.PlayerList)//挨个查玩家的生命数 一旦有大于0的 都不能结束
            {
                object lives;
                if (p.CustomProperties.TryGetValue(AsteroidsGame.PLAYER_LIVES, out lives))
                {
                    if ((int) lives > 0)
                    {
                        allDestroyed = false;
                        break;
                    }
                }
            }

            if (allDestroyed)//如果都死完了 则主机停止所有协程(即小行星生成)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    StopAllCoroutines();
                }

                string winner = "";
                int score = -1;

                foreach (Player p in PhotonNetwork.PlayerList)//把最高分的玩家显示出来
                {
                    if (p.GetScore() > score)
                    {
                        winner = p.NickName;
                        score = p.GetScore();
                    }
                }

                StartCoroutine(EndOfGame(winner, score));//结束
            }
        }

        /// <summary>
        /// 当倒计时结束时
        /// </summary>
        private void OnCountdownTimerIsExpired()
        {
            StartGame();
        }
    }
}