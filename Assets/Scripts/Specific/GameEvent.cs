using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : GameEventBase
{
    public delegate void ResponseHandle(params object[] args);
    protected ResponseHandle response;

    public delegate bool CheckHandle(out object[] args);
    protected CheckHandle checkHandle;

    protected object[] args;

    public GameEvent(string eventName)
    {
        Name = eventName;
    }

    public override void Update()
    {
        if (!Enable)
        {
            return;
        }

        if (checkHandle!=null && checkHandle(out args))
        {
            if (response != null) response(args);
        }
    }

    public void AddCheckHandle(CheckHandle check)
    {
        checkHandle += check;
    }

    public void RemoveCheckHandle(CheckHandle check)
    {
        checkHandle -= check;
    }

    public void AddResponse(ResponseHandle response)
    {
        this.response += response;
    }

    public void RemoveResponse(ResponseHandle response)
    {
        this.response -= response;
    }
}
