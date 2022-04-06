using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ProEnum
{
    [Tooltip("近战")]
    Soilder,
    [Tooltip("远程")]
    Archer,
    [Tooltip("辅助")]
    Doctor,
    [Tooltip("坦克")]
    Tanker
}

public enum BuffEnum
{
    [Tooltip("正常")]
    Default,
    [Tooltip("定身")]
    FixedBody,
    [Tooltip("迟缓")]
    Slow,
    [Tooltip("流血")]
    Bleeding,
    [Tooltip("沉默")]
    Silent,
    [Tooltip("系统Buff 侵蚀")]
    Erode,
    [Tooltip("无核 拥有该buff时无法复活")]
    Coreless
}

public enum StateEnum
{
    [Tooltip("尚未生成")]
    NonLife,
    [Tooltip("存活")]
    Alive,
    [Tooltip("彻底死亡")]
    Dead,
    [Tooltip("复活中")]
    Respawning

}

public enum PlayerEnum
{
    RedPlayer1,
    RedPlayer2,
    RedPlayer3,
    RedPlayer4,
    RedPlayer5,
    BluePlayer1,
    BluePlayer2,
    BluePlayer3,
    BluePlayer4,
    BluePlayer5
}

public enum TeamEnum
{
    Null,
    Red,
    Blue
}


