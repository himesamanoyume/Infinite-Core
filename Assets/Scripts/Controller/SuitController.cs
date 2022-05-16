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
    /// ��֮�Ժ�
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void PreserveSuitEffect(EquipSuit equipSuit ,bool enable)
    {
        //�չ���������
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
    /// �츳����
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void TheGiftedSuitEffect(EquipSuit equipSuit, bool enable)
    {
        //���ܱ�������
        if (enable)
        {
            
        }
        else
        {
            
        }
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// ���һǹ
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void FeintShotSuitEffect(EquipSuit equipSuit, bool enable)
    {
        //�����ͷź���һ����������ȴʱ��
        if (enable)
        {

        }
        else
        {

        }
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// �ļ����
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void WorriedSuitEffect(EquipSuit equipSuit, bool enable)
    {
        //������ȴʱ�����
        if (enable)
        {

        }
        else
        {

        }
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// ��Ѫ��ħ
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void BloodthirstySuitEffect(EquipSuit equipSuit, bool enable)
    {
        //������ɵ��˺����ٷֱȻ�ȡ����
        if (enable)
        {

        }
        else
        {

        }
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// ��սʿ
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void BerserkerSuitEffect(EquipSuit equipSuit, bool enable)
    {
        //����Խ�٣�����Խ�죬������������
        if (enable)
        {

        }
        else
        {

        }
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void ImpregnableSuitEffect(EquipSuit equipSuit, bool enable)
    {
        //���ݷ������ɻ��ܣ������½������ܱ��ƻ���60���������ɣ�����˺�ʱ�ظ���ʧ�Ļ���ֵ�����ܱ��ƻ�ʱ����Χ���һ���˺�
        if (enable)
        {

        }
        else
        {

        }
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// ӿȪ�౨
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void YongQuanXiangBaoSuitEffect(EquipSuit equipSuit, bool enable)
    {
        //�����ܵ��˺�����������
        if (enable)
        {

        }
        else
        {

        }
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// �ͽ�����
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void BountyHunterSuitEffect(EquipSuit equipSuit, bool enable)
    {
        //�ý�ɫ���ֽ��Խ�࣬���������ӣ���������
        if (enable)
        {

        }
        else
        {

        }
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// ����ԴȪ
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void LifespringSuitEffect(EquipSuit equipSuit, bool enable)
    {
        //�����ָ�����
        if (enable)
        {

        }
        else
        {

        }
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
    }

    /// <summary>
    /// �񱩽���
    /// </summary>
    /// <param name="equipSuit"></param>
    /// <param name="enable"></param>
    void CrazyAttackSuitEffect(EquipSuit equipSuit, bool enable)
    {
        //��������
        if (enable)
        {

        }
        else
        {

        }
        Debug.LogWarning(equipSuit + " SuitEffect " + enable);
    }
}
