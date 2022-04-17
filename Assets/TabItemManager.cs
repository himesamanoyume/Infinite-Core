using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class TabItemManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Text playerNameText;
    [SerializeField]
    Text lvText;
    [SerializeField]
    Text killText;
    [SerializeField]
    Text deathText;

    int m_actorNumber;
    CharBase m_charBase;
    CharManager charManager;
    bool isInit = false;
    // Start is called before the first frame update
    void Start()
    {
        charManager = GameObject.Find("CharManager").GetComponent<CharManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInit)
        {
            lvText.text = m_charBase.Level.ToString();
            killText.text = m_charBase.Kill.ToString();
            deathText.text = m_charBase.Death.ToString();
        }
    }

    public void InitTabItem(int actorNumber, CharBase charBase)
    {
        m_actorNumber = actorNumber;
        m_charBase = charBase;
        playerNameText.text = m_charBase.PlayerName;
        lvText.text = m_charBase.Level.ToString();
        killText.text = m_charBase.Kill.ToString();
        deathText.text = m_charBase.Death.ToString();
        isInit = true;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer.ActorNumber == m_actorNumber)
        {
            Destroy(gameObject);
        }
    }
}
