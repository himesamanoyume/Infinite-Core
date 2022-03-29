using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class LauncherSelect : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Dropdown proSelect;
    [SerializeField]
    Dropdown skillQSelect;
    [SerializeField]
    Dropdown skillESelect;
    [SerializeField]
    Dropdown skillRSelect;
    [SerializeField]
    Dropdown skillBrustSelect;

    #region Unity Callbacks
        
    public void OnProSelectChanged()
    {
        Debug.Log((ProEnum)proSelect.value);
    }

    #endregion
}
