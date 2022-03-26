// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Spaceship.cs" company="Exit Games GmbH">
//   Part of: Asteroid Demo,
// </copyright>
// <summary>
//  Spaceship
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;

using UnityEngine;

using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Pun.Demo.Asteroids
{
    public class Spaceship : MonoBehaviour
    {
        public float RotationSpeed = 90.0f;
        public float MovementSpeed = 2.0f;
        public float MaxSpeed = 0.2f;

        public ParticleSystem Destruction;
        public GameObject EngineTrail;
        public GameObject BulletPrefab;

        private PhotonView photonView;

#pragma warning disable 0109
        private new Rigidbody rigidbody;
        private new Collider collider;
        private new Renderer renderer;
#pragma warning restore 0109

        private float rotation = 0.0f;
        private float acceleration = 0.0f;
        private float shootingTimer = 0.0f;

        private bool controllable = true;

        #region UNITY

        public void Awake()
        {
            photonView = GetComponent<PhotonView>();

            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
            renderer = GetComponent<Renderer>();
        }

        public void Start()
        {
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                r.material.color = AsteroidsGame.GetColor(photonView.Owner.GetPlayerNumber());
            }
        }

        public void Update()
        {
            if (!photonView.AmOwner || !controllable)
            {
                return;
            }

            // we don't want the master client to apply input to remote ships while the remote player is inactive
            if (this.photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                return;
            }

            rotation = Input.GetAxis("Horizontal");
            acceleration = Input.GetAxis("Vertical");

            if (Input.GetButton("Jump") && shootingTimer <= 0.0)
            {
                shootingTimer = 0.2f;

                photonView.RPC("Fire", RpcTarget.AllViaServer, rigidbody.position, rigidbody.rotation);
            }

            if (shootingTimer > 0.0f)
            {
                shootingTimer -= Time.deltaTime;
            }
        }

        public void FixedUpdate()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (!controllable)
            {
                return;
            }

            Quaternion rot = rigidbody.rotation * Quaternion.Euler(0, rotation * RotationSpeed * Time.fixedDeltaTime, 0);
            rigidbody.MoveRotation(rot);

            Vector3 force = (rot * Vector3.forward) * acceleration * 1000.0f * MovementSpeed * Time.fixedDeltaTime;
            rigidbody.AddForce(force);

            if (rigidbody.velocity.magnitude > (MaxSpeed * 1000.0f))
            {
                rigidbody.velocity = rigidbody.velocity.normalized * MaxSpeed * 1000.0f;
            }

            CheckExitScreen();
        }

        #endregion

        #region COROUTINES
        //协同程序

        private IEnumerator WaitForRespawn()
        {
            //等待指定复活秒数
            yield return new WaitForSeconds(AsteroidsGame.PLAYER_RESPAWN_TIME);
            //时间结束后从服务器发起远程调用RespawnSpaceship方法
            photonView.RPC("RespawnSpaceship", RpcTarget.AllViaServer);
        }

        #endregion

        #region PUN CALLBACKS

        [PunRPC]
        public void DestroySpaceship()
        {
            //刚体reset
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            //碰撞和渲染取消
            collider.enabled = false;
            renderer.enabled = false;
            //可操作取消
            controllable = false;
            //引擎拖尾取消
            EngineTrail.SetActive(false);
            //播放被破坏的粒子效果
            Destruction.Play();

            //如果被破坏的是自己
            if (photonView.IsMine)
            {
                object lives;
                //尝试获取自己的生命次数 得到值赋给lives 能获得则为次数 不能则为false (localplayer指本人)
                if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(AsteroidsGame.PLAYER_LIVES, out lives))
                {
                    //
                    PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable 
                    {
                        {
                            //更新生命数 小于1则为0 否则减1
                            AsteroidsGame.PLAYER_LIVES,
                            ((int) lives <= 1) ? 0 : ((int) lives - 1)
                        }
                    });
                    //如果生命次数大于1 
                    if (((int) lives) > 1)
                    {
                        //协程开始WaitForRespawn
                        StartCoroutine("WaitForRespawn");
                    }
                }
            }
        }

        [PunRPC]
        public void Fire(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
        {
            //延迟=服务器时间 减去 发起给服务器的时间
            float lag = (float) (PhotonNetwork.Time - info.SentServerTime);
            GameObject bullet;

            /** Use this if you want to fire one bullet at a time **/
            bullet = Instantiate(BulletPrefab, position, Quaternion.identity) as GameObject;
            //初始化子弹 传递发射子弹的拥有者 初始方向 延迟的绝对值
            bullet.GetComponent<Bullet>().InitializeBullet(photonView.Owner, (rotation * Vector3.forward), Mathf.Abs(lag));


            /** Use this if you want to fire two bullets at once **/
            //Vector3 baseX = rotation * Vector3.right;
            //Vector3 baseZ = rotation * Vector3.forward;

            //Vector3 offsetLeft = -1.5f * baseX - 0.5f * baseZ;
            //Vector3 offsetRight = 1.5f * baseX - 0.5f * baseZ;

            //bullet = Instantiate(BulletPrefab, rigidbody.position + offsetLeft, Quaternion.identity) as GameObject;
            //bullet.GetComponent<Bullet>().InitializeBullet(photonView.Owner, baseZ, Mathf.Abs(lag));
            //bullet = Instantiate(BulletPrefab, rigidbody.position + offsetRight, Quaternion.identity) as GameObject;
            //bullet.GetComponent<Bullet>().InitializeBullet(photonView.Owner, baseZ, Mathf.Abs(lag));
        }

        /// <summary>
        /// 使碰撞体 渲染开启 可操作开启
        /// </summary>
        [PunRPC]
        public void RespawnSpaceship()
        {
            collider.enabled = true;
            renderer.enabled = true;

            controllable = true;

            EngineTrail.SetActive(true);
            Destruction.Stop();
        }
        
        #endregion

        private void CheckExitScreen()
        {
            if (Camera.main == null)
            {
                return;
            }
            
            if (Mathf.Abs(rigidbody.position.x) > (Camera.main.orthographicSize * Camera.main.aspect))
            {
                rigidbody.position = new Vector3(-Mathf.Sign(rigidbody.position.x) * Camera.main.orthographicSize * Camera.main.aspect, 0, rigidbody.position.z);
                rigidbody.position -= rigidbody.position.normalized * 0.1f; // offset a little bit to avoid looping back & forth between the 2 edges 
            }

            if (Mathf.Abs(rigidbody.position.z) > Camera.main.orthographicSize)
            {
                rigidbody.position = new Vector3(rigidbody.position.x, rigidbody.position.y, -Mathf.Sign(rigidbody.position.z) * Camera.main.orthographicSize);
                rigidbody.position -= rigidbody.position.normalized * 0.1f; // offset a little bit to avoid looping back & forth between the 2 edges 
            }
        }
    }
}