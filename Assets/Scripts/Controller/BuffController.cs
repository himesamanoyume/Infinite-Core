using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    private CharBase charBase;

    // Start is called before the first frame update
    void Start()
    {
        charBase = GetComponent<CharBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBuff(BuffEnum buffEnum, float time)
    {
        StartCoroutine(BuffCountDown(buffEnum, time));
    }

    IEnumerator BuffCountDown(BuffEnum buffEnum, float time)
    {
        yield return new WaitForSeconds(time);
        Debug.LogWarning(buffEnum + "½áÊø");
        charBase.Buff.Remove((int)buffEnum);
    }
}
