using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipBase : MonoBehaviour
{
    /// <summary>
    /// װ��ID
    /// </summary>
    public EquipEnum.EquipID equipID;

    /// <summary>
    /// װ������
    /// </summary>
    public EquipEnum.EquipType equipType;

    /// <summary>
    /// ������
    /// </summary>
    public EquipEnum.MainEntry mainEntry;

    /// <summary>
    /// װ��Ʒ��
    /// </summary>
    public EquipEnum.EquipQuality equipQuality;

    /// <summary>
    /// ��һ��������
    /// </summary>
    public SkillEnum.EntryLevelUp firstEntry;

    /// <summary>
    /// �ڶ���������
    /// </summary>
    public SkillEnum.EntryLevelUp secondEntry;

    /// <summary>
    /// ������������
    /// </summary>
    public SkillEnum.EntryLevelUp thirdEntry;

    /// <summary>
    /// ��װ����
    /// </summary>
    public EquipEnum.EquipSuit equipSuit;
}
