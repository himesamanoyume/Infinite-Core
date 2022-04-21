using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGroup : MonoBehaviour
{

    private void Start()
    {

    }

    private void Update()
    {
        if (transform.GetChild(0) == null)
        {
            GameObject.Find("MonsterManager").GetComponent<MonsterManager>().GroupWasKilled(gameObject.transform);
        }
    }

    public void InitPos(Transform pos)
    {
        gameObject.transform.position = pos.position;
    }
}