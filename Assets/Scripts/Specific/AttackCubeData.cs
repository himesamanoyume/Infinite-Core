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

    //public byte InitCenter { get; set; }
    //public byte InitSize { get; set; }
    //public byte FinalCenter { get; set; }
    //public byte FinalSize { get; set; }
    //public byte InitToFinalTime { get; set; }
    //public byte Ratio { get; set; }
    //public byte ActiveTime { get; set; }
    //public byte Lag { get; set; }

    //public static object Deserialize(byte[] data)
    //{
    //    var result = new AttackCubeData();
    //    result.InitCenter = data[0];
    //    result.InitSize = data[1];
    //    result.FinalCenter = data[2];
    //    result.FinalSize = data[3];
    //    result.InitToFinalTime = data[4];
    //    result.Ratio = data[5];
    //    result.ActiveTime = data[6];
    //    result.Lag = data[7];
    //    return result;
    //}

    //public static byte[] Serialize(object customType)
    //{
    //    var c = (AttackCubeData)customType;
    //    return new byte[] { c.InitCenter, c.InitSize, c.FinalCenter, c.FinalSize, c.InitToFinalTime, c.Ratio, c.ActiveTime, c.Lag };
    //}


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



