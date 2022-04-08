using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventGroup : GameEventBase
{

    protected List<GameEventBase> events;
    
    public GameEventGroup(EventEnum eventName)
    {
        Name = eventName;
    }

    /// <summary>
    /// ����¼�
    /// </summary>
    /// <param name="gameEvent"></param>
    public void AddEvent(GameEventBase gameEvent)
    {
        if (events == null)
        {
            events = new List<GameEventBase>();
        }

        //�������
        if (events.Find(e=>e.Name == gameEvent.Name)!=null)
        {
            Debug.LogWarning("�¼�������!");
            return;
        }
        events.Add(gameEvent);
    }

    /// <summary>
    /// ��ȡ�¼�����
    /// </summary>
    /// <param name="eventName"></param>
    /// <returns></returns>
    public GameEventBase GetEvent(EventEnum eventName)
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
    /// ���������¼�
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
                //������¼��� �������¼����������¼�
                (eventItem as GameEventGroup).EnableAllEvents(enable);
                //Debug.Log(eventItem.Name + " " + enable);
            }
            else
            {
                //����ǵ����¼� ���������¼�
                eventItem.Enable = enable;
                //Debug.Log(eventItem.Name + " " + enable);
            }
        }
    }

    /// <summary>
    /// �Ƴ��¼�
    /// </summary>
    /// <param name="eventName"></param>
    public void RemoveEvent(EventEnum eventName)
    {
        if (events == null) return;

        foreach (GameEventBase eventItem in events)
        {
            //����ò�����ΪeventName���¼� ���Ƴ�
            if (eventItem.Name == eventName)
            {
                events.Remove(eventItem);
                return;
            }
            else
            {
                //��������¼� ����Ϊ�¼������Ѱ��
                if (eventItem is GameEventGroup)
                {
                    (eventItem as GameEventGroup).RemoveEvent(eventName);
                }
            }
        }
    }

    /// <summary>
    /// ���¹���������¼�
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
