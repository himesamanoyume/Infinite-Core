using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventName
{
    rootGroup,

    #region SystemGroup
    systemGroup,

    onToast,

    #endregion

    #region PlayerGroup
    playerGroup,

    #region PlayerGroup -> PlayerMgrGroup
    playerMgrGroup,

    onPlayerActorNumberChanged,
    onPlayerNameChanged,
    onPlayerTeamChanged,
    onPlayerKill,
    onPlayerKilled,
    onPlayerDead,
    onPlayerRespawn,
    onPlayerRespawning,
    onPlayerMoneyChanged,
    onPlayerCurrentExpChanged,
    onPlayerMaxExpChanged,
    onPlayerStateChanged,
    onPlayerBuffChanged,
    onPlayerLevelChanged,
    onPlayerLevelUp,
    onPlayerAttackChanged,
    onPlayerShieldChanged,
    onPlayerMaxHealthChanged,
    onPlayerCurrentHealthChanged,
    onPlayerCriticalHitChanged,
    onPlayerCriticalHitRateChanged,
    onPlayerDefenceChanged,
    onPlayerAttackSpeedChanged,
    onPlayerRestoreChanged,

    #region PlayerGroup -> PlayerMgrGroup -> SkillGroup
    playerSkillGroup,
    #endregion

    #region PlayerGroup -> PlayerMgrGroup -> EquipGroup
    playerEquipGroup,
    #endregion

    onPlayerMoveSpeedChanged,
    onPlayerAttackRangeChanged,
    onPlayerRespawnTimeChanged,
    onPlayerRespawnCountDownStart,
    onPlayerRespawnCountDownEnd

    #endregion
    #endregion

}

public class GameEventBase 
{
    #region Event Name

    private EventName eventName;

    public EventName Name
    {
        get { return eventName; }
        protected set { eventName = value; }
    }

    #endregion

    #region Event state

    private bool enable;

    public bool Enable
    {
        get { return enable; }
        set { enable = value; }
    }
    #endregion

    public virtual void Update() { }
}
