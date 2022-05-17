using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class SuitController : MonoBehaviour
{
    public Dictionary<int, EquipSuit> suitList = new Dictionary<int, EquipSuit>();

    CharBase charBase;

    public Dictionary<EquipSuit, int> suitCount = new Dictionary<EquipSuit, int>();

    Dictionary<EquipSuit, SuitEffectDelegate> suitEffectList = new Dictionary<EquipSuit, SuitEffectDelegate>();

    Dictionary<EquipSuit, bool> suitState = new Dictionary<EquipSuit, bool>();

    delegate void SuitEffectDelegate(EquipSuit equipSuit, bool enable);

    PhotonView photonView;

    GameObject infoPrefab;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        suitCount.Add(EquipSuit.Null, 6);
        suitCount.Add(EquipSuit.Preserve, 0);
        suitCount.Add(EquipSuit.TheGifted, 0);
        suitCount.Add(EquipSuit.FeintShot, 0);
        suitCount.Add(EquipSuit.Worried, 0);
        suitCount.Add(EquipSuit.Bloodthirsty, 0);
        suitCount.Add(EquipSuit.Berserker, 0);
        suitCount.Add(EquipSuit.Impregnable, 0);
        suitCount.Add(EquipSuit.YongQuanXiangBao, 0);
        suitCount.Add(EquipSuit.BountyHunter, 0);
        suitCount.Add(EquipSuit.Lifespring, 0);
        suitCount.Add(EquipSuit.CrazyAttack, 0);

        SuitEffectDelegate suitEffectDelegate;
        suitEffectList.Add(EquipSuit.Null, suitEffectDelegate = NullSuitEffect);
        suitEffectList.Add(EquipSuit.Preserve, suitEffectDelegate = PreserveSuitEffect);
        suitEffectList.Add(EquipSuit.TheGifted, suitEffectDelegate = TheGiftedSuitEffect);
        suitEffectList.Add(EquipSuit.FeintShot, suitEffectDelegate = FeintShotSuitEffect);
        suitEffectList.Add(EquipSuit.Worried, suitEffectDelegate = WorriedSuitEffect);
        suitEffectList.Add(EquipSuit.Bloodthirsty, suitEffectDelegate = BloodthirstySuitEffect);
        suitEffectList.Add(EquipSuit.Berserker, suitEffectDelegate = BerserkerSuitEffect);
        suitEffectList.Add(EquipSuit.Impregnable, suitEffectDelegate = ImpregnableSuitEffect);
        suitEffectList.Add(EquipSuit.YongQuanXiangBao, suitEffectDelegate = YongQuanXiangBaoSuitEffect);
        suitEffectList.Add(EquipSuit.BountyHunter, suitEffectDelegate = BountyHunterSuitEffect);
        suitEffectList.Add(EquipSuit.Lifespring, suitEffectDelegate = LifespringSuitEffect);
        suitEffectList.Add(EquipSuit.CrazyAttack, suitEffectDelegate = CrazyAttackSuitEffect);

        suitState.Add(EquipSuit.Null, true);
        suitState.Add(EquipSuit.Preserve, false);
        suitState.Add(EquipSuit.TheGifted, false);
        suitState.Add(EquipSuit.FeintShot, false);
        suitState.Add(EquipSuit.Worried, false);
        suitState.Add(EquipSuit.Bloodthirsty, false);
        suitState.Add(EquipSuit.Berserker, false);
        suitState.Add(EquipSuit.Impregnable, false);
        suitState.Add(EquipSuit.YongQuanXiangBao, false);
        suitState.Add(EquipSuit.BountyHunter, false);
        suitState.Add(EquipSuit.Lifespring, false);
        suitState.Add(EquipSuit.CrazyAttack, false);

        infoPrefab = (GameObject)Resources.Load("Prefabs/UI/MiscInfo/MiscInfoPrefab");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSuitCount(EquipSuit equipSuit)
    {
        if (photonView.IsMine)
        {
            suitCount[equipSuit]++;
            //Debug.LogError(equipSuit + " +");
        } 
    }

    public void RemoveSuitCount(EquipSuit equipSuit)
    {
        if (photonView.IsMine)
        {
            suitCount[equipSuit]--;
            if (suitCount[EquipSuit.Null] <= 0)
                suitCount[EquipSuit.Null] = 0;
            //Debug.LogError(equipSuit + " -");
        }
            
    }

    public void SuitUpdate()
    {
        if (photonView.IsMine)
        {
            foreach (var item in suitCount)
            {
                if (item.Value >= 3)
                {
                    if (suitState[item.Key] == false)
                    {
                        suitEffectList[item.Key](item.Key, true);
                        suitState[item.Key] = true;
                        //Debug.LogError(item.Key + " true");
                    }
                }
                else
                {
                    if (suitState[item.Key] == true)
                    {
                        suitEffectList[item.Key](item.Key, false);
                        suitState[item.Key] = false;
                        //Debug.LogError(item.Key + " false");
                    }
                }
            }
        }
    }

    void NullSuitEffect(EquipSuit equipSuit, bool enable)
    {
        if (enable)
        {

        }
        else
        {

        }
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// 持之以恒
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void PreserveSuitEffect(EquipSuit equipSuit ,bool enable)
    {
        //普攻倍率增加
        if (enable)
        {
            charBase.NormalAttackRatio += 0.3f;
        }
        else
        {
            charBase.NormalAttackRatio -= 0.3f;
        }

        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " 套装效果 " + (enable ? "生效" : "失效"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// 天赋异禀
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void TheGiftedSuitEffect(EquipSuit equipSuit, bool enable)
    {
        //技能倍率增加
        if (enable)
        {
            
        }
        else
        {
            
        }
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " 套装效果 " + (enable?"生效":"失效"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// 虚晃一枪
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void FeintShotSuitEffect(EquipSuit equipSuit, bool enable)
    {
        //技能释放后有一定概率无冷却时间
        if (enable)
        {

        }
        else
        {

        }
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " 套装效果 " + (enable ? "生效" : "失效"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// 心急如焚
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void WorriedSuitEffect(EquipSuit equipSuit, bool enable)
    {
        //技能冷却时间减少
        if (enable)
        {

        }
        else
        {

        }
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " 套装效果 " + (enable ? "生效" : "失效"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// 嗜血狂魔
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void BloodthirstySuitEffect(EquipSuit equipSuit, bool enable)
    {
        //根据造成的伤害按百分比获取生命
        if (enable)
        {

        }
        else
        {

        }
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " 套装效果 " + (enable ? "生效" : "失效"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// 狂战士
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void BerserkerSuitEffect(EquipSuit equipSuit, bool enable)
    {
        //生命越少，攻速越快，暴击爆伤增加
        if (enable)
        {

        }
        else
        {

        }
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " 套装效果 " + (enable ? "生效" : "失效"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// 固若金汤
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void ImpregnableSuitEffect(EquipSuit equipSuit, bool enable)
    {
        //根据防御生成护盾，攻击下降，护盾被破坏后60秒后才能生成，造成伤害时回复损失的护盾值，护盾被破坏时对周围造成一次伤害
        if (enable)
        {

        }
        else
        {

        }
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " 套装效果 " + (enable ? "生效" : "失效"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// 涌泉相报
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void YongQuanXiangBaoSuitEffect(EquipSuit equipSuit, bool enable)
    {
        //根据受到伤害返还给敌人
        if (enable)
        {

        }
        else
        {

        }
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " 套装效果 " + (enable ? "生效" : "失效"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// 赏金猎人
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void BountyHunterSuitEffect(EquipSuit equipSuit, bool enable)
    {
        //该角色所持金币越多，攻击力增加，移速增加
        if (enable)
        {

        }
        else
        {

        }
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " 套装效果 " + (enable ? "生效" : "失效"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// 生命源泉
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void LifespringSuitEffect(EquipSuit equipSuit, bool enable)
    {
        //生命恢复增加
        if (enable)
        {

        }
        else
        {

        }
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " 套装效果 " + (enable ? "生效" : "失效"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// 狂暴进攻
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void CrazyAttackSuitEffect(EquipSuit equipSuit, bool enable)
    {
        //攻速增加
        if (enable)
        {

        }
        else
        {

        }
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " 套装效果 " + (enable ? "生效" : "失效"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
    }
}
