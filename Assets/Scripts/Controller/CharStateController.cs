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
        if (charBase.CurrentExp>=charBase.MaxExp)
        {
            CharManager.Instance.PlayerLevelUp(charBase.RunId,1);
        }

        //死亡
        if (charBase.State == StateEnum.存活)
        {
            if (charBase.CurrentHealth <= 0)
            {
                CharManager.Instance.PlayerKilled(charBase.RunId);
            }
        }
        
        //复活
        if (charBase.State == StateEnum.复活中)
        {
            charBase.RespawnCountDown -= Time.deltaTime;
            if (charBase.RespawnCountDown<0)
            {
                charBase.RespawnCountDown = 0;
                charBase.State = StateEnum.存活;
                charBase.CurrentHealth = charBase.MaxHealth;
                CharSpawnController.instance.SpawnPlayer(charBase);
            }
        }

        
        if (charBase.State == StateEnum.存活)
        {
            //回血
            if (charBase.CurrentHealth > 0 && charBase.CurrentHealth < charBase.MaxHealth)
            {
                charBase.CurrentHealth += charBase.Restore * Time.deltaTime;
            }
            //检测血量
            if (charBase.CurrentHealth > charBase.MaxHealth)
            {
                charBase.CurrentHealth = charBase.MaxHealth;
            }
        }
    }
}
