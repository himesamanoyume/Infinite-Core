using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;
using Photon.Pun;

public class AttackCube : MonoBehaviour
{
    Player owner;
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
    /// 攻击力倍率
    /// </summary>
    float ratio;
    /// <summary>
    /// 设置Cube生效时间 为0即为立刻生效
    /// </summary>
    float activeTime;
    /// <summary>
    /// 设置Cube销毁时间
    /// </summary>
    float destory;
    /// <summary>
    /// 延迟 可能会用到
    /// </summary>
    float lag;

    public Player Owner { get; private set; }
    public Vector3 InitOffset { get; set; }
    public Vector3 InitScale { get; set; }
    public Vector3 FinalOffset { get; set; }
    public Vector3 FinalScale { get; set; }
    public float InitToFinalTime { get; set; }
    public float Ratio { get; set; }
    public float ActiveTime { get; set; }
    public float Destory { get; set; }
    public float Lag { get; set; }

    BoxCollider boxCollider;

    Vector3 finalPos;

    public void InitAttackCube(Player owner, Vector3 originalDirection, float lag, Vector3[] attackCubeData, float[] attackCubeData2)
    {
        this.owner = owner;
        transform.forward = originalDirection;

        initOffset = attackCubeData[0];
        transform.position += initOffset;

        initScale = attackCubeData[1];
        transform.localScale = initScale;

        finalOffset = attackCubeData[2];
        finalPos = transform.position + finalOffset;

        finalScale = attackCubeData[3];
        initToFinalTime = attackCubeData2[0];
        ratio = attackCubeData2[1];
        activeTime = attackCubeData2[2];
        destory = attackCubeData2[3];
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

    private IEnumerator SetActiveBox()
    {
        yield return new WaitForSeconds(activeTime);
        boxCollider.enabled = true;
    }

    private void Update()
    {
        CubeSizeChange();
    }

    private void Start()
    {
        //Debug.LogWarning("Start");
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;

        Destroy(gameObject, destory);
    }

}
