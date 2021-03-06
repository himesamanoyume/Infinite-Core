using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;
using Photon.Pun;

public class MonsterAttackCube : MonoBehaviour
{
    /// <summary>
    /// 初始Cube 坐标偏移
    /// </summary>
    Vector3 initOffset;
    /// <summary>
    /// 初始Cube 大小
    /// </summary>
    Vector3 initScale;
    /// <summary>
    /// 最终Cube 坐标偏移
    /// </summary>
    Vector3 finalOffset;
    /// <summary>
    /// 最终Cube 大小
    /// </summary>
    Vector3 finalScale;
    /// <summary>
    /// 变化时间 时间结束时自动销毁
    /// </summary>
    float initToFinalTime;
    /// <summary>
    /// 设置Cube生效时间 为0即为立刻生效
    /// </summary>
    float activeTime;
    /// <summary>
    /// 设置Cube销毁时间
    /// </summary>
    //float destory;
    /// <summary>
    /// 延迟 可能会用到
    /// </summary>
    float lag;
    /// <summary>
    /// 计算完爆伤后的攻击力
    /// </summary>
    [SerializeField]
    float finalAttack;
    /// <summary>
    /// 最终伤害倍率
    /// </summary>
    [SerializeField]
    float finalDamage;

    BoxCollider boxCollider;

    Vector3 finalPos;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;

        Destroy(gameObject, initToFinalTime);
    }

    private void FixedUpdate()
    {
        CubeSizeChange();
    }

    public void InitAttackCube(Vector3 originalDirection, float lag, Vector3[] attackCubeData, float[] attackCubeData2)
    {
        transform.forward = originalDirection;

        initOffset = attackCubeData[0];
        transform.position += initOffset;

        initScale = attackCubeData[1];
        transform.localScale = initScale;

        finalOffset = attackCubeData[2];
        finalPos = transform.position + finalOffset;

        finalScale = attackCubeData[3];

        initToFinalTime = attackCubeData2[0];
        finalDamage = attackCubeData2[1];
        activeTime = attackCubeData2[2];
        finalAttack = attackCubeData2[3];

        this.lag = lag;

        StartCoroutine("SetActiveBox");

    }

    /// <summary>
    /// Cube的外形变化函数
    /// </summary>
    void CubeSizeChange()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, finalScale, 0.1f);
        transform.position = Vector3.Lerp(transform.position, finalPos, 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("PlayerModel"))
        {
            PhotonView photonView = other.gameObject.GetPhotonView();

            int otherActorNumber = photonView.OwnerActorNr;

            CharManager charManager = GameObject.Find("CharManager").GetComponent<CharManager>();
            charManager.recorders.TryGetValue(otherActorNumber, out GameObject recorder);

            photonView.RPC("PlayerDamaged", RpcTarget.All, otherActorNumber, -2, finalAttack, finalDamage);
        }        
    }

    private IEnumerator SetActiveBox()
    {
        yield return new WaitForSeconds(activeTime);
        boxCollider.enabled = true;
    }
}
