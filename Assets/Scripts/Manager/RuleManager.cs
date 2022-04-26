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
        //实行简易规则:
        //开局1分钟后 城墙放下 可进入高收益地区
        //开局2分钟后 无限核心区域开放 毒圈开始收缩 高收益地区刷出4只世界BOSS
        //开局3分钟后 毒圈收缩至只有无限核心区域可活动 玩家无法复活
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
        
    }

    void TwoMin()
    {
        Debug.LogWarning("2分钟");
        monsterManager.GetComponent<MonsterManager>().WorldBossMonsterGroupSpawn();
    }

    void ThreeMin()
    {
        Debug.LogWarning("3分钟");
        monsterManager.GetComponent<MonsterManager>().InfiniteCoreMonsterGroupSpawn();
    }

    void EndGame()
    {
        Debug.LogWarning("4分钟");
    }
}
