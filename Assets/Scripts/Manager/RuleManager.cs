using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class RuleManager : MonoBehaviourPunCallbacks
{
    public GameObject monsterManager;

    public GameObject gameTime;
    public int m_gameTimeInt;

    GameTime m_gameTime;

    bool isOneMin;
    bool isTwoMin;
    bool isThreeMin;
    bool isFourMin;

    void Start()
    {
        m_gameTime = gameTime.GetComponent<GameTime>();
    }

    void Update()
    {
        //ʵ�м��׹���:
        //����1���Ӻ� ��ǽ���� �ɽ�����������
        //����2���Ӻ� ���޺������򿪷� ��Ȧ��ʼ���� ���������ˢ��4ֻ����BOSS
        //����3���Ӻ� ��Ȧ������ֻ�����޺�������ɻ ����޷�����
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
        
    }

    void TwoMin()
    {
        Debug.LogWarning("2����");
        monsterManager.GetComponent<MonsterManager>().WorldBossMonsterGroupSpawn();
    }

    void ThreeMin()
    {
        Debug.LogWarning("3����");
        monsterManager.GetComponent<MonsterManager>().InfiniteCoreMonsterGroupSpawn();
    }

    void EndGame()
    {
        Debug.LogWarning("4����");
    }
}
