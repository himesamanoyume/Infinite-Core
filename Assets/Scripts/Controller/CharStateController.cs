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
        if (charBase.CurrentExp>=charBase.MaxExp)
        {
            CharManager.Instance.PlayerLevelUp(charBase.RunId,1);
        }

        //À¿Õˆ
        if (charBase.State == StateEnum.Alive)
        {
            if (charBase.CurrentHealth <= 0)
            {
                CharManager.Instance.PlayerKilled(charBase.RunId);
            }
        }
        
        //∏¥ªÓ
        if (charBase.State == StateEnum.Respawning)
        {
            charBase.RespawnCountDown -= Time.deltaTime;
            if (charBase.RespawnCountDown<0)
            {
                charBase.RespawnCountDown = 0;
                charBase.State = StateEnum.Alive;
                charBase.CurrentHealth = charBase.MaxHealth;
                CharSpawnController.instance.SpawnPlayer(charBase);
            }
        }

        
        if (charBase.State == StateEnum.Alive)
        {
            //ªÿ—™
            if (charBase.CurrentHealth > 0 && charBase.CurrentHealth < charBase.MaxHealth)
            {
                charBase.CurrentHealth += charBase.Restore * Time.deltaTime;
            }
            //ºÏ≤‚—™¡ø
            if (charBase.CurrentHealth > charBase.MaxHealth)
            {
                charBase.CurrentHealth = charBase.MaxHealth;
            }
        }
    }
}
