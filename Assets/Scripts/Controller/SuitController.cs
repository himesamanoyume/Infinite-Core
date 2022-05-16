using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuitController : MonoBehaviour
{
    public Dictionary<int, EquipSuit> suitList = new Dictionary<int, EquipSuit>();

    CharBase charBase;

    public Dictionary<EquipSuit, int> suitCount = new Dictionary<EquipSuit, int>();

    Dictionary<EquipSuit, SuitEffectDelegate> suitEffectList = new Dictionary<EquipSuit, SuitEffectDelegate>();

    delegate void SuitEffectDelegate(EquipSuit equipSuit, bool enable);

    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSuitCount(EquipSuit equipSuit)
    {
        if (equipSuit != EquipSuit.Null)
        {
            suitCount[equipSuit]++;
            suitCount[EquipSuit.Null]--;

            if (suitCount[EquipSuit.Null] <= 0)
                suitCount[EquipSuit.Null] = 0;

            SuitUpdate();
        }
        
    }

    public void RemoveSuitCount(EquipSuit equipSuit)
    {
        suitCount[equipSuit]--;
        SuitUpdate();
    }

    public void SuitUpdate()
    {
        foreach (var item in suitCount)
        {
            if (item.Value >= 3)
            {
                suitEffectList[item.Key](item.Key, true);
            }
            else
            {
                suitEffectList[item.Key](item.Key, false);
            }
        }
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
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
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
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
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
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
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
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
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
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
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
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
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
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
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
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
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
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
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
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
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
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
    }
}
