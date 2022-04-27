using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class RuleManager : MonoBehaviourPunCallbacks
{
    public GameObject monsterManager;
    public GameObject charManager;

    CharBase m_charBase;
    GameObject m_recorder;

    public GameObject gameTime;
    public int m_gameTimeInt;

    public GameObject oneMinWallGroup;
    public GameObject twoMinWallGroup;

    GameTime m_gameTime;

    bool isOneMin;
    bool isTwoMin;
    bool isThreeMin;
    bool isFourMin;

    void Start()
    {
        m_gameTime = gameTime.GetComponent<GameTime>();
        StartCoroutine(InitRecorder());
        
    }

    IEnumerator InitRecorder()
    {
        yield return new WaitForSeconds(1);
        charManager.GetComponent<CharManager>().FindPlayerRecorder(PhotonNetwork.LocalPlayer.ActorNumber, out GameObject recorder, out CharBase charBase);

        m_charBase = charBase;
        m_recorder = recorder;
    }

    void Update()
    {
        //ʵ�м��׹���:
        //����1���Ӻ� ��ǽ���� �ɽ�����������
        //����2���Ӻ� ���޺������򿪷� ���������ˢ��4ֻ����BOSS
        //����3���Ӻ� ��Ȧ������ֻ�����޺�������ɻ ���������Ѫ ������޷�����
        //4����ʱǿ�ƽ�����Ϸ
        //�ķ����������޺��� �ɻ��25%�������˺��ӳ�
        //�κ�һ������ұ�ȫ�����׻�ɱ�� �Է����ʤ��

        m_gameTimeInt = m_gameTime.gameTimeInt;

        if (PhotonNetwork.IsMasterClient)
        {
            if (!isOneMin && m_gameTimeInt >= 60)
            {
                isOneMin = true;
                OneMin();
            }

            if (!isTwoMin && m_gameTimeInt >= 120)
            {
                isTwoMin = true;
                TwoMin();
            }

            if (!isThreeMin && m_gameTimeInt >= 180)
            {
                isThreeMin = true;
                ThreeMin();
            }

            if (!isFourMin && m_gameTimeInt >= 240)
            {
                isFourMin = true;
                EndGame();
            }
        }
    }

    void OneMin()
    {
        Debug.LogWarning("1����");
        m_recorder.GetPhotonView().RPC("BroadcastInfo", RpcTarget.AllViaServer, "��һ����ǽ���!�ѿɽ����Σ����");
        oneMinWallGroup.transform.position = new Vector3(oneMinWallGroup.transform.position.x,
            oneMinWallGroup.transform.position.y - 5,
            oneMinWallGroup.transform.position.z);
        
    }

    void TwoMin()
    {
        Debug.LogWarning("2����");
        m_recorder.GetPhotonView().RPC("BroadcastInfo", RpcTarget.AllViaServer, "���޺������򿪷�!��Σ�����������BOSS");
        twoMinWallGroup.transform.position = new Vector3(twoMinWallGroup.transform.position.x,
            twoMinWallGroup.transform.position.y - 5,
            twoMinWallGroup.transform.position.z);
        monsterManager.GetComponent<MonsterManager>().WorldBossMonsterGroupSpawn();
        monsterManager.GetComponent<MonsterManager>().InfiniteCoreMonsterGroupSpawn();
        m_recorder.GetPhotonView().RPC("AllGetBuff", RpcTarget.All, (int)BuffEnum.Coreless);
    }

    void ThreeMin()
    {
        Debug.LogWarning("3����");
        m_recorder.GetPhotonView().RPC("BroadcastInfo", RpcTarget.AllViaServer, "��ȫ������!�뾡��������޺�������");
        
    }

    void EndGame()
    {
        m_recorder.GetPhotonView().RPC("BroadcastInfo", RpcTarget.AllViaServer, "��Ϸ����");
        Debug.LogWarning("4����");
    }
}
