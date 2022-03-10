using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStateController : MonoBehaviour
{
    CharBase charBase;
    private void Start()
    {
        charBase = gameObject.GetComponent<CharBase>();

    }

    private void FixedUpdate()
    {
        //…˝º∂
        if (charBase.Exp>=charBase.MaxExp)
        {
            CharManager.instance.PlayerLevelUp(charBase.RunId,1);
        }

        //À¿Õˆ
        if (charBase.Health<=0)
        {
            CharManager.instance.PlayerKilled(charBase.RunId);
        }

        if (charBase.IsRespawn)
        {
            charBase.RespawnCountDown -= Time.deltaTime;
            if (charBase.RespawnCountDown<=0)
            {
                //CharManager.instance.PlayerSpawn(charBase.RunId);
            }
        }
    }
}
