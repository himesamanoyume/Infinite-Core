using Photon.Realtime;
using UnityEngine;

namespace Photon.Pun.Demo.Asteroids
{
    public class Bullet : MonoBehaviour
    {
        public Player Owner { get; private set; }

        public void Start()
        {
            Destroy(gameObject, 3.0f);//发射后3秒自动销毁
        }

        public void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// 初始化子弹
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="originalDirection"></param>
        /// <param name="lag"></param>
        public void InitializeBullet(Player owner, Vector3 originalDirection, float lag)
        {
            Owner = owner;

            transform.forward = originalDirection;

            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.velocity = originalDirection * 200.0f;
            rigidbody.position += rigidbody.velocity * lag;//速度乘延迟加在位置上
        }
    }
}