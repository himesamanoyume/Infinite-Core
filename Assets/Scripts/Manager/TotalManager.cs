using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class TotalManager : MonoBehaviour
{
    public static TotalManager instance;

    public Dictionary<int,GameObject> recorders;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        recorders = new Dictionary<int,GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        FindRecorder();
    }

    void FindRecorder()
    {
        if (recorders.Count == PhotonNetwork.PlayerList.Length)
        {
            
            return;
        }
        else
        {
            recorders.Clear();
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("PlayerRecorder");
            foreach (GameObject obj in gameObjects)
            {
                recorders.Add(obj.GetComponent<CharBase>().ActorNumber, obj);
                
            }

            Debug.LogWarning("已添加记录者");
        }

    }
}
