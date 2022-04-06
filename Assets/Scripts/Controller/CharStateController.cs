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
        if (charBase.CurrentExp>=charBase.MaxExp)
        {
            CharManager.Instance.PlayerLevelUp(charBase.ActorNumber,1);
        }

        //����
        if (charBase.State == StateEnum.Alive)
        {
            if (charBase.CurrentHealth <= 0)
            {
                CharManager.Instance.OnPlayerKilled(charBase.ActorNumber);
            }
        }
        
        //����
        if (charBase.State == StateEnum.Respawning)
        {
            charBase.RespawnCountDown -= Time.deltaTime;

            if (charBase.RespawnCountDown<0)
            {
                charBase.RespawnCountDown = 0;
                charBase.State = StateEnum.Alive;
                charBase.CurrentHealth = charBase.MaxHealth;

                CharSpawnController.instance.OnRespawnPlayer(charBase.ActorNumber);
            }
        }

        
        if (charBase.State == StateEnum.Alive)
        {
            //��Ѫ
            if (charBase.CurrentHealth > 0 && charBase.CurrentHealth < charBase.MaxHealth)
            {
                charBase.CurrentHealth += charBase.Restore * Time.deltaTime;
            }
            //���Ѫ��
            if (charBase.CurrentHealth > charBase.MaxHealth)
            {
                charBase.CurrentHealth = charBase.MaxHealth;
            }
        }
    }
}
