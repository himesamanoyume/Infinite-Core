using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using System.Threading;
using System;

public class MonsterManager : MonoBehaviour
{
    public Transform redPatrolMonsterSpawnPos0;
    public Transform redPatrolMonsterSpawnPos1;
    public Transform redPatrolMonsterSpawnPos2;
    public Transform redPatrolMonsterSpawnPos3;

    public Transform bluePatrolMonsterSpawnPos4;
    public Transform bluePatrolMonsterSpawnPos5;
    public Transform bluePatrolMonsterSpawnPos6;
    public Transform bluePatrolMonsterSpawnPos7;

    public Transform redWorldBossMonsterSpawnPos8;
    public Transform redWorldBossMonsterSpawnPos9;
    public Transform blueWorldBossMonsterSpawnPos10;
    public Transform blueWorldBossMonsterSpawnPos11;

    public Transform infiniteCoreMonsterSpawnPos12;

    List<Transform> patrolPosList = new List<Transform>();
    List<Transform> worldBossPosList = new List<Transform>();
    List<Transform> infiniteCorePosList = new List<Transform>();
   
    public GameObject patrolMonsterGroup;

    void Start()
    {
        patrolPosList.Add(redPatrolMonsterSpawnPos0);
        patrolPosList.Add(redPatrolMonsterSpawnPos1);
        patrolPosList.Add(redPatrolMonsterSpawnPos2);
        patrolPosList.Add(redPatrolMonsterSpawnPos3);
        patrolPosList.Add(bluePatrolMonsterSpawnPos4);
        patrolPosList.Add(bluePatrolMonsterSpawnPos5);
        patrolPosList.Add(bluePatrolMonsterSpawnPos6);
        patrolPosList.Add(bluePatrolMonsterSpawnPos7);

        worldBossPosList.Add(redWorldBossMonsterSpawnPos8);
        worldBossPosList.Add(redWorldBossMonsterSpawnPos9);
        worldBossPosList.Add(blueWorldBossMonsterSpawnPos10);
        worldBossPosList.Add(blueWorldBossMonsterSpawnPos11);

        infiniteCorePosList.Add(infiniteCoreMonsterSpawnPos12);

        PatrolMonsterGroupSpawn();

    }

    void Update()
    {
       
    }

    void PatrolMonsterGroupSpawn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < patrolPosList.Count; i++)
            {
                GameObject gameObject = PhotonNetwork.InstantiateRoomObject(patrolMonsterGroup.name, patrolPosList[i].position, Quaternion.identity);
                gameObject.GetComponent<MonsterGroup>().InitGroup(MonsterType.PatrolMonster, i);
            }
        }
    }

    public void WorldBossMonsterGroupSpawn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < worldBossPosList.Count; i++)
            {
                GameObject gameObject = PhotonNetwork.InstantiateRoomObject(patrolMonsterGroup.name, worldBossPosList[i].position, Quaternion.identity);
                gameObject.GetComponent<MonsterGroup>().InitGroup(MonsterType.WorldMonster, i);
            }
        }
    }

    public void InfiniteCoreMonsterGroupSpawn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < infiniteCorePosList.Count; i++)
            {
                GameObject gameObject = PhotonNetwork.InstantiateRoomObject(patrolMonsterGroup.name, infiniteCorePosList[i].position, Quaternion.identity);
                gameObject.GetComponent<MonsterGroup>().InitGroup(MonsterType.InfiniteCore, i);
            }
        }
    }
}




