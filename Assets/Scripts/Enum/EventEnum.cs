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

    #region CharMgrGroup
    CharMgrGroup,

    AllowGetPlayerModelList,
    AllowGetRecorderList,
    AllowGetPlayerInfoBarList,


    #endregion

    #region PlayerGroup
    PlayerGroup,

        #region PlayerGroup -> PlayerMgrGroup
            PlayerMgrGroup,

            OnPlayerFinalDamageChanged,
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
            OnPlayerMoveSpeedChanged,
            OnPlayerAttackRangeChanged,
            OnPlayerRespawnTimeChanged,
            OnPlayerRespawnCountDownStart,
            OnPlayerRespawnCountDownEnd,
            OnPlayerDamaged,
            

            #region PlayerGroup -> PlayerMgrGroup -> SkillGroup
            PlayerSkillGroup,
            #endregion

            #region PlayerGroup -> PlayerMgrGroup -> EquipGroup
            PlayerEquipGroup,
            #endregion

        #endregion

        #region PlayerGroup -> PlayerControlGroup
            PlayerControlGroup,

            AllowPlayerTowardChanged,
            SendPlayerMoveSpeed,
            OnPlayerNormalAttack,
            AllowPlayerAttack,
            AllowPlayerMove,

        #endregion
    #endregion

}
