using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class MonsterGroup : MonoBehaviour, IPunObservable
{
    public int m_id = -1;
    public bool isEmpty;
    bool isRespawn = false;
    bool isInit = false;

    public MonsterType m_type;

    public Transform no1Pos;
    public Transform no2Pos;
    public Transform no3Pos;

    public GameObject partolMonsterPrefab;

    private void Start()
    {
        
        
    }

    private void Update()
    {
        if (m_id != -1)
        {
            transform.name = m_type.ToString() + "Group" + m_id;
        }

        if (isInit)
        {
            if (GetComponentsInChildren<Transform>(true).Length <= 4 && isEmpty == false && isRespawn == false)
            {
                isEmpty = true;
            }
            else if(GetComponentsInChildren<Transform>(true).Length <= 4 && isEmpty == true && isRespawn == false)
            {
                isEmpty = false;
                StartCoroutine(SpawnMonster(1));
            }
        }            
    }

    public void InitGroup(MonsterType monsterType, int id)
    {
        m_type = monsterType;
        m_id = id;
        switch (m_type)
        {
            case MonsterType.PatrolMonster:
                StartCoroutine(SpawnMonster(0));
                break;
            case MonsterType.WorldMonster:
                break;
            case MonsterType.InfiniteCore:
                break;
        }
        transform.name = m_type.ToString() + "Group" + id;
    }

    IEnumerator SpawnMonster(int respawnTime)
    {
        isEmpty = false;
        isRespawn = true;
        yield return new WaitForSeconds(respawnTime);
        GameObject g;
        switch (m_type)
        {
            case MonsterType.PatrolMonster:
                g = PhotonNetwork.InstantiateRoomObject(partolMonsterPrefab.name, no1Pos.position, Quaternion.identity);
                g.GetComponent<MonsterController>().InitController(m_id);
                g.transform.SetParent(transform);

                g = PhotonNetwork.InstantiateRoomObject(partolMonsterPrefab.name, no2Pos.position, Quaternion.identity);
                g.GetComponent<MonsterController>().InitController(m_id);
                g.transform.SetParent(transform);

                g = PhotonNetwork.InstantiateRoomObject(partolMonsterPrefab.name, no3Pos.position, Quaternion.identity);
                g.GetComponent<MonsterController>().InitController(m_id);
                g.transform.SetParent(transform);
                break;
            case MonsterType.WorldMonster:

                break;
            case MonsterType.InfiniteCore:

                break;
        }
        isInit = true;
        isRespawn = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(m_type);
            stream.SendNext(m_id);
        }
        else
        {
            m_type = (MonsterType)stream.ReceiveNext();
            m_id = (int)stream.ReceiveNext();
        }
    }
}