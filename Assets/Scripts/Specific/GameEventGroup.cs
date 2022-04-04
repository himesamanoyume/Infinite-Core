using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventGroup : GameEventBase
{

    protected List<GameEventBase> events;
    
    public GameEventGroup(string eventName)
    {
        Name = eventName;
    }

    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="gameEvent"></param>
    public void AddEvent(GameEventBase gameEvent)
    {
        if (events == null)
        {
            events = new List<GameEventBase>();
        }

        //检查重名
        if (events.Find(e=>e.Name == gameEvent.Name)!=null)
        {
            return;
        }
        events.Add(gameEvent);
    }

    /// <summary>
    /// 获取事件对象
    /// </summary>
    /// <param name="eventName"></param>
    /// <returns></returns>
    public GameEventBase GetEvent(string eventName)
    {
        Queue<GameEventBase> queue = new Queue<GameEventBase>();

        queue.Enqueue(this);
        while (queue.Count>0)
        {
            GameEventGroup temp = queue.Dequeue() as GameEventGroup;
            if (temp != null && temp.events != null && temp.events.Count > 0)
            {
                var children = temp.events;
                foreach (GameEventBase eventItem in children)
                {
                    if (eventItem.Name == eventName)
                    {
                        return eventItem;
                    }
                    queue.Enqueue(eventItem);
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 启用所有事件
    /// </summary>
    /// <param name="enable"></param>
    public void EnableAllEvents(bool enable)
    {
        Enable = enable;
        if (events == null) return;

        foreach (GameEventBase eventItem in events)
        {
            if (eventItem is GameEventGroup)
            {
                //如果是事件组 则启用事件组内所有事件
                (eventItem as GameEventGroup).EnableAllEvents(enable);
            }
            else
            {
                //如果是单个事件 则启动其事件
                eventItem.Enable = enable;
            }
        }
    }

    /// <summary>
    /// 移除事件
    /// </summary>
    /// <param name="eventName"></param>
    public void RemoveEvent(string eventName)
    {
        if (events == null) return;

        foreach (GameEventBase eventItem in events)
        {
            //如果该层有名为eventName的事件 则移除
            if (eventItem.Name == eventName)
            {
                events.Remove(eventItem);
                return;
            }
            else
            {
                //如果不是事件 则作为事件组从中寻找
                if (eventItem is GameEventGroup)
                {
                    (eventItem as GameEventGroup).RemoveEvent(eventName);
                }
            }
        }
    }

    /// <summary>
    /// 更新管理的所有事件
    /// </summary>
    public override void Update()
    {
        if (!Enable || events == null)
        {
            return;
        }

        foreach (GameEventBase eventItem in events)
        {
            if (eventItem != null) eventItem.Update();
        }
    }
}
