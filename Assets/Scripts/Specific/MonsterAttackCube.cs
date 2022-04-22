using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;
using Photon.Pun;

public class MonsterAttackCube : MonoBehaviour
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
    /// ����Cube��Чʱ�� Ϊ0��Ϊ������Ч
    /// </summary>
    float activeTime;
    /// <summary>
    /// ����Cube����ʱ��
    /// </summary>
    //float destory;
    /// <summary>
    /// �ӳ� ���ܻ��õ�
    /// </summary>
    float lag;
    /// <summary>
    /// �����걬�˺�Ĺ�����
    /// </summary>
    [SerializeField]
    float finalAttack;
    /// <summary>
    /// �����˺�����
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
    /// Cube�����α仯����
    /// </summary>
    void CubeSizeChange()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, finalScale, 0.1f);
        transform.position = Vector3.Lerp(transform.position, finalPos, 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("PlayerModel")) return;

        PhotonView photonView = other.gameObject.GetPhotonView();

        int otherActorNumber = photonView.OwnerActorNr;

        CharManager charManager = GameObject.Find("CharManager").GetComponent<CharManager>();
        charManager.recorders.TryGetValue(otherActorNumber, out GameObject recorder);

        photonView.RPC("PlayerDamaged", RpcTarget.AllViaServer, otherActorNumber, -2, finalAttack, finalDamage);
    }

    private IEnumerator SetActiveBox()
    {
        yield return new WaitForSeconds(activeTime);
        boxCollider.enabled = true;
    }
}