using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;
using Photon.Pun;

public class AttackCube : MonoBehaviour
{
    Player owner;
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
    /// Cube�����α仯����
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
