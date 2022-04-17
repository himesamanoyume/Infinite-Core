using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackCubeData
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
    
    public Vector3 InitOffset { get; set; }
    public Vector3 InitScale { get; set; }
    public Vector3 FinalOffset { get; set; }
    public Vector3 FinalScale { get; set; }
    public float InitToFinalTime { get; set; }
    public float Ratio { get; set; }
    public float ActiveTime { get; set; }
    public float Destory { get; set; }
    public float Lag { get; set; }




    public AttackCubeData(Vector3 initOffset, Vector3 initScale, Vector3 finalOffset, Vector3 finalScale, float initToFinalTime, float ratio, float activeTime, float destory)
    {
        this.initOffset = initOffset;
        this.initScale = initScale;
        this.finalOffset = finalOffset;
        this.finalScale = finalScale;
        this.initToFinalTime = initToFinalTime;
        this.ratio = ratio;
        this.activeTime = activeTime;
        this.destory = destory;
    }

}



