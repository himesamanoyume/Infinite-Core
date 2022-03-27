using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipBase : MonoBehaviour
{
    /// <summary>
    /// װ��ID
    /// </summary>
    public EquipID equipID;

    /// <summary>
    /// װ������
    /// </summary>
    public EquipType equipType;

    /// <summary>
    /// ������
    /// </summary>
    public MainEntry mainEntry;

    /// <summary>
    /// װ��Ʒ��
    /// </summary>
    public EquipQuality equipQuality;

    /// <summary>
    /// ��һ��������
    /// </summary>
    public EntryLevelUp firstEntry;

    /// <summary>
    /// �ڶ���������
    /// </summary>
    public EntryLevelUp secondEntry;

    /// <summary>
    /// ������������
    /// </summary>
    public EntryLevelUp thirdEntry;

    /// <summary>
    /// ��װ����
    /// </summary>
    public EquipSuit equipSuit;
}
