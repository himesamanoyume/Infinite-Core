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
            CharManager.Instance.PlayerLevelUp(charBase.RunId,1);
        }

        //����
        if (charBase.State == CharEnum.StateEnum.���)
        {
            if (charBase.Health <= 0)
            {
                CharManager.Instance.PlayerKilled(charBase.RunId);
            }
        }
        
        //����
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
            //��Ѫ
            if (charBase.Health > 0 && charBase.Health < charBase.MaxHealth)
            {
                charBase.Health += charBase.Restore * Time.deltaTime;
            }
            //���Ѫ��
            if (charBase.Health > charBase.MaxHealth)
            {
                charBase.Health = charBase.MaxHealth;
            }
        }
    }
}
