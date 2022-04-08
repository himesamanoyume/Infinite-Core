using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventEnum
{
    rootGroup,

    #region SystemGroup
    SystemGroup,

    OnToast,

    #endregion

    #region PlayerGroup
    PlayerGroup,

    #region PlayerGroup -> PlayerMgrGroup
    PlayerMgrGroup,

    OnPlayerActorNumberChanged,
    OnPlayerNameChanged,
    OnPlayerTeamChanged,
    OnPlayerKill,
    OnPlayerKilled,
    OnPlayerDead,
    OnPlayerRespawn,
    OnPlayerRespawning,
    OnPlayerMoneyChanged,
    OnPlayerCurrentExpChanged,
    OnPlayerMaxExpChanged,
    OnPlayerStateChanged,
    OnPlayerBuffChanged,
    OnPlayerLevelChanged,
    OnPlayerLevelUp,
    OnPlayerAttackChanged,
    OnPlayerShieldChanged,
    OnPlayerMaxHealthChanged,
    OnPlayerCurrentHealthChanged,
    OnPlayerCriticalHitChanged,
    OnPlayerCriticalHitRateChanged,
    OnPlayerDefenceChanged,
    OnPlayerAttackSpeedChanged,
    OnPlayerRestoreChanged,
    OnPlayerRestoreing,

    #region PlayerGroup -> PlayerMgrGroup -> SkillGroup
    PlayerSkillGroup,
    #endregion

    #region PlayerGroup -> PlayerMgrGroup -> EquipGroup
    PlayerEquipGroup,
    #endregion

    OnPlayerMoveSpeedChanged,
    OnPlayerAttackRangeChanged,
    OnPlayerRespawnTimeChanged,
    OnPlayerRespawnCountDownStart,
    OnPlayerRespawnCountDownEnd

    #endregion
    #endregion

}
