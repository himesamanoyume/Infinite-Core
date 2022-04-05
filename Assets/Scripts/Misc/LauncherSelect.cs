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
    Dropdown skillBurstSelect;

    #region Callbacks

    /// <summary>
    /// ְҵѡ��ص�
    /// </summary>
    public void OnProSelectChanged()
    {
        Hashtable props = new Hashtable()
        {
            {InfiniteCoreGame.PLAYER_PRO,(ProEnum)proSelect.value }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        Debug.Log((ProEnum)proSelect.value + " Changed");
    }
    /// <summary>
    /// Q����ѡ��ص�
    /// </summary>
    public void OnSkillQSelectChanged()
    {
        Hashtable props = new Hashtable()
        {
            {InfiniteCoreGame.PLAYER_SKILL_Q,(SkillQ)skillQSelect.value }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        Debug.Log((SkillQ)skillQSelect.value + " Changed");
    }
    /// <summary>
    /// E����ѡ��ص�
    /// </summary>
    public void OnSkillESelectChanged()
    {
        Hashtable props = new Hashtable()
        {
            {InfiniteCoreGame.PLAYER_SKILL_E,(SkillE)skillQSelect.value }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        Debug.Log((SkillE)skillQSelect.value + " Changed");
    }
    /// <summary>
    /// R����ѡ��ص�
    /// </summary>
    public void OnSkillRSelectChanged()
    {
        Hashtable props = new Hashtable()
        {
            {InfiniteCoreGame.PLAYER_SKILL_R,(SkillR)skillRSelect.value }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        Debug.Log((SkillR)skillRSelect.value + " Changed");
    }
    /// <summary>
    /// Burst����ѡ��ص�
    /// </summary>
    public void OnSkillBurstSelectChanged()
    {
        Hashtable props = new Hashtable()
        {
            {InfiniteCoreGame.PLAYER_SKILL_BURST,(SkillBurst)skillBurstSelect.value }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        Debug.Log((SkillBurst)skillBurstSelect.value + " Changed");
    }
    #endregion
}
