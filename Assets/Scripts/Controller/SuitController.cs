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

        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " ��װЧ�� " + (enable ? "��Ч" : "ʧЧ"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
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
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " ��װЧ�� " + (enable?"��Ч":"ʧЧ"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
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
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " ��װЧ�� " + (enable ? "��Ч" : "ʧЧ"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
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
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " ��װЧ�� " + (enable ? "��Ч" : "ʧЧ"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
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
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " ��װЧ�� " + (enable ? "��Ч" : "ʧЧ"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
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
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " ��װЧ�� " + (enable ? "��Ч" : "ʧЧ"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
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
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " ��װЧ�� " + (enable ? "��Ч" : "ʧЧ"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
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
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " ��װЧ�� " + (enable ? "��Ч" : "ʧЧ"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
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
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " ��װЧ�� " + (enable ? "��Ч" : "ʧЧ"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
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
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " ��װЧ�� " + (enable ? "��Ч" : "ʧЧ"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
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
        Instantiate(infoPrefab, GameObject.FindGameObjectWithTag("MiscInfoMenu").transform).GetComponent<MiscInfo>().InitMiscInfo(MiscLevel.Epic, equipSuit + " ��װЧ�� " + (enable ? "��Ч" : "ʧЧ"));
        //Debug.LogError(equipSuit + " SuitEffect " + enable);
    }
}
