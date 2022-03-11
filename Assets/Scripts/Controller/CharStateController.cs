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
        //升级
        if (charBase.Exp>=charBase.MaxExp)
        {
            CharManager.instance.PlayerLevelUp(charBase.RunId,1);
        }

        //死亡
        if (charBase.State == CharEnum.StateEnum.存活)
        {
            if (charBase.Health <= 0)
            {
                CharManager.instance.PlayerKilled(charBase.RunId);
            }
        }
        

        if (charBase.State == CharEnum.StateEnum.复活中)
        {
            charBase.RespawnCountDown -= Time.deltaTime;
            if (charBase.RespawnCountDown<0)
            {
                charBase.RespawnCountDown = 0;
                charBase.State = CharEnum.StateEnum.存活;
                charBase.Health = charBase.MaxHealth;
                CharSpawnController.instance.SpawnPlayer(charBase);
            }
        }

        if (charBase.State == CharEnum.StateEnum.存活)
        {
            if (charBase.Health > 0 && charBase.Health < charBase.MaxHealth)
            {
                charBase.Health += charBase.Restore * Time.deltaTime;
            }
        }
    }
}
