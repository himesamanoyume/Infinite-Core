using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChildController : MonoBehaviour
{
    private Paramater paramater;
    // Start is called before the first frame update
    void Start()
    {
        paramater = transform.parent.GetComponent<MonsterController>().paramater;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerModel"))
        {
            paramater.currentTarget = other.gameObject.transform;
            paramater.isChaseTarget = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerModel"))
        {
            paramater.isChaseTarget = false;
            paramater.currentTarget = null;
        }
    }
}
