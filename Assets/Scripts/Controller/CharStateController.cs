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

        TotalManager.instance.recorders.TryGetValue(photonView.OwnerActorNr, out GameObject recorder);
        try
        {
            charBase = recorder.GetComponent<CharBase>();
        }
        catch (System.Exception)
        {
            //Debug.LogError("找不到");
            
        }
        

    }

    //private void FixedUpdate()
    //{
    //    //升级
    //    if (charBase.CurrentExp>=charBase.MaxExp)
    //    {
    //        CharManager.Instance.PlayerLevelUp(charBase.ActorNumber,1);
    //    }

    //    //死亡
    //    if (charBase.State == StateEnum.Alive)
    //    {
    //        if (charBase.CurrentHealth <= 0)
    //        {
    //            CharManager.Instance.OnPlayerKilled(charBase.ActorNumber);
    //        }
    //    }
        
    //    //复活
    //    if (charBase.State == StateEnum.Respawning)
    //    {
    //        charBase.RespawnCountDown -= Time.deltaTime;

    //        if (charBase.RespawnCountDown<0)
    //        {
    //            charBase.RespawnCountDown = 0;
    //            charBase.State = StateEnum.Alive;
    //            charBase.CurrentHealth = charBase.MaxHealth;

    //            CharSpawnController.instance.OnRespawnPlayer(charBase.ActorNumber);
    //        }
    //    }

        
    //    if (charBase.State == StateEnum.Alive)
    //    {
    //        //回血
    //        if (charBase.CurrentHealth > 0 && charBase.CurrentHealth < charBase.MaxHealth)
    //        {
    //            charBase.CurrentHealth += charBase.Restore * Time.deltaTime;
    //        }
    //        //检测血量
    //        if (charBase.CurrentHealth > charBase.MaxHealth)
    //        {
    //            charBase.CurrentHealth = charBase.MaxHealth;
    //        }
    //    }
    //}
}
