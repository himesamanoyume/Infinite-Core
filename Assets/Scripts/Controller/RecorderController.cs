using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class RecorderController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetParents(GameObject total)
    {
        this.transform.SetParent(total.transform);
    }
}
