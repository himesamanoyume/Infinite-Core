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

        EnableAllEvents(EventName.playerGroup, true);
        EnableAllEvents(EventName.systemGroup, true);
    }

    private static void Init()
    {
        RootGroup = new GameEventGroup(EventName.rootGroup);

        //于此处添加事件组和事件 事件组命名最后必须有Group

        #region SystemGroup
        GameEventGroup systemGroup = new GameEventGroup(EventName.systemGroup);
        RootGroup.AddEvent(systemGroup);

        GameEvent onToast = new GameEvent(EventName.onToast);
        systemGroup.AddEvent(onToast);
        #endregion

        GameEventGroup playerGroup = new GameEventGroup(EventName.playerGroup);
        RootGroup.AddEvent(playerGroup);
        #region PlayerGroup

        GameEventGroup playerMgrGroup = new GameEventGroup(EventName.playerMgrGroup);
        playerGroup.AddEvent(playerMgrGroup);
        #region PlayerGroup -> PlayerMgrGroup

        GameEvent onPlayerActorNumberChanged = new GameEvent(EventName.onPlayerActorNumberChanged);
        playerMgrGroup.AddEvent(onPlayerActorNumberChanged);

        GameEvent onPlayerNameChanged = new GameEvent(EventName.onPlayerNameChanged);
        playerMgrGroup.AddEvent(onPlayerNameChanged);

        GameEvent onPlayerTeamChanged = new GameEvent(EventName.onPlayerTeamChanged);
        playerMgrGroup.AddEvent(onPlayerTeamChanged);

        GameEvent onPlayerKill = new GameEvent(EventName.onPlayerKill);
        playerMgrGroup.AddEvent(onPlayerKill);

        GameEvent onPlayerKilled = new GameEvent(EventName.onPlayerKilled);
        playerMgrGroup.AddEvent(onPlayerKilled);

        GameEvent onPlayerDead = new GameEvent(EventName.onPlayerDead);
        playerMgrGroup.AddEvent(onPlayerDead);

        GameEvent onPlayerRespawn = new GameEvent(EventName.onPlayerRespawn);
        playerMgrGroup.AddEvent(onPlayerRespawn);

        GameEvent onPlayerRespawning = new GameEvent(EventName.onPlayerRespawning);
        playerMgrGroup.AddEvent(onPlayerRespawning);

        GameEvent onPlayerMoneyChanged = new GameEvent(EventName.onPlayerMoneyChanged);
        playerMgrGroup.AddEvent(onPlayerMoneyChanged);

        GameEvent onPlayerCurrentExpChanged = new GameEvent(EventName.onPlayerCurrentExpChanged);
        playerMgrGroup.AddEvent(onPlayerCurrentExpChanged);

        GameEvent onPlayerMaxExpChanged = new GameEvent(EventName.onPlayerMaxExpChanged);
        playerMgrGroup.AddEvent(onPlayerMaxExpChanged);

        GameEvent onPlayerStateChanged = new GameEvent(EventName.onPlayerStateChanged);
        playerMgrGroup.AddEvent(onPlayerStateChanged);

        GameEvent onPlayerBuffChanged = new GameEvent(EventName.onPlayerBuffChanged);
        playerMgrGroup.AddEvent(onPlayerBuffChanged);

        GameEvent onPlayerLevelChanged = new GameEvent(EventName.onPlayerLevelChanged);
        playerMgrGroup.AddEvent(onPlayerLevelChanged);

        GameEvent onPlayerLevelUp = new GameEvent(EventName.onPlayerLevelUp);
        playerMgrGroup.AddEvent(onPlayerLevelUp);

        GameEvent onPlayerAttackChanged = new GameEvent(EventName.onPlayerAttackChanged);
        playerMgrGroup.AddEvent(onPlayerAttackChanged);

        GameEvent onPlayerShieldChanged = new GameEvent(EventName.onPlayerShieldChanged);
        playerMgrGroup.AddEvent(onPlayerShieldChanged);

        GameEvent onPlayerMaxHealthChanged = new GameEvent(EventName.onPlayerMaxHealthChanged);
        playerMgrGroup.AddEvent(onPlayerMaxHealthChanged);

        GameEvent onPlayerCurrentHealthChanged = new GameEvent(EventName.onPlayerCurrentHealthChanged);
        playerMgrGroup.AddEvent(onPlayerCurrentHealthChanged);

        GameEvent onPlayerCriticalHitChanged = new GameEvent(EventName.onPlayerCriticalHitChanged);
        playerMgrGroup.AddEvent(onPlayerCriticalHitChanged);

        GameEvent onPlayerCriticalHitRateChanged = new GameEvent(EventName.onPlayerCriticalHitRateChanged);
        playerMgrGroup.AddEvent(onPlayerCriticalHitRateChanged);

        GameEvent onPlayerDefenceChanged = new GameEvent(EventName.onPlayerDefenceChanged);
        playerMgrGroup.AddEvent(onPlayerDefenceChanged);

        GameEvent onPlayerAttackSpeedChanged = new GameEvent(EventName.onPlayerAttackSpeedChanged);
        playerMgrGroup.AddEvent(onPlayerAttackSpeedChanged);

        GameEvent onPlayerRestoreChanged = new GameEvent(EventName.onPlayerRestoreChanged);
        playerMgrGroup.AddEvent(onPlayerRestoreChanged);

        

        GameEventGroup playerSkillGroup = new GameEventGroup(EventName.playerSkillGroup);
        playerMgrGroup.AddEvent(playerSkillGroup);
        #region PlayerGroup -> PlayerMgrGroup -> SkillGroup

        #endregion

        GameEventGroup playerEquipGroup = new GameEventGroup(EventName.playerEquipGroup);
        playerMgrGroup.AddEvent(playerEquipGroup);
        #region PlayerGroup -> PlayerMgrGroup -> EquipGroup

        #endregion

        GameEvent onPlayerMoveSpeedChanged = new GameEvent(EventName.onPlayerMoveSpeedChanged);
        playerMgrGroup.AddEvent(onPlayerMoveSpeedChanged);

        GameEvent onPlayerAttackRangeChanged = new GameEvent(EventName.onPlayerAttackRangeChanged);
        playerMgrGroup.AddEvent(onPlayerAttackRangeChanged);

        GameEvent onPlayerRespawnTimeChanged = new GameEvent(EventName.onPlayerRespawnTimeChanged);
        playerMgrGroup.AddEvent(onPlayerRespawnTimeChanged);

        GameEvent onPlayerRespawnCountDownStart = new GameEvent(EventName.onPlayerRespawnCountDownStart);
        playerMgrGroup.AddEvent(onPlayerRespawnCountDownStart);

        GameEvent onPlayerRespawnCountDownEnd = new GameEvent(EventName.onPlayerRespawnCountDownEnd);
        playerMgrGroup.AddEvent(onPlayerRespawnCountDownEnd);

        #endregion

        #endregion
    }

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="check"></param>
    public static void RegisterEvent(EventName eventName,GameEvent.CheckHandle check)
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
    public static void SubscribeEvent(EventName eventName,GameEvent.ResponseHandle response)
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
    public static void EnableEvent(EventName eventName,bool enable)
    {
        if (eventName == EventName.rootGroup) Debug.LogWarning("RootGroup不应使用此方法 无效启用");
        var target = RootGroup.GetEvent(eventName);
        if (target != null)
        {
            target.Enable = enable;
        }
    }

    public static void EnableAllEvents(EventName eventName, bool enable)
    {
        if (eventName == EventName.rootGroup) Debug.LogWarning("RootGroup不应使用此方法 无效启用");
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
