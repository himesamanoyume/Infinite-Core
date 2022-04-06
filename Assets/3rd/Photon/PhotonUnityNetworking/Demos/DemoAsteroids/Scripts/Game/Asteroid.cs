// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Asteroid.cs" company="Exit Games GmbH">
//   Part of: Asteroid Demo
// </copyright>
// <summary>
//  Asteroid Component
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

using Random = UnityEngine.Random;
using Photon.Pun.UtilityScripts;

namespace Photon.Pun.Demo.Asteroids
{
    public class Asteroid : MonoBehaviour
    {
        public bool isLargeAsteroid;

        private bool isDestroyed;

        private PhotonView photonView;

#pragma warning disable 0109
        private new Rigidbody rigidbody;
#pragma warning restore 0109

        #region UNITY

        public void Awake()
        {
            photonView = GetComponent<PhotonView>();

            rigidbody = GetComponent<Rigidbody>();

            //InstantiationData 这是在调用PhotonNetwork.Instantiate时传递的实例化数据(如果它被用于生成这个预制体)
            if (photonView.InstantiationData != null)
            {
                //给rigidbody添加力 没有值 只是赋予了一个序号
                rigidbody.AddForce((Vector3) photonView.InstantiationData[0]);
                //添加力矩
                rigidbody.AddTorque((Vector3) photonView.InstantiationData[1]);
                //把是否为大行星作为bool值也当作实例化数据
                isLargeAsteroid = (bool) photonView.InstantiationData[2];
            }
        }

        public void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }
            //如果该物体的x的绝对值>相机在正交模式下一半的大小*纵横比(宽度除以高度)  或  该物体z的绝对值>相机在正交模式下一半的大小时
            if (Mathf.Abs(transform.position.x) > Camera.main.orthographicSize * Camera.main.aspect || Mathf.Abs(transform.position.z) > Camera.main.orthographicSize)
            {
                // Out of the screen
                //说明出了飞出了屏幕 销毁
                PhotonNetwork.Destroy(gameObject);
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            //如果已经被摧毁则不触发
            if (isDestroyed)
            {
                return;
            }

            //如果对比碰撞体的tag为子弹
            if (collision.gameObject.CompareTag("Bullet"))
            {
                //如果是自己的
                if (photonView.IsMine)
                {
                    //获取碰撞体的子弹组件
                    Bullet bullet = collision.gameObject.GetComponent<Bullet>();
                    //给子弹的拥有者加分 是大行星加2分 否则加1分
                    bullet.Owner.AddScore(isLargeAsteroid ? 2 : 1);
                    //全局销毁
                    DestroyAsteroidGlobally();
                }
                else
                {
                    //本地销毁
                    DestroyAsteroidLocally();
                }
            }//如果检测到tag为玩家
            else if (collision.gameObject.CompareTag("Player"))
            {
                //如果是自己的
                if (photonView.IsMine)
                {
                    //通过这个碰撞体上PhotonView组件中远程调用所有客户端的DestroySpaceship方法
                    collision.gameObject.GetComponent<PhotonView>().RPC("DestroySpaceship", RpcTarget.All);
                    //全局销毁
                    DestroyAsteroidGlobally();
                }
            }
        }

        #endregion

        private void DestroyAsteroidGlobally()
        {
            //已被销毁变为true
            isDestroyed = true;

            //如果是大流星
            if (isLargeAsteroid)
            {
                //随机一个爆炸出小流星的数量
                int numberToSpawn = Random.Range(3, 6);

                for (int counter = 0; counter < numberToSpawn; ++counter)
                {
                    Vector3 force = Quaternion.Euler(0, counter * 360.0f / numberToSpawn, 0) * Vector3.forward * Random.Range(0.5f, 1.5f) * 300.0f;
                    Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);
                    //传递力，力矩，不是大行星，服务器时间 作为实例化数据
                    object[] instantiationData = {force, torque, false, PhotonNetwork.Time};

                    //
                    PhotonNetwork.InstantiateRoomObject("SmallAsteroid", transform.position + force.normalized * 10.0f, Quaternion.Euler(0, Random.value * 180.0f, 0), 0, instantiationData);
                }
            }
            //刷出小行星后销毁大行星
            Debug.LogWarning("DestroyAsteroidGlobally");
            PhotonNetwork.Destroy(gameObject);
        }

        private void DestroyAsteroidLocally()
        {
            isDestroyed = true;
            //不渲染了
            GetComponent<Renderer>().enabled = false;
            Debug.LogWarning("DestroyAsteroidLocally");
        }
    }
}