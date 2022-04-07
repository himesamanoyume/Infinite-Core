using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class CharStateController : MonoBehaviour
{
    CharBase charBase = null;

    private void Start()
    {
        FindCharBase();

        
        //GameEventManager.EnableEvent(GameEventManager.EVENT_ON_PLAYER_LEVEL_UP, false);

    }

    private void Update()
    {
        if (charBase == null)
        {
            FindCharBase();
        }
        else
        {

        }
    }

    void FindCharBase()
    {

        PhotonView photonView = this.gameObject.GetPhotonView();

        CharManager.Instance.recorders.TryGetValue(photonView.OwnerActorNr, out GameObject recorder);
        try
        {
            charBase = recorder.GetComponent<CharBase>();
        }
        catch (System.Exception)
        {
            Debug.LogError("�Ҳ���");
            
        }
        

    }

    private void FixedUpdate()
    {
        //����ToGivePlayerExp 1 1000 1200
        if (charBase.CurrentExp >= charBase.MaxExp && charBase.CurrentExp!=0)
        {

        }

        //����
        if (charBase.State == StateEnum.Alive)
        {
            if (charBase.CurrentHealth <= 0)
            {

                //CharManager.Instance.OnPlayerKilled(charBase.ActorNumber);
            }
        }

        //����
        if (charBase.State == StateEnum.Respawning)
        {
            
            charBase.RespawnCountDown -= Time.deltaTime;

            if (charBase.RespawnCountDown < 0)
            {
                

                charBase.RespawnCountDown = 0;
                charBase.State = StateEnum.Alive;
                charBase.CurrentHealth = charBase.MaxHealth;

                //CharSpawnController.instance.OnRespawnPlayer(charBase.ActorNumber);
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

    bool OnPlayerLevelUpCheck(out object[] args)
    {
        args = new object[2] { charBase.ActorNumber, 1 };
        return true;
    }


}
