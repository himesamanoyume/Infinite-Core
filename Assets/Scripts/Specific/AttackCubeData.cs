using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackCubeData
{
    /// <summary>
    /// ��ʼCube ����ƫ��
    /// </summary>
    Vector3 initOffset;
    /// <summary>
    /// ��ʼCube ��С
    /// </summary>
    Vector3 initScale;
    /// <summary>
    /// ����Cube ����ƫ��
    /// </summary>
    Vector3 finalOffset;
    /// <summary>
    /// ����Cube ��С
    /// </summary>
    Vector3 finalScale;
    /// <summary>
    /// �仯ʱ�� ʱ�����ʱ�Զ�����
    /// </summary>
    float initToFinalTime;
    /// <summary>
    /// ����������
    /// </summary>
    float ratio;
    /// <summary>
    /// ����Cube��Чʱ�� Ϊ0��Ϊ������Ч
    /// </summary>
    float activeTime;
    /// <summary>
    /// ����Cube����ʱ��
    /// </summary>
    float destory;
    /// <summary>
    /// �ӳ� ���ܻ��õ�
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



