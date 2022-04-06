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

    }

    private static void Init()
    {
        RootGroup = new GameEventGroup("RootGroup");

        //于此处添加事件组和事件

        #region PlayerGroup
        GameEventGroup playerGroup = new GameEventGroup("PlayerGroup");
        RootGroup.AddEvent(playerGroup);

        #region PlayerGroup -> PlayerMgrGroup
        GameEventGroup playerMgrGroup = new GameEventGroup("PlayerMgrGroup");
        playerGroup.AddEvent(playerMgrGroup);

        GameEvent onPlayerActorNumberChanged = new GameEvent("OnPlayerActorNumberChanged");
        playerMgrGroup.AddEvent(onPlayerActorNumberChanged);

        GameEvent onPlayerNameChanged = new GameEvent("OnPlayerNameChanged");
        playerMgrGroup.AddEvent(onPlayerNameChanged);

        GameEvent onPlayerTeamChanged = new GameEvent("OnPlayerTeamChanged");
        playerMgrGroup.AddEvent(onPlayerTeamChanged);

        GameEvent onPlayerKill = new GameEvent("OnPlayerKill");
        playerMgrGroup.AddEvent(onPlayerKill);

        GameEvent onPlayerKilled = new GameEvent("OnPlayerKilled");
        playerMgrGroup.AddEvent(onPlayerKilled);

        GameEvent onPlayerDead = new GameEvent("OnPlayerDead");
        playerMgrGroup.AddEvent(onPlayerDead);

        GameEvent onPlayerRespawn = new GameEvent("OnPlayerRespawn");
        playerMgrGroup.AddEvent(onPlayerRespawn);

        GameEvent onPlayerMoneyChanged = new GameEvent("OnPlayerMoneyChanged");
        playerMgrGroup.AddEvent(onPlayerMoneyChanged);

        GameEvent onPlayerCurrentExpChanged = new GameEvent("OnPlayerCurrentExpChanged");
        playerMgrGroup.AddEvent(onPlayerCurrentExpChanged);

        GameEvent onPlayerMaxExpChanged = new GameEvent("OnPlayerMaxExpChanged");
        playerMgrGroup.AddEvent(onPlayerMaxExpChanged);

        GameEvent onPlayerStateChanged = new GameEvent("OnPlayerStateChanged");
        playerMgrGroup.AddEvent(onPlayerStateChanged);

        GameEvent onPlayerBuffChanged = new GameEvent("OnPlayerBuffChanged");
        playerMgrGroup.AddEvent(onPlayerBuffChanged);

        GameEvent onPlayerLevelChanged = new GameEvent("OnPlayerLevelChanged");
        playerMgrGroup.AddEvent(onPlayerLevelChanged);

        GameEvent onPlayerAttackChanged = new GameEvent("OnPlayerAttackChanged");
        playerMgrGroup.AddEvent(onPlayerAttackChanged);

        GameEvent onPlayerShieldChanged = new GameEvent("OnPlayerShieldChanged");
        playerMgrGroup.AddEvent(onPlayerShieldChanged);

        GameEvent onPlayerMaxHealthChanged = new GameEvent("OnPlayerMaxHealthChanged");
        playerMgrGroup.AddEvent(onPlayerMaxHealthChanged);

        GameEvent onPlayerCurrentHealthChanged = new GameEvent("OnPlayerCurrentHealthChanged");
        playerMgrGroup.AddEvent(onPlayerCurrentHealthChanged);

        GameEvent onPlayerCriticalHitChanged = new GameEvent("OnPlayerCriticalHitChanged");
        playerMgrGroup.AddEvent(onPlayerCriticalHitChanged);

        GameEvent onPlayerCriticalHitRateChanged = new GameEvent("OnPlayerCriticalHitRateChanged");
        playerMgrGroup.AddEvent(onPlayerCriticalHitRateChanged);

        GameEvent onPlayerDefenceChanged = new GameEvent("OnPlayerDefenceChanged");
        playerMgrGroup.AddEvent(onPlayerDefenceChanged);

        GameEvent onPlayerAttackSpeedChanged = new GameEvent("OnPlayerAttackSpeedChanged");
        playerMgrGroup.AddEvent(onPlayerAttackSpeedChanged);

        GameEvent onPlayerRestoreChanged = new GameEvent("OnPlayerRestoreChanged");
        playerMgrGroup.AddEvent(onPlayerRestoreChanged);

        #region PlayerGroup -> PlayerMgrGroup -> SkillGroup

        GameEventGroup playerSkillGroup = new GameEventGroup("PlayerSkillGroup");
        playerMgrGroup.AddEvent(playerSkillGroup);

        #endregion

        #region PlayerGroup -> PlayerMgrGroup -> EquipGroup

        GameEventGroup playerEquipGroup = new GameEventGroup("PlayerEquipGroup");
        playerMgrGroup.AddEvent(playerEquipGroup);

        #endregion

        GameEvent onPlayerMoveSpeedChanged = new GameEvent("OnPlayerMoveSpeedChanged");
        playerMgrGroup.AddEvent(onPlayerMoveSpeedChanged);

        GameEvent onPlayerAttackRangeChanged = new GameEvent("OnPlayerAttackRangeChanged");
        playerMgrGroup.AddEvent(onPlayerAttackRangeChanged);

        GameEvent onPlayerRespawnTimeChanged = new GameEvent("OnPlayerRespawnTimeChanged");
        playerMgrGroup.AddEvent(onPlayerRespawnTimeChanged);

        GameEvent onPlayerRespawnCountDownStart = new GameEvent("OnPlayerRespawnCountDownStart");
        playerMgrGroup.AddEvent(onPlayerRespawnCountDownStart);

        GameEvent onPlayerRespawnCountDownEnd = new GameEvent("OnPlayerRespawnCountDownEnd");
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
