using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager
{
    /// <summary>
    /// Root�� ����ȫ���¼�
    /// </summary>
    private static GameEventGroup rootGroup;

    public static GameEventGroup RootGroup
    {
        get { return rootGroup; }
        private set { rootGroup = value; }
    }


    static GameEventManager()
    {
        Init();

        //�ڴ˴������¼�����¼��ĳ�ʼ״̬
        RootGroup.Enable = true;

    }

    private static void Init()
    {
        RootGroup = new GameEventGroup("Root");

        //�ڴ˴�����¼�����¼�


    }

    /// <summary>
    /// ע���¼�
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="check"></param>
    public static void RegisterEvent(string eventName,GameEvent.CheckHandle check)
    {
        var target = RootGroup?.GetEvent(eventName);
        if (target != null && target is GameEvent temp)
        {
            temp.AddCheckHandle(check);
        }
    }

    /// <summary>
    /// �����¼�
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="response"></param>
    public static void SubscribeEvent(string eventName,GameEvent.ResponseHandle response)
    {
        var target = RootGroup.GetEvent(eventName);
        if (target != null && target is GameEvent temp)
        {
            temp.AddResponse(response);
        }
    }

    /// <summary>
    /// ����������¼�
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="enable"></param>
    public static void EnableEvent(string eventName,bool enable)
    {
        var target = RootGroup.GetEvent(eventName);
        if (target != null)
        {
            target.Enable = enable;
        }
    }

    public static void EnableAllEvents(string eventName, bool enable)
    {
        var target = RootGroup?.GetEvent(eventName) as GameEventGroup;
        if (target != null)
        {
            target.EnableAllEvents(enable);
        }
    }

    public static void Update()
    {
        if (RootGroup== null) return;
        
        RootGroup.Update();
    }
}
