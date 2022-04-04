using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager
{
    /// <summary>
    /// Root组 管理全部事件
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

        //于此处控制事件组和事件的初始状态
        RootGroup.Enable = true;

    }

    private static void Init()
    {
        RootGroup = new GameEventGroup("Root");

        //于此处添加事件组和事件


    }

    /// <summary>
    /// 注册事件
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
    /// 订阅事件
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
    /// 启动或禁用事件
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
