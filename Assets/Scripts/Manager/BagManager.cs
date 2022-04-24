using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class BagManager : MonoBehaviour
{
    [Header("Bag背包面板")]
    public Transform bagMenu;

    [Header("Bag属性")]
    public Text playerName;
    public Text pro;
    public Text attack;
    public Text criticalHitRate;
    public Text criticalHit;
    public Text defence;
    public Text attackSpeed;
    public Text moveSpeed;

    [Header("Bag仓库")]
    [Header("已穿着")]
    public Transform wearingHeadContent;
    public Transform wearingArmorContent;
    public Transform wearingHandContent;
    public Transform wearingTrousersContent;
    public Transform wearingKneeContent;
    public Transform wearingBootsContent;

    [Header("仓库位")]
    public Transform HeadContent;
    public Transform ArmorContent;
    public Transform HandContent;
    public Transform TrousersContent;
    public Transform KneeContent;
    public Transform BootsContent;

    bool isOpenBag = false;
    int m_actorNumber;
    GameObject m_recorder;
    CharBase m_charBase;

    private void Start()
    {
        StartCoroutine(InitPropsText());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpenBag = !isOpenBag;
            if (isOpenBag)
            {
                GameEventManager.EnableEvent(EventEnum.PlayerControlGroup, false);
                bagMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(101, 132.596f);
            }
            else
            {
                GameEventManager.EnableEvent(EventEnum.PlayerControlGroup, true);
                bagMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(-3744, 132.596f);
            }
        }

        if (m_recorder && m_charBase)
        {
            criticalHitRate.text = (m_charBase.CriticalHitRate *100).ToString("f1") + "%";
            criticalHit.text = (m_charBase.CriticalHit * 100).ToString("f1") + "%";
            defence.text = m_charBase.Defence.ToString();
            attackSpeed.text = (m_charBase.AttackSpeed * 100).ToString("f1") + "%";
            moveSpeed.text = m_charBase.MoveSpeed.ToString();
        }
    }

    IEnumerator InitPropsText()
    {
        yield return new WaitForSeconds(1);
        m_actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        GameObject.Find("CharManager").GetComponent<CharManager>().FindPlayerRecorder(m_actorNumber, out GameObject playerRecorder, out CharBase charBase);
        m_recorder = playerRecorder;
        m_charBase = charBase;

        playerName.text = m_charBase.PlayerName;
        pro.text = m_charBase.Pro.ToString();

    }
}
