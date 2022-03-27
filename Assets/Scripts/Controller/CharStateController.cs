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
            CharManager.Instance.PlayerLevelUp(charBase.RunId,1);
        }

        //����
        if (charBase.State == StateEnum.���)
        {
            if (charBase.CurrentHealth <= 0)
            {
                CharManager.Instance.PlayerKilled(charBase.RunId);
            }
        }
        
        //����
        if (charBase.State == StateEnum.������)
        {
            charBase.RespawnCountDown -= Time.deltaTime;
            if (charBase.RespawnCountDown<0)
            {
                charBase.RespawnCountDown = 0;
                charBase.State = StateEnum.���;
                charBase.CurrentHealth = charBase.MaxHealth;
                CharSpawnController.instance.SpawnPlayer(charBase);
            }
        }

        
        if (charBase.State == StateEnum.���)
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
