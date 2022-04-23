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

    List<Transform> patrolPosList = new List<Transform>();
   
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

}




