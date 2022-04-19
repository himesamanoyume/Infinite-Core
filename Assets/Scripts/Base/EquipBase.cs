using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipBase : MonoBehaviour
{
    /// <summary>
    /// 装备ID
    /// </summary>
    //public EquipID equipID;

    /// <summary>
    /// 装备类型
    /// </summary>
    public EquipType equipType;

    /// <summary>
    /// 主词条
    /// </summary>
    public MainEntry mainEntry;

    /// <summary>
    /// 主词条数值
    /// </summary>
    public float mainEntryValue;

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

    #region props

    Color isWear = new Color(0, 0.4f, 0.1f);
    Color noWear = new Color(0.4f, 0.4f, 0.4f);

    int minNormalAttackValue = 500;
    int minArtifactAttackValue = 750;
    int minEpicAttackValue = 1000;
    int maxAttackValue = 2000;

    int minNormalHealthValue = 5000;
    int minArtifactHealthValue = 6500;
    int minEpicHealthValue = 8000;
    int maxHealthValue = 15000;

    float minNormalCriticalHitRateValue = 0.05f;
    float minArtifactCriticalHitRateValue = 0.1f;
    float minEpicCriticalHitRateValue = 0.15f;
    float maxCriticalHitRateValue = 0.45f;

    float minNormalCriticalHitValue = 0.15f;
    float minArtifactCriticalHitValue = 0.3f;
    float minEpicCriticalHitValue = 0.45f;
    float maxCriticalHitValue = 1f;

    int minNormalDefenceValue = 2000;
    int minArtifactDefenceValue = 3000;
    int minEpicDefenceValue = 4000;
    int maxDefenceValue = 9000;

    float minNormalAttackSpeedValue = 0.03f;
    float minArtifactAttackSpeedValue = 0.06f;
    float minEpicAttackSpeedValue = 0.09f;
    float maxAttackSpeedValue = 0.15f;

    #endregion

    public void InitEquip()
    {
        equipType = (EquipType)Random.Range(1, System.Enum.GetNames(typeof(EquipType)).Length + 1);

        float temp = Random.Range(0, 1f);
        if (0<=temp && temp< 0.5f)
        {
            equipQuality = EquipQuality.Normal;
        }else if (0.5f <= temp && temp < 0.8f)
        {
            equipQuality = EquipQuality.Artifact;
        }
        else if (0.8f <= temp && temp < 0.95f)
        {
            equipQuality = EquipQuality.Epic;
        }
        else
        {
            equipQuality = EquipQuality.Strange;
        }

        equipSuit = (EquipSuit)Random.Range(1, System.Enum.GetNames(typeof(EquipSuit)).Length + 1);

        switch (equipType)
        {
            case EquipType.Head:
                mainEntry = MainEntry.CriticalHitRate;
                InitHeadMainValue();
                break;
            case EquipType.Armor:
                mainEntry = MainEntry.Defence;
                InitArmorMainValue();
                break;
            case EquipType.Hand:
                mainEntry = MainEntry.Attack;
                InitHandMainValue();
                break;
            case EquipType.Trousers:
                mainEntry = MainEntry.Health;
                InitTrousersMainValue();
                break;
            case EquipType.Knee:
                mainEntry=MainEntry.CriticalHit;
                InitKneeMainValue();
                break;
            case EquipType.Boots:
                mainEntry = MainEntry.AttackSpeed;
                InitBootsMainValue();
                break;
        }

        InitEntry(equipQuality);

    }

    int RandomMainValue(int min, int max)
    {
        return Random.Range(min, max+1);
    }

    float RandomMainValue(float min, float max)
    {
        return Random.Range(min, max);
    } 


    void InitHeadMainValue()
    {
        switch (equipQuality)
        {
            case EquipQuality.Normal:
                mainEntryValue = RandomMainValue(minNormalCriticalHitRateValue, maxCriticalHitRateValue);
                break;
            case EquipQuality.Artifact:
                mainEntryValue = RandomMainValue(minArtifactCriticalHitRateValue, maxCriticalHitRateValue);
                break;
            case EquipQuality.Epic:
                mainEntryValue = RandomMainValue(minEpicCriticalHitRateValue, maxCriticalHitRateValue);
                break;
            case EquipQuality.Strange:
                mainEntryValue = RandomMainValue(minEpicCriticalHitRateValue, maxCriticalHitRateValue);
                break;
        }
    }

    void InitArmorMainValue()
    {
        switch (equipQuality)
        {
            case EquipQuality.Normal:
                mainEntryValue = RandomMainValue(minNormalDefenceValue, maxDefenceValue);
                break;
            case EquipQuality.Artifact:
                mainEntryValue = RandomMainValue(minArtifactDefenceValue, maxDefenceValue);
                break;
            case EquipQuality.Epic:
                mainEntryValue = RandomMainValue(minEpicDefenceValue, maxDefenceValue);
                break;
            case EquipQuality.Strange:
                mainEntryValue = RandomMainValue(minEpicDefenceValue, maxDefenceValue);
                break;
        }
    }

    void InitHandMainValue()
    {
        switch (equipQuality)
        {
            case EquipQuality.Normal:
                mainEntryValue = RandomMainValue(minNormalAttackValue, maxAttackValue);
                break;
            case EquipQuality.Artifact:
                mainEntryValue = RandomMainValue(minArtifactAttackValue, maxAttackValue);
                break;
            case EquipQuality.Epic:
                mainEntryValue = RandomMainValue(minEpicAttackValue, maxAttackValue);
                break;
            case EquipQuality.Strange:
                mainEntryValue = RandomMainValue(minEpicAttackValue, maxAttackValue);
                break;
        }
    }

    void InitTrousersMainValue()
    {
        switch (equipQuality)
        {
            case EquipQuality.Normal:
                mainEntryValue = RandomMainValue(minNormalHealthValue, maxHealthValue);
                break;
            case EquipQuality.Artifact:
                mainEntryValue = RandomMainValue(minArtifactHealthValue, maxHealthValue);
                break;
            case EquipQuality.Epic:
                mainEntryValue = RandomMainValue(minEpicHealthValue, maxHealthValue);
                break;
            case EquipQuality.Strange:
                mainEntryValue = RandomMainValue(minEpicHealthValue, maxHealthValue);
                break;
        }
    }

    void InitKneeMainValue()
    {
        switch (equipQuality)
        {
            case EquipQuality.Normal:
                mainEntryValue = RandomMainValue(minNormalCriticalHitValue, maxCriticalHitValue);
                break;
            case EquipQuality.Artifact:
                mainEntryValue = RandomMainValue(minArtifactCriticalHitValue, maxCriticalHitValue);
                break;
            case EquipQuality.Epic:
                mainEntryValue = RandomMainValue(minEpicCriticalHitValue, maxCriticalHitValue);
                break;
            case EquipQuality.Strange:
                mainEntryValue = RandomMainValue(minEpicCriticalHitValue, maxCriticalHitValue);
                break;
        }
    }

    void InitBootsMainValue()
    {
        switch (equipQuality)
        {
            case EquipQuality.Normal:
                mainEntryValue = RandomMainValue(minNormalAttackSpeedValue, maxAttackSpeedValue);
                break;
            case EquipQuality.Artifact:
                mainEntryValue = RandomMainValue(minArtifactAttackSpeedValue, maxAttackSpeedValue);
                break;
            case EquipQuality.Epic:
                mainEntryValue = RandomMainValue(minEpicAttackSpeedValue, maxAttackSpeedValue);
                break;
            case EquipQuality.Strange:
                mainEntryValue = RandomMainValue(minEpicAttackSpeedValue, maxAttackSpeedValue);
                break;
        }
    }

    void InitEntry(EquipQuality equipQuality)
    {
        switch (equipQuality)
        {
            case EquipQuality.Normal:
                firstEntry = (EntryLevelUp)Random.Range(0, 3);
                break;
            case EquipQuality.Artifact:
                firstEntry = (EntryLevelUp)Random.Range(0, 3);
                secondEntry = (EntryLevelUp)Random.Range(0, 3);
                break;
            default:
                firstEntry = (EntryLevelUp)Random.Range(0, 3);
                secondEntry = (EntryLevelUp)Random.Range(0, 3);
                thirdEntry = (EntryLevelUp)Random.Range(0, 3);
                break;
        }
    }


    public void GetEquipBase(out EquipBase equipBase)
    {
        equipBase = this;
    }
}
