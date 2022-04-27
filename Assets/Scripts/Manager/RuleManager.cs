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
        //实行简易规则:
        //开局1分钟后 城墙放下 可进入高收益地区
        //开局2分钟后 无限核心区域开放 高收益地区刷出4只世界BOSS
        //开局3分钟后 毒圈收缩至只有无限核心区域可活动 否则持续扣血 且玩家无法复活
        //4分钟时强制结束游戏
        //哪方击败了无限核心 可获得25%的最终伤害加成
        //任何一方的玩家被全部彻底击杀后 对方获得胜利

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
        Debug.LogWarning("1分钟");
        m_recorder.GetPhotonView().RPC("BroadcastInfo", RpcTarget.AllViaServer, "第一道城墙解除!已可进入高危区域");
        oneMinWallGroup.transform.position = new Vector3(oneMinWallGroup.transform.position.x,
            oneMinWallGroup.transform.position.y - 5,
            oneMinWallGroup.transform.position.z);
        
    }

    void TwoMin()
    {
        Debug.LogWarning("2分钟");
        m_recorder.GetPhotonView().RPC("BroadcastInfo", RpcTarget.AllViaServer, "无限核心区域开放!高危区域出现世界BOSS");
        twoMinWallGroup.transform.position = new Vector3(twoMinWallGroup.transform.position.x,
            twoMinWallGroup.transform.position.y - 5,
            twoMinWallGroup.transform.position.z);
        monsterManager.GetComponent<MonsterManager>().WorldBossMonsterGroupSpawn();
        monsterManager.GetComponent<MonsterManager>().InfiniteCoreMonsterGroupSpawn();
        m_recorder.GetPhotonView().RPC("AllGetBuff", RpcTarget.All, (int)BuffEnum.Coreless);
    }

    void ThreeMin()
    {
        Debug.LogWarning("3分钟");
        m_recorder.GetPhotonView().RPC("BroadcastInfo", RpcTarget.AllViaServer, "安全区收缩!请尽快进入无限核心区域");
        
    }

    void EndGame()
    {
        m_recorder.GetPhotonView().RPC("BroadcastInfo", RpcTarget.AllViaServer, "游戏结束");
        Debug.LogWarning("4分钟");
    }
}
