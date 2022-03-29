using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ProEnum
{
    Soilder,//近战 Soilder
    Archer,//远程 Archer
    Doctor,//辅助 Doctor
    Tanker//坦克 Tanker
}

public enum BuffEnum
{
    FixedBody,//定身
    Slow,//迟缓
    Bleeding,//流血
    Silent,//沉默
    Erode,//系统Buff 侵蚀
    Coreless//无核 拥有该buff时无法复活
}

public enum StateEnum
{
    NonLife,//尚未生成
    Alive,//存活
    Dead,//彻底死亡
    Respawning//复活中

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


