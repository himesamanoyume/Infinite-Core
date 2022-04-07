using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager
{
    /// <summary>
    /// Root组 管理全部事件
    /// </summary>
    private static GameEventGroup rootGroup;

    public static GameEventGroup RootGroup
    {
        get { return rootGroup; }
        private set { rootGroup = value; }
    }

    static GameEventManager()
    {
        Init();

        //于此处控制事件组和事件的初始状态
        RootGroup.Enable = true;
        EnableAllEvents(EVENT_PLAYER_GROUP, true);
    }

    #region Const Event Name

    public const string EVENT_PLAYER_GROUP = "PlayerGroup";
    public const string EVENT_PLAYER_MGR_GROUP = "PlayerMgrGroup";
    public const string EVENT_ON_PLAYER_ACTORNUMBER_CHANGED = "OnPlayerActorNumberChanged";
    public const string EVENT_ON_PLAYER_NAME_CHANGED = "OnPlayerNameChanged";
    public const string EVENT_ON_PLAYER_TEAM_CHANGED = "OnPlayerTeamChanged";
    public const string EVENT_ON_PLAYER_KILL = "OnPlayerKill";
    public const string EVENT_ON_PLAYER_KILLED = "OnPlayerKilled";
    public const string EVENT_ON_PLAYER_DEAD = "OnPlayerDead";
    public const string EVENT_ON_PLAYER_RESPAWN = "OnPlayerRespawn";
    public const string EVENT_ON_PLAYER_MONEY_CHANGED = "OnPlayerMoneyChanged";
    public const string EVENT_ON_PLAYER_CURRENT_EXP_CHANGED = "OnPlayerCurrentExpChanged";
    public const string EVENT_ON_PLAYER_MAX_EXP_CHANGED = "OnPlayerMaxExpChanged";
    public const string EVENT_ON_PLAYER_STATE_CHANGED = "OnPlayerStateChanged";
    public const string EVENT_ON_PLAYER_BUFF_CHANGED = "OnPlayerBuffChanged";
    public const string EVENT_ON_PLAYER_LEVEL_CHANGED = "OnPlayerLevelChanged";
    public const string EVENT_ON_PLAYER_LEVEL_UP = "OnPlayerLevelUp";
    public const string EVENT_ON_PLAYER_ATTACK_CHANGED = "OnPlayerAttackChanged";
    public const string EVENT_ON_PLAYER_SHIELD_CHANGED = "OnPlayerShieldChanged";
    public const string EVENT_ON_PLAYER_MAX_HEALTH_CHANGED = "OnPlayerMaxHealthChanged";
    public const string EVENT_ON_PLAYER_CURRENT_HEALTH_CHANGED = "OnPlayerCurrentHealthChanged";
    public const string EVENT_ON_PLAYER_CRITICAL_HIT_CHANGED = "OnPlayerCriticalHitChanged";
    public const string EVENT_ON_PLAYER_CRITICAL_HIT_RATE_CHANGED = "OnPlayerCriticalHitRateChanged";
    public const string EVENT_ON_PLAYER_DEFENCE_CHANGED = "OnPlayerDefenceChanged";
    public const string EVENT_ON_PLAYER_ATTACK_SPEED_CHANGED = "OnPlayerAttackSpeedChanged";
    public const string EVENT_ON_PLAYER_RESTORE_CHANGED = "OnPlayerRestoreChanged";
    public const string EVENT_PLAYER_SKILL_GROUP = "PlayerSkillGroup";
    public const string EVENT_PLAYER_EQUIP_GROUP = "PlayerEquipGroup";
    public const string EVENT_ON_PLAYER_MOVE_SPEED_CHANGED = "OnPlayerMoveSpeedChanged";
    public const string EVENT_ON_PLAYER_ATTACK_RANGE_CHANGED = "OnPlayerAttackRangeChanged";
    public const string EVENT_ON_PLAYER_RESPAWN_TIME_CHANGED = "OnPlayerRespawnTimeChanged";
    public const string EVENT_ON_PLAYER_RESPAWN_COUNTDOWN_START = "OnPlayerRespawnCountDownStart";
    public const string EVENT_ON_PLAYER_RESPAWN_COUNTDOWN_END = "OnPlayerRespawnCountDownEnd";

    #endregion

    private static void Init()
    {
        RootGroup = new GameEventGroup("RootGroup");

        //于此处添加事件组和事件

        #region PlayerGroup
        GameEventGroup playerGroup = new GameEventGroup(EVENT_PLAYER_GROUP);
        RootGroup.AddEvent(playerGroup);

        #region PlayerGroup -> PlayerMgrGroup
        GameEventGroup playerMgrGroup = new GameEventGroup(EVENT_PLAYER_MGR_GROUP);
        playerGroup.AddEvent(playerMgrGroup);

        GameEvent onPlayerActorNumberChanged = new GameEvent(EVENT_ON_PLAYER_ACTORNUMBER_CHANGED);
        playerMgrGroup.AddEvent(onPlayerActorNumberChanged);

        GameEvent onPlayerNameChanged = new GameEvent(EVENT_ON_PLAYER_NAME_CHANGED);
        playerMgrGroup.AddEvent(onPlayerNameChanged);

        GameEvent onPlayerTeamChanged = new GameEvent(EVENT_ON_PLAYER_TEAM_CHANGED);
        playerMgrGroup.AddEvent(onPlayerTeamChanged);

        GameEvent onPlayerKill = new GameEvent(EVENT_ON_PLAYER_KILL);
        playerMgrGroup.AddEvent(onPlayerKill);

        GameEvent onPlayerKilled = new GameEvent(EVENT_ON_PLAYER_KILLED);
        playerMgrGroup.AddEvent(onPlayerKilled);

        GameEvent onPlayerDead = new GameEvent(EVENT_ON_PLAYER_DEAD);
        playerMgrGroup.AddEvent(onPlayerDead);

        GameEvent onPlayerRespawn = new GameEvent(EVENT_ON_PLAYER_RESPAWN);
        playerMgrGroup.AddEvent(onPlayerRespawn);

        GameEvent onPlayerMoneyChanged = new GameEvent(EVENT_ON_PLAYER_MONEY_CHANGED);
        playerMgrGroup.AddEvent(onPlayerMoneyChanged);

        GameEvent onPlayerCurrentExpChanged = new GameEvent(EVENT_ON_PLAYER_CURRENT_EXP_CHANGED);
        playerMgrGroup.AddEvent(onPlayerCurrentExpChanged);

        GameEvent onPlayerMaxExpChanged = new GameEvent(EVENT_ON_PLAYER_MAX_EXP_CHANGED);
        playerMgrGroup.AddEvent(onPlayerMaxExpChanged);

        GameEvent onPlayerStateChanged = new GameEvent(EVENT_ON_PLAYER_STATE_CHANGED);
        playerMgrGroup.AddEvent(onPlayerStateChanged);

        GameEvent onPlayerBuffChanged = new GameEvent(EVENT_ON_PLAYER_BUFF_CHANGED);
        playerMgrGroup.AddEvent(onPlayerBuffChanged);

        GameEvent onPlayerLevelChanged = new GameEvent(EVENT_ON_PLAYER_LEVEL_CHANGED);
        playerMgrGroup.AddEvent(onPlayerLevelChanged);

        GameEvent onPlayerLevelUp = new GameEvent(EVENT_ON_PLAYER_LEVEL_UP);
        playerMgrGroup.AddEvent(onPlayerLevelUp);

        GameEvent onPlayerAttackChanged = new GameEvent(EVENT_ON_PLAYER_ATTACK_CHANGED);
        playerMgrGroup.AddEvent(onPlayerAttackChanged);

        GameEvent onPlayerShieldChanged = new GameEvent(EVENT_ON_PLAYER_SHIELD_CHANGED);
        playerMgrGroup.AddEvent(onPlayerShieldChanged);

        GameEvent onPlayerMaxHealthChanged = new GameEvent(EVENT_ON_PLAYER_MAX_HEALTH_CHANGED);
        playerMgrGroup.AddEvent(onPlayerMaxHealthChanged);

        GameEvent onPlayerCurrentHealthChanged = new GameEvent(EVENT_ON_PLAYER_CURRENT_HEALTH_CHANGED);
        playerMgrGroup.AddEvent(onPlayerCurrentHealthChanged);

        GameEvent onPlayerCriticalHitChanged = new GameEvent(EVENT_ON_PLAYER_CRITICAL_HIT_CHANGED);
        playerMgrGroup.AddEvent(onPlayerCriticalHitChanged);

        GameEvent onPlayerCriticalHitRateChanged = new GameEvent(EVENT_ON_PLAYER_CRITICAL_HIT_RATE_CHANGED);
        playerMgrGroup.AddEvent(onPlayerCriticalHitRateChanged);

        GameEvent onPlayerDefenceChanged = new GameEvent(EVENT_ON_PLAYER_DEFENCE_CHANGED);
        playerMgrGroup.AddEvent(onPlayerDefenceChanged);

        GameEvent onPlayerAttackSpeedChanged = new GameEvent(EVENT_ON_PLAYER_ATTACK_SPEED_CHANGED);
        playerMgrGroup.AddEvent(onPlayerAttackSpeedChanged);

        GameEvent onPlayerRestoreChanged = new GameEvent(EVENT_ON_PLAYER_RESTORE_CHANGED);
        playerMgrGroup.AddEvent(onPlayerRestoreChanged);

        #region PlayerGroup -> PlayerMgrGroup -> SkillGroup

        GameEventGroup playerSkillGroup = new GameEventGroup(EVENT_PLAYER_SKILL_GROUP);
        playerMgrGroup.AddEvent(playerSkillGroup);

        #endregion

        #region PlayerGroup -> PlayerMgrGroup -> EquipGroup

        GameEventGroup playerEquipGroup = new GameEventGroup(EVENT_PLAYER_EQUIP_GROUP);
        playerMgrGroup.AddEvent(playerEquipGroup);

        #endregion

        GameEvent onPlayerMoveSpeedChanged = new GameEvent(EVENT_ON_PLAYER_MOVE_SPEED_CHANGED);
        playerMgrGroup.AddEvent(onPlayerMoveSpeedChanged);

        GameEvent onPlayerAttackRangeChanged = new GameEvent(EVENT_ON_PLAYER_ATTACK_RANGE_CHANGED);
        playerMgrGroup.AddEvent(onPlayerAttackRangeChanged);

        GameEvent onPlayerRespawnTimeChanged = new GameEvent(EVENT_ON_PLAYER_RESPAWN_TIME_CHANGED);
        playerMgrGroup.AddEvent(onPlayerRespawnTimeChanged);

        GameEvent onPlayerRespawnCountDownStart = new GameEvent(EVENT_ON_PLAYER_RESPAWN_COUNTDOWN_START);
        playerMgrGroup.AddEvent(onPlayerRespawnCountDownStart);

        GameEvent onPlayerRespawnCountDownEnd = new GameEvent(EVENT_ON_PLAYER_RESPAWN_COUNTDOWN_END);
        playerMgrGroup.AddEvent(onPlayerRespawnCountDownEnd);

        #endregion

        #endregion
    }

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="check"></param>
    public static void RegisterEvent(string eventName,GameEvent.CheckHandle check)
    {
        var target = RootGroup?.GetEvent(eventName);
        if (target != null && target is GameEvent temp)
        {
            temp.AddCheckHandle(check);
        }
    }

    /// <summary>
    /// 订阅事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="response"></param>
    public static void SubscribeEvent(string eventName,GameEvent.ResponseHandle response)
    {
        var target = RootGroup.GetEvent(eventName);
        if (target != null && target is GameEvent temp)
        {
            temp.AddResponse(response);
        }
    }

    /// <summary>
    /// 启动或禁用事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="enable"></param>
    public static void EnableEvent(string eventName,bool enable)
    {
        var target = RootGroup.GetEvent(eventName);
        if (target != null)
        {
            target.Enable = enable;
        }
    }

    public static void EnableAllEvents(string eventName, bool enable)
    {
        var target = RootGroup?.GetEvent(eventName) as GameEventGroup;
        if (target != null)
        {
            target.EnableAllEvents(enable);
        }
    }

    public static void Update()
    {
        if (RootGroup== null) return;
        
        RootGroup.Update();
    }
}
