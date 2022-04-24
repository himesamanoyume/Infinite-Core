using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// װ������
/// </summary>
public enum EquipType
{
    [Tooltip("ͷ��")]
    Head,
    [Tooltip("����")]
    Armor,
    [Tooltip("����")]
    Hand,
    [Tooltip("����")]
    Trousers,
    [Tooltip("��ϥ")]
    Knee,
    [Tooltip("Ь��")]
    Boots
}

/// <summary>
/// װ��Ʒ��
/// </summary>
public enum EquipQuality
{
    [Tooltip("��ͨ")]
    Normal,
    [Tooltip("����")]
    Artifact,
    [Tooltip("ʷʫ")]
    Epic,
    [Tooltip("����")]
    Strange
}

/// <summary>
/// װ����װ
/// </summary>
public enum EquipSuit
{
    Null,
    [Tooltip("��֮�Ժ�")]
    Preserve,
    [Tooltip("�츳����")]
    TheGifted,
    [Tooltip("���һǹ")]
    FeintShot,
    [Tooltip("�ļ����")]
    Worried,
    [Tooltip("��Ѫ��ħ")]
    Bloodthirsty,
    [Tooltip("��սʿ")]
    Berserker,
    [Tooltip("��������")]
    Impregnable,
    [Tooltip("ӿȪ�౨")]
    YongQuanXiangBao,
    [Tooltip("�ͽ�����")]
    BountyHunter,
    [Tooltip("����ԴȪ")]
    Lifespring,
    [Tooltip("�񱩽���")]
    CrazyAttack
}

#region װ��ö��

/// <summary>
/// ͷ��ö��
/// </summary>
public enum EquipHead
{
    Null,
    [Tooltip("��֮�Ժ�")]
    Preserve,
    [Tooltip("�츳����")]
    TheGifted,
    [Tooltip("���һǹ")]
    FeintShot,
    [Tooltip("�ļ����")]
    Worried,
    [Tooltip("��Ѫ��ħ")]
    Bloodthirsty,
    [Tooltip("��սʿ")]
    Berserker,
    [Tooltip("��������")]
    Impregnable,
    [Tooltip("ӿȪ�౨")]
    YongQuanXiangBao,
    [Tooltip("�ͽ�����")]
    BountyHunter,
    [Tooltip("����ԴȪ")]
    Lifespring,
    [Tooltip("�񱩺���")]
    CrazyAttack
}

/// <summary>
/// ����ö��
/// </summary>
public enum EquipArmor
{
    Null,
    [Tooltip("��֮�Ժ�")]
    Preserve,
    [Tooltip("�츳����")]
    TheGifted,
    [Tooltip("���һǹ")]
    FeintShot,
    [Tooltip("�ļ����")]
    Worried,
    [Tooltip("��Ѫ��ħ")]
    Bloodthirsty,
    [Tooltip("��սʿ")]
    Berserker,
    [Tooltip("��������")]
    Impregnable,
    [Tooltip("ӿȪ�౨")]
    YongQuanXiangBao,
    [Tooltip("�ͽ�����")]
    BountyHunter,
    [Tooltip("����ԴȪ")]
    Lifespring,
    [Tooltip("�񱩺���")]
    CrazyAttack
}

/// <summary>
/// ����ö��
/// </summary>
public enum EquipHand
{
    Null,
    [Tooltip("��֮�Ժ�")]
    Preserve,
    [Tooltip("�츳����")]
    TheGifted,
    [Tooltip("���һǹ")]
    FeintShot,
    [Tooltip("�ļ����")]
    Worried,
    [Tooltip("��Ѫ��ħ")]
    Bloodthirsty,
    [Tooltip("��սʿ")]
    Berserker,
    [Tooltip("��������")]
    Impregnable,
    [Tooltip("ӿȪ�౨")]
    YongQuanXiangBao,
    [Tooltip("�ͽ�����")]
    BountyHunter,
    [Tooltip("����ԴȪ")]
    Lifespring,
    [Tooltip("�񱩺���")]
    CrazyAttack
}

/// <summary>
/// ����ö��
/// </summary>
public enum EquipTrousers
{
    Null,
    [Tooltip("��֮�Ժ�")]
    Preserve,
    [Tooltip("�츳����")]
    TheGifted,
    [Tooltip("���һǹ")]
    FeintShot,
    [Tooltip("�ļ����")]
    Worried,
    [Tooltip("��Ѫ��ħ")]
    Bloodthirsty,
    [Tooltip("��սʿ")]
    Berserker,
    [Tooltip("��������")]
    Impregnable,
    [Tooltip("ӿȪ�౨")]
    YongQuanXiangBao,
    [Tooltip("�ͽ�����")]
    BountyHunter,
    [Tooltip("����ԴȪ")]
    Lifespring,
    [Tooltip("�񱩺���")]
    CrazyAttack
}

/// <summary>
/// ��ϥö��
/// </summary>
public enum EquipKnee
{
    Null,
    [Tooltip("��֮�Ժ�")]
    Preserve,
    [Tooltip("�츳����")]
    TheGifted,
    [Tooltip("���һǹ")]
    FeintShot,
    [Tooltip("�ļ����")]
    Worried,
    [Tooltip("��Ѫ��ħ")]
    Bloodthirsty,
    [Tooltip("��սʿ")]
    Berserker,
    [Tooltip("��������")]
    Impregnable,
    [Tooltip("ӿȪ�౨")]
    YongQuanXiangBao,
    [Tooltip("�ͽ�����")]
    BountyHunter,
    [Tooltip("����ԴȪ")]
    Lifespring,
    [Tooltip("�񱩺���")]
    CrazyAttack
}

/// <summary>
/// Ь��ö��
/// </summary>
public enum EquipBoots
{
    Null,
    [Tooltip("��֮�Ժ�")]
    Preserve,
    [Tooltip("�츳����")]
    TheGifted,
    [Tooltip("���һǹ")]
    FeintShot,
    [Tooltip("�ļ����")]
    Worried,
    [Tooltip("��Ѫ��ħ")]
    Bloodthirsty,
    [Tooltip("��սʿ")]
    Berserker,
    [Tooltip("��������")]
    Impregnable,
    [Tooltip("ӿȪ�౨")]
    YongQuanXiangBao,
    [Tooltip("�ͽ�����")]
    BountyHunter,
    [Tooltip("����ԴȪ")]
    Lifespring,
    [Tooltip("�񱩺���")]
    CrazyAttack
}

#endregion

/// <summary>
/// ������
/// </summary>
public enum MainEntry
{
    Null,
    [Tooltip("����")]
    Attack,
    [Tooltip("����")]
    Health,
    [Tooltip("����")]
    CriticalHit,
    [Tooltip("����")]
    CriticalHitRate,
    [Tooltip("����")]
    Defence,
    [Tooltip("����")]
    AttackSpeed
}

/// <summary>
/// ������
/// </summary>
public enum EntryLevelUp
{
    Null,
    Q,
    E,
    R
}


