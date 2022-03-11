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
        //����
        if (charBase.Exp>=charBase.MaxExp)
        {
            CharManager.instance.PlayerLevelUp(charBase.RunId,1);
        }

        //����
        if (charBase.State == CharEnum.StateEnum.���)
        {
            if (charBase.Health <= 0)
            {
                CharManager.instance.PlayerKilled(charBase.RunId);
            }
        }
        

        if (charBase.State == CharEnum.StateEnum.������)
        {
            charBase.RespawnCountDown -= Time.deltaTime;
            if (charBase.RespawnCountDown<0)
            {
                charBase.RespawnCountDown = 0;
                charBase.State = CharEnum.StateEnum.���;
                charBase.Health = charBase.MaxHealth;
                CharSpawnController.instance.SpawnPlayer(charBase);
            }
        }

        if (charBase.State == CharEnum.StateEnum.���)
        {
            if (charBase.Health > 0 && charBase.Health < charBase.MaxHealth)
            {
                charBase.Health += charBase.Restore * Time.deltaTime;
            }
        }
    }
}
