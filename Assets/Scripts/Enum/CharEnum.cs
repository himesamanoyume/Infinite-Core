using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ProEnum
{
    [Tooltip("��ս")]
    Soilder,
    [Tooltip("Զ��")]
    Archer,
    [Tooltip("����")]
    Doctor,
    [Tooltip("̹��")]
    Tanker
}

public enum BuffEnum
{
    [Tooltip("����")]
    Default,
    [Tooltip("����")]
    FixedBody,
    [Tooltip("�ٻ�")]
    Slow,
    [Tooltip("��Ѫ")]
    Bleeding,
    [Tooltip("��Ĭ")]
    Silent,
    [Tooltip("ϵͳBuff ��ʴ")]
    Erode,
    [Tooltip("�޺� ӵ�и�buffʱ�޷�����")]
    Coreless
}

public enum StateEnum
{
    [Tooltip("��δ����")]
    NonLife,
    [Tooltip("���")]
    Alive,
    [Tooltip("��������")]
    Dead,
    [Tooltip("������")]
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


