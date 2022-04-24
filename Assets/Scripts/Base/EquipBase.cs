using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipBase : MonoBehaviour
{
    /// <summary>
    /// 装备ID
    /// </summary>
    //public EquipID equipID;

    [Header("装备属性")]
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

    [Header("UI")]
    public Text equipNameText;
    public Text mainEntryText;
    public Text mainEntryValueText;
    public Image mainEntryQualityIcon;
    public Slider mainEntrySlider;
    public Text firstEntryText;
    public Text secondEntryText;
    public Text thirdEntryText;
    public Image background;
    public Button button;

    #region props

    Color normalColor = new Color(0.679f, 0.672f, 0.679f);
    Color artifactColor = new Color(0.886f, 0.313f, 0.886f);
    Color epicColor = new Color(1, 0.739f, 0.297f);
    Color strangeColor = new Color(1, 0, 0.270f);
    Color isWearColor = new Color(0, 0.4f, 0.1f);
    Color noWearColor = new Color(0.4f, 0.4f, 0.4f);

    [SerializeField]
    bool isWear = false; 
    GameObject wearGameObject;
    GameObject contentGameObject;

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
        equipType = (EquipType)Random.Range(0, System.Enum.GetNames(typeof(EquipType)).Length);

        contentGameObject = GameObject.FindGameObjectWithTag(equipType.ToString() + "Content");

        transform.SetParent(contentGameObject.transform);

        float temp = Random.Range(0, 1f);
        if (0<=temp && temp< 0.7f)
        {
            equipQuality = EquipQuality.Normal;
        }else if (0.7f <= temp && temp < 0.9f)
        {
            equipQuality = EquipQuality.Artifact;
        }
        else if (0.9f <= temp && temp < 0.99f)
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

        InitEntry();
        InitText();

        wearGameObject = GameObject.FindGameObjectWithTag(equipType.ToString() + "Wear");

        GameObject prefab = (GameObject)Resources.Load("Prefabs/UI/MiscInfo/MiscInfo");

        switch (equipQuality)
        {
            case EquipQuality.Normal:
                Instantiate(prefab).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Normal, "获得了普通"+ equipNameText.text);
                break;
            case EquipQuality.Artifact:
                Instantiate(prefab).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Artifact, "获得了神器" + equipNameText.text);
                break;
            case EquipQuality.Epic:
                Instantiate(prefab).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, "获得了史诗" + equipNameText.text);
                break;
            case EquipQuality.Strange:
                Instantiate(prefab).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Normal, "获得了奇特" + equipNameText.text+"!");
                break;
            default:
                break;
        }

    }

    public void EquipItemOnClick()
    {
        if (!isWear)
        {
            if (wearGameObject.transform.GetComponentsInChildren<Transform>(true).Length > 1)
            {
                wearGameObject.transform.GetChild(0).GetComponent<EquipBase>().TakeOffEquip();
                transform.SetParent(wearGameObject.transform);
                transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(174.8149f, -105);
                isWear = true;
            }
            else
            {
                transform.SetParent(wearGameObject.transform);
                transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(174.8149f, -105);
                isWear = true;
            }
        }
    }

    int RandomMainValue(int min, int max)
    {
        return Random.Range(min, max+1);
    }

    float RandomMainValue(float min, float max)
    {
        return Random.Range(min, max);
    } 

    public void TakeOffEquip()
    {
        transform.SetParent(contentGameObject.transform);
        isWear = false;
    }

    delegate void GetCnDele(out string cnText);
    GetCnDele getCnDele;

    public void GetCnMainEntryText(out string equipMainEntryCNText)
    {
        switch (mainEntry)
        {
            case MainEntry.Null:
                equipMainEntryCNText = "无";
                break;
            case MainEntry.Attack:
                equipMainEntryCNText = "攻击";
                break;
            case MainEntry.Health:
                equipMainEntryCNText = "生命";
                break;
            case MainEntry.CriticalHit:
                equipMainEntryCNText = "爆伤";
                break;
            case MainEntry.CriticalHitRate:
                equipMainEntryCNText = "暴击";
                break;
            case MainEntry.Defence:
                equipMainEntryCNText = "防御";
                break;
            case MainEntry.AttackSpeed:
                equipMainEntryCNText = "攻速";
                break;
            default:
                equipMainEntryCNText = "无";
                break;
        }
    }

    public void GetCnSuitText(out string equipSuitCNText)
    {
        switch (equipSuit)
        {
            case EquipSuit.Null:
                equipSuitCNText = "";
                break;
            case EquipSuit.Preserve:
                equipSuitCNText = "持之以恒";
                break;
            case EquipSuit.TheGifted:
                equipSuitCNText = "天赋异禀";
                break;
            case EquipSuit.FeintShot:
                equipSuitCNText = "虚晃一枪";
                break;
            case EquipSuit.Worried:
                equipSuitCNText = "心急如焚";
                break;
            case EquipSuit.Bloodthirsty:
                equipSuitCNText = "嗜血狂魔";
                break;
            case EquipSuit.Berserker:
                equipSuitCNText = "狂战士";
                break;
            case EquipSuit.Impregnable:
                equipSuitCNText = "固若金汤";
                break;
            case EquipSuit.YongQuanXiangBao:
                equipSuitCNText = "涌泉相报";
                break;
            case EquipSuit.BountyHunter:
                equipSuitCNText = "赏金猎人";
                break;
            case EquipSuit.Lifespring:
                equipSuitCNText = "生命源泉";
                break;
            case EquipSuit.CrazyAttack:
                equipSuitCNText = "狂暴进攻";
                break;
            default:
                equipSuitCNText = "无";
                break;
        }
    }

    public void GetCnTypeText(out string equipTypeCNText)
    {
        switch (equipType)
        {
            case EquipType.Head:
                equipTypeCNText = "头盔";
                break;
            case EquipType.Armor:
                equipTypeCNText = "护甲";
                break;
            case EquipType.Hand:
                equipTypeCNText = "护手";
                break;
            case EquipType.Trousers:
                equipTypeCNText = "护腿";
                break;
            case EquipType.Knee:
                equipTypeCNText = "护膝";
                break;
            case EquipType.Boots:
                equipTypeCNText = "鞋子";
                break;
            default:
                equipTypeCNText = "无";
                break;
        }
    }

    public void GetCnQualityText(out string equipQualityCNText)
    {
        switch (equipQuality)
        {
            case EquipQuality.Normal:
                equipQualityCNText = "普通";
                break;
            case EquipQuality.Artifact:
                equipQualityCNText = "神器";
                break;
            case EquipQuality.Epic:
                equipQualityCNText = "史诗";
                break;
            case EquipQuality.Strange:
                equipQualityCNText = "奇特";
                break;
            default:
                equipQualityCNText = "无";
                break;
        }
    }

    void InitText()
    {

        getCnDele = GetCnMainEntryText;
        getCnDele(out string equipMainEntryCNText);
        getCnDele = GetCnSuitText;
        getCnDele(out string equipSuitCNText);
        getCnDele = GetCnTypeText;
        getCnDele(out string equipTypeCNText);
        getCnDele= GetCnQualityText;
        getCnDele(out string equipQualityCNText);

        equipNameText.text = equipSuitCNText + equipTypeCNText;
        mainEntryText.text = equipMainEntryCNText;
        switch (mainEntry)
        {
            case MainEntry.Null:
                mainEntryValueText.text = mainEntryValue.ToString("f1");
                break;
            case MainEntry.Attack:
                mainEntryValueText.text = mainEntryValue.ToString();
                break;
            case MainEntry.Health:
                mainEntryValueText.text = ((int)mainEntryValue).ToString();
                break;
            case MainEntry.CriticalHit:
                mainEntryValueText.text = (mainEntryValue * 100).ToString("f1") + "%";
                break;
            case MainEntry.CriticalHitRate:
                mainEntryValueText.text = (mainEntryValue * 100).ToString("f1") + "%";
                break;
            case MainEntry.Defence:
                mainEntryValueText.text = mainEntryValue.ToString();
                break;
            case MainEntry.AttackSpeed:
                mainEntryValueText.text = (mainEntryValue * 100).ToString("f1") + "%";
                break;
            default:
                mainEntryValueText.text = mainEntryValue.ToString("f1");
                break;
        }
        
        switch (equipQuality)
        {
            case EquipQuality.Normal:
                mainEntryQualityIcon.color = normalColor;
                InitViceEntry(firstEntryText, firstEntry);
                break;
            case EquipQuality.Artifact:
                mainEntryQualityIcon.color = artifactColor;
                InitViceEntry(firstEntryText, firstEntry);
                InitViceEntry(secondEntryText, secondEntry);
                break;
            case EquipQuality.Epic:
                mainEntryQualityIcon.color = epicColor;
                InitViceEntry(firstEntryText, firstEntry);
                InitViceEntry(secondEntryText, secondEntry);
                InitViceEntry(thirdEntryText, thirdEntry);
                break;
            case EquipQuality.Strange:
                mainEntryQualityIcon.color = strangeColor;
                InitViceEntry(firstEntryText, firstEntry);
                InitViceEntry(secondEntryText, secondEntry);
                InitViceEntry(thirdEntryText, thirdEntry);
                break;
        }
        switch (mainEntry)
        {
            case MainEntry.Attack:
                mainEntrySlider.maxValue = maxAttackValue;
                mainEntrySlider.value = mainEntryValue;
                break;
            case MainEntry.Health:
                mainEntrySlider.maxValue = maxHealthValue;
                mainEntrySlider.value = mainEntryValue;
                break;
            case MainEntry.CriticalHit:
                mainEntrySlider.maxValue = maxCriticalHitValue;
                mainEntrySlider.value = mainEntryValue;
                break;
            case MainEntry.CriticalHitRate:
                mainEntrySlider.maxValue = maxCriticalHitRateValue;
                mainEntrySlider.value = mainEntryValue;
                break;
            case MainEntry.Defence:
                mainEntrySlider.maxValue = maxDefenceValue;
                mainEntrySlider.value = mainEntryValue;
                break;
            case MainEntry.AttackSpeed:
                mainEntrySlider.maxValue = maxAttackSpeedValue;
                mainEntrySlider.value = mainEntryValue;
                break;
        }
        
    }

    /// <summary>
    /// 副词条的技能提升
    /// </summary>
    /// <param name="target"></param>
    /// <param name="targetLevelUp"></param>
    void InitViceEntry(Text target, EntryLevelUp targetLevelUp)
    {
        target.text = targetLevelUp.ToString();
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

    void InitEntry()
    {
        switch (equipQuality)
        {
            case EquipQuality.Normal:
                firstEntry = (EntryLevelUp)Random.Range(1, 4);
                break;
            case EquipQuality.Artifact:
                firstEntry = (EntryLevelUp)Random.Range(1, 4);
                secondEntry = (EntryLevelUp)Random.Range(1, 4);
                break;
            default:
                firstEntry = (EntryLevelUp)Random.Range(1, 4);
                secondEntry = (EntryLevelUp)Random.Range(1, 4);
                thirdEntry = (EntryLevelUp)Random.Range(1, 4);
                break;
        }
    }

    public void GetEquipBase(out EquipBase equipBase)
    {
        equipBase = this;
    }


    private void Start()
    {
        
    }
}
