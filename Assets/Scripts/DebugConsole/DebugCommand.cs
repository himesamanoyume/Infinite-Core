using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugCommand : DebugCommandBase
{
    private Action command;

    public DebugCommand(string id, string description, string format, Action command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}

public class DebugCommand<T1> : DebugCommandBase
{
    private Action<T1> command;

    public DebugCommand(string id, string description, string format, Action<T1> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value)
    {
        command.Invoke(value);
    }
}

public class DebugCommand<T1, T2> : DebugCommandBase
{
    private Action<T1, T2> command;

    public DebugCommand(string id, string description, string format, Action<T1, T2> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value1, T2 value2)
    {
        command.Invoke(value1, value2);
    }
}

public class DebugCommand<T1, T2, T3> : DebugCommandBase
{
    private Action<T1, T2, T3> command;

    public DebugCommand(string id, string description, string format, Action<T1, T2, T3> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value1, T2 value2, T3 value3)
    {
        command.Invoke(value1, value2, value3);
    }
}
