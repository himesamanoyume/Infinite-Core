using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipBase : MonoBehaviour
{
    /// <summary>
    /// 装备ID
    /// </summary>
    public EquipID equipID;

    /// <summary>
    /// 装备类型
    /// </summary>
    public EquipType equipType;

    /// <summary>
    /// 主词条
    /// </summary>
    public MainEntry mainEntry;

    /// <summary>
    /// 装备品质
    /// </summary>
    public EquipQuality equipQuality;

    /// <summary>
    /// 第一个副词条
    /// </summary>
    public EntryLevelUp firstEntry;

    /// <summary>
    /// 第二个副词条
    /// </summary>
    public EntryLevelUp secondEntry;

    /// <summary>
    /// 第三个副词条
    /// </summary>
    public EntryLevelUp thirdEntry;

    /// <summary>
    /// 套装类型
    /// </summary>
    public EquipSuit equipSuit;
}
