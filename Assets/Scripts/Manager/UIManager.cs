using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class UIManager : MonoBehaviourPunCallbacks
{
    #region Props

    [Header("Esc选单")]
    public GameObject exitMenu; 
    public bool isExitMenuActive = false;
    
    [Header("底部信息栏")]
    public GameObject myselfInfoMenu;
    public Slider healthSlider;
    public Slider shieldSlider;
    public Slider expSlider;
    public Image skillQCountDown;
    public Image skillECountDown;
    public Image skillRCountDown;
    public Image skillBrustCountDown;
    public Text countDownText;


    [Header("重要信息通知")]
    public Image importantInfoPanel;
    public Image importantInfo;


    [Header("Tab积分板")]
    public GameObject tabMenu;
    public Transform tabRedContent;
    public Transform tabBlueContent;
    public GameObject playerTabItemPrefab;

    [SerializeField]
    List<string> iptInfoList = new List<string>();
    bool isNext = true;
    bool isShow = false;

    CharBase charBase = null;
    float m_maxShield;
    public Text healthText;

    Dictionary<int, GameObject> tabPlayerList = new Dictionary<int, GameObject>();
    CharManager charManager;

    Vector2 defaultSizeDelta;
    Color importantInfoDefaultColor;
    Color infoTextDefaultColor;

    #endregion

    #region Unity

    private void Start()
    {
        defaultSizeDelta = importantInfo.GetComponent<RectTransform>().sizeDelta;
        importantInfoDefaultColor = importantInfo.color;

        infoText = importantInfoPanel.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();

        infoTextDefaultColor = infoText.color;
        charManager = GameObject.Find("CharManager").GetComponent<CharManager>();

        #region Subscribe Event

        GameEventManager.SubscribeEvent(EventEnum.OnPlayerRespawning, OnPlayerRespawning);
        GameEventManager.SubscribeEvent(EventEnum.OnPlayerRespawnCountDownEnd, OnPlayerRespawnCountDownEnd);

        #endregion


    }

    private void Update()
    {
        
        if (charManager != null)
        {
            if (charBase!=null)
            {
                
            }
            else
            {
                charManager.recorders.TryGetValue(PhotonNetwork.LocalPlayer.ActorNumber, out GameObject recorder);

                try
                {
                    charBase = recorder.GetComponent<CharBase>();
                }
                catch (System.Exception)
                {

                   
                }
            }

            
            foreach (int key in charManager.recorders.Keys)
            {
                //Debug.LogWarning("Item1");
                if (!tabPlayerList.TryGetValue(key, out GameObject tabPlayerItem))
                {
                    //Debug.LogWarning("Item2");
                    CharBase tempCharBase = charManager.recorders[key].GetComponent<CharBase>();
                    GameObject tabItem;
                    switch (tempCharBase.PlayerTeam)
                    {
                        case TeamEnum.Red:
                            tabItem = Instantiate(playerTabItemPrefab, tabRedContent);
                            tabItem.GetComponent<TabItemManager>().InitTabItem(tempCharBase.ActorNumber, tempCharBase);
                            tabPlayerList.Add(tempCharBase.ActorNumber, tabItem);
                            //Debug.LogWarning("Red Item");
                            break;
                        case TeamEnum.Blue:
                            tabItem = Instantiate(playerTabItemPrefab, tabBlueContent);
                            tabItem.GetComponent<TabItemManager>().InitTabItem(tempCharBase.ActorNumber, tempCharBase);
                            tabPlayerList.Add(tempCharBase.ActorNumber, tabItem);
                            //Debug.LogWarning("Blue Item");
                            break;
                    }
                    
                }

            }
        }

        PostImportantInfo();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isExitMenuActive = !isExitMenuActive;
            activeMenu = SetActiveExitMenu;
            SetActiveMenu(exitMenu.name, activeMenu);
        }

        if (Input.GetKey(KeyCode.Tab) && !isExitMenuActive)
        {
            tabMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1030);
            //tabMenu.SetActive(true);
        }
        else
        {
            tabMenu.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 900);
            //tabMenu.SetActive(false);
        }

        

        if (charBase)
        {

            healthSlider.maxValue = charBase.MaxHealth;
            healthSlider.value = charBase.CurrentHealth;
            if (charBase.Shield > m_maxShield)
            {
                m_maxShield = charBase.Shield;
            }
            shieldSlider.value = charBase.Shield;
            if (shieldSlider.value == 0)
            {
                m_maxShield = 0;
            }
            healthText.text = ((int)charBase.CurrentHealth).ToString() + "/" + ((int)charBase.MaxHealth).ToString();

            expSlider.maxValue = charBase.MaxExp;
            expSlider.value = charBase.CurrentExp;
            
        }

        if (isShow)
        {
            importantInfo.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(importantInfo.GetComponent<RectTransform>().sizeDelta, new Vector2(1600, importantInfo.GetComponent<RectTransform>().sizeDelta.y), 0.2f);

            importantInfo.color = Color.Lerp(importantInfo.color, new Color(1, 1, 1, 0.6f), 0.1f);

            infoText.color = Color.Lerp(infoText.color, new Color(0.21f, 0.21f, 0.21f, 1), 0.1f);
        }
    }

    #endregion

    #region Unity Function

    delegate void IsActiveMenu();
    IsActiveMenu activeMenu;

    void SetActiveExitMenu()
    {
        exitMenu.SetActive(isExitMenuActive);
        if (!isExitMenuActive)
        {
            GameEventManager.EnableEvent(EventEnum.PlayerControlGroup, true);
        }
    }

    /// <summary>
    /// 激活菜单
    /// </summary>
    /// <param name="menuName"></param>
    private void SetActiveMenu(string menuName, IsActiveMenu isActiveMenu)
    {
        GameEventManager.EnableEvent(EventEnum.PlayerControlGroup, false);
        exitMenu.SetActive(menuName.Equals(exitMenu.name));
        isActiveMenu();
    }

    public void GetImportantInfo(string infoText)
    {
        iptInfoList.Add(infoText);
    }

    //信息通知
    void PostImportantInfo()
    {
        if (iptInfoList.Count != 0)
        {
            if (isNext)
            {
                isNext = false;
                StartCoroutine("ShowImportantInfo", iptInfoList[0]);
            }
        }
    }

    Text infoText;
    IEnumerator ShowImportantInfo(string text)
    {
        //Debug.LogWarning("Show");
        importantInfo.gameObject.SetActive(true);
        importantInfo.color = importantInfoDefaultColor;
        importantInfo.GetComponent<RectTransform>().sizeDelta = defaultSizeDelta;
        infoText.color = infoTextDefaultColor;
        isShow = true;
        
        infoText.text = text;

        yield return new WaitForSeconds(5);

        importantInfo.gameObject.SetActive(false);
        iptInfoList.Remove(iptInfoList[0]);
        yield return new WaitForSeconds(2);
        isNext = true;
        isShow = false;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (tabPlayerList.TryGetValue(otherPlayer.ActorNumber,out GameObject item))
        {
            tabPlayerList.Remove(otherPlayer.ActorNumber);
        }
    }
    #endregion

    #region Event Response

    public void OnPlayerRespawning(object[] args)
    {
        int actorNumber;
        float countDown;
        if (args.Length == 2)
        {
            actorNumber = (int)args[0];
            countDown = (float)args[1];

            countDownText.text = "复活倒计时: " + ((int)countDown).ToString();
            
        }
    }

    public void OnPlayerRespawnCountDownEnd(object[] args)
    {
        int actorNumber;
        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            countDownText.text = "";

        }
    }

    #endregion
}
