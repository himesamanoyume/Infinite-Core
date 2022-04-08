using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventBase 
{
    #region Event Name

    private EventEnum eventName;

    public EventEnum Name
    {
        get { return eventName; }
        protected set { eventName = value; }
    }

    #endregion

    #region Event state

    private bool enable;

    public bool Enable
    {
        get { return enable; }
        set { enable = value; }
    }
    #endregion

    public virtual void Update() { }
}
