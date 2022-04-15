using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    [SerializeField]
    GameObject exitMenu; 
    [SerializeField]
    bool isExitMenuActive = false;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isExitMenuActive = !isExitMenuActive;
            activeMenu = SetActiveExitMenu;
            SetActiveMenu(exitMenu.name, activeMenu);
        }
    }

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
    /// ¼¤»î²Ëµ¥
    /// </summary>
    /// <param name="menuName"></param>
    private void SetActiveMenu(string menuName, IsActiveMenu isActiveMenu)
    {
        GameEventManager.EnableEvent(EventEnum.PlayerControlGroup, false);
        exitMenu.SetActive(menuName.Equals(exitMenu.name));
        isActiveMenu();
    }

}
