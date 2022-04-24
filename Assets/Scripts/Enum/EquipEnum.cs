using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 装备类型
/// </summary>
public enum EquipType
{
    [Tooltip("头盔")]
    Head,
    [Tooltip("护甲")]
    Armor,
    [Tooltip("护手")]
    Hand,
    [Tooltip("护腿")]
    Trousers,
    [Tooltip("护膝")]
    Knee,
    [Tooltip("鞋子")]
    Boots
}

/// <summary>
/// 装备品质
/// </summary>
public enum EquipQuality
{
    [Tooltip("普通")]
    Normal,
    [Tooltip("神器")]
    Artifact,
    [Tooltip("史诗")]
    Epic,
    [Tooltip("奇特")]
    Strange
}

/// <summary>
/// 装备套装
/// </summary>
public enum EquipSuit
{
    Null,
    [Tooltip("持之以恒")]
    Preserve,
    [Tooltip("天赋异禀")]
    TheGifted,
    [Tooltip("虚晃一枪")]
    FeintShot,
    [Tooltip("心急如焚")]
    Worried,
    [Tooltip("嗜血狂魔")]
    Bloodthirsty,
    [Tooltip("狂战士")]
    Berserker,
    [Tooltip("固若金汤")]
    Impregnable,
    [Tooltip("涌泉相报")]
    YongQuanXiangBao,
    [Tooltip("赏金猎人")]
    BountyHunter,
    [Tooltip("生命源泉")]
    Lifespring,
    [Tooltip("狂暴进攻")]
    CrazyAttack
}

#region 装备枚举

/// <summary>
/// 头盔枚举
/// </summary>
public enum EquipHead
{
    Null,
    [Tooltip("持之以恒")]
    Preserve,
    [Tooltip("天赋异禀")]
    TheGifted,
    [Tooltip("虚晃一枪")]
    FeintShot,
    [Tooltip("心急如焚")]
    Worried,
    [Tooltip("嗜血狂魔")]
    Bloodthirsty,
    [Tooltip("狂战士")]
    Berserker,
    [Tooltip("固若金汤")]
    Impregnable,
    [Tooltip("涌泉相报")]
    YongQuanXiangBao,
    [Tooltip("赏金猎人")]
    BountyHunter,
    [Tooltip("生命源泉")]
    Lifespring,
    [Tooltip("狂暴轰入")]
    CrazyAttack
}

/// <summary>
/// 护甲枚举
/// </summary>
public enum EquipArmor
{
    Null,
    [Tooltip("持之以恒")]
    Preserve,
    [Tooltip("天赋异禀")]
    TheGifted,
    [Tooltip("虚晃一枪")]
    FeintShot,
    [Tooltip("心急如焚")]
    Worried,
    [Tooltip("嗜血狂魔")]
    Bloodthirsty,
    [Tooltip("狂战士")]
    Berserker,
    [Tooltip("固若金汤")]
    Impregnable,
    [Tooltip("涌泉相报")]
    YongQuanXiangBao,
    [Tooltip("赏金猎人")]
    BountyHunter,
    [Tooltip("生命源泉")]
    Lifespring,
    [Tooltip("狂暴轰入")]
    CrazyAttack
}

/// <summary>
/// 护手枚举
/// </summary>
public enum EquipHand
{
    Null,
    [Tooltip("持之以恒")]
    Preserve,
    [Tooltip("天赋异禀")]
    TheGifted,
    [Tooltip("虚晃一枪")]
    FeintShot,
    [Tooltip("心急如焚")]
    Worried,
    [Tooltip("嗜血狂魔")]
    Bloodthirsty,
    [Tooltip("狂战士")]
    Berserker,
    [Tooltip("固若金汤")]
    Impregnable,
    [Tooltip("涌泉相报")]
    YongQuanXiangBao,
    [Tooltip("赏金猎人")]
    BountyHunter,
    [Tooltip("生命源泉")]
    Lifespring,
    [Tooltip("狂暴轰入")]
    CrazyAttack
}

/// <summary>
/// 护腿枚举
/// </summary>
public enum EquipTrousers
{
    Null,
    [Tooltip("持之以恒")]
    Preserve,
    [Tooltip("天赋异禀")]
    TheGifted,
    [Tooltip("虚晃一枪")]
    FeintShot,
    [Tooltip("心急如焚")]
    Worried,
    [Tooltip("嗜血狂魔")]
    Bloodthirsty,
    [Tooltip("狂战士")]
    Berserker,
    [Tooltip("固若金汤")]
    Impregnable,
    [Tooltip("涌泉相报")]
    YongQuanXiangBao,
    [Tooltip("赏金猎人")]
    BountyHunter,
    [Tooltip("生命源泉")]
    Lifespring,
    [Tooltip("狂暴轰入")]
    CrazyAttack
}

/// <summary>
/// 护膝枚举
/// </summary>
public enum EquipKnee
{
    Null,
    [Tooltip("持之以恒")]
    Preserve,
    [Tooltip("天赋异禀")]
    TheGifted,
    [Tooltip("虚晃一枪")]
    FeintShot,
    [Tooltip("心急如焚")]
    Worried,
    [Tooltip("嗜血狂魔")]
    Bloodthirsty,
    [Tooltip("狂战士")]
    Berserker,
    [Tooltip("固若金汤")]
    Impregnable,
    [Tooltip("涌泉相报")]
    YongQuanXiangBao,
    [Tooltip("赏金猎人")]
    BountyHunter,
    [Tooltip("生命源泉")]
    Lifespring,
    [Tooltip("狂暴轰入")]
    CrazyAttack
}

/// <summary>
/// 鞋子枚举
/// </summary>
public enum EquipBoots
{
    Null,
    [Tooltip("持之以恒")]
    Preserve,
    [Tooltip("天赋异禀")]
    TheGifted,
    [Tooltip("虚晃一枪")]
    FeintShot,
    [Tooltip("心急如焚")]
    Worried,
    [Tooltip("嗜血狂魔")]
    Bloodthirsty,
    [Tooltip("狂战士")]
    Berserker,
    [Tooltip("固若金汤")]
    Impregnable,
    [Tooltip("涌泉相报")]
    YongQuanXiangBao,
    [Tooltip("赏金猎人")]
    BountyHunter,
    [Tooltip("生命源泉")]
    Lifespring,
    [Tooltip("狂暴轰入")]
    CrazyAttack
}

#endregion

/// <summary>
/// 主词条
/// </summary>
public enum MainEntry
{
    Null,
    [Tooltip("攻击")]
    Attack,
    [Tooltip("生命")]
    Health,
    [Tooltip("爆伤")]
    CriticalHit,
    [Tooltip("暴击")]
    CriticalHitRate,
    [Tooltip("防御")]
    Defence,
    [Tooltip("攻速")]
    AttackSpeed
}

/// <summary>
/// 副词条
/// </summary>
public enum EntryLevelUp
{
    Null,
    Q,
    E,
    R
}


