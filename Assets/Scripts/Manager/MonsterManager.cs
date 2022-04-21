using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class MonsterManager : MonoBehaviour
{
    public Transform redPatrolMonsterSpawnPos1;
    public Transform redPatrolMonsterSpawnPos2;
    public Transform redPatrolMonsterSpawnPos3;
    public Transform redPatrolMonsterSpawnPos4;

    public Transform bluePatrolMonsterSpawnPos1;
    public Transform bluePatrolMonsterSpawnPos2;
    public Transform bluePatrolMonsterSpawnPos3;
    public Transform bluePatrolMonsterSpawnPos4;

    Dictionary<Transform, bool> isEmptyPatrol = new Dictionary<Transform, bool>();

    public GameObject patrolMonsterGroup;


    // Start is called before the first frame update
    void Start()
    {
        isEmptyPatrol.Add(redPatrolMonsterSpawnPos1, true);
        isEmptyPatrol.Add(redPatrolMonsterSpawnPos2, true);
        isEmptyPatrol.Add(redPatrolMonsterSpawnPos3, true);
        isEmptyPatrol.Add(redPatrolMonsterSpawnPos4, true);
        isEmptyPatrol.Add(bluePatrolMonsterSpawnPos1, true);
        isEmptyPatrol.Add(bluePatrolMonsterSpawnPos2, true);
        isEmptyPatrol.Add(bluePatrolMonsterSpawnPos3, true);
        isEmptyPatrol.Add(bluePatrolMonsterSpawnPos4, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            try
            {
                foreach (var item in isEmptyPatrol)
                {
                    if (item.Value == true)
                    {
                        StartCoroutine(SpawnMonster(patrolMonsterGroup, 20f, item.Key));
                    }
                }
            }
            catch (System.Exception)
            {

            }
        }
    }

    IEnumerator SpawnMonster(GameObject targetGroup, float spawnTime, Transform pos)
    {
        isEmptyPatrol[pos] = false;
        yield return spawnTime;
        PhotonNetwork.InstantiateRoomObject(targetGroup.name, pos.position, Quaternion.identity);
        
    }

    

    public void GroupWasKilled(Transform pos)
    {
        isEmptyPatrol[pos] = true;
    }
}




