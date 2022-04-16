using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
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

        EnableEvent(EventEnum.SystemGroup, true);
        EnableEvent(EventEnum.PlayerGroup, true);
        EnableEvent(EventEnum.PlayerMgrGroup, true);
        EnableAllEvents(EventEnum.CharMgrGroup, true);
        EnableAllEvents(EventEnum.PlayerControlGroup, true);
        

    }

    private static void Init()
    {
        Debug.LogWarning("GameEventManager Init");
        RootGroup = new GameEventGroup(EventEnum.rootGroup);

        //于此处添加事件组和事件 事件组命名最后必须有Group

        #region SystemGroup
        GameEventGroup systemGroup = new GameEventGroup(EventEnum.SystemGroup);
        RootGroup.AddEvent(systemGroup);

        GameEvent onPlayerLeftRoom = new GameEvent(EventEnum.OnPlayerLeftRoom);
        systemGroup.AddEvent(onPlayerLeftRoom);

        GameEvent onToast = new GameEvent(EventEnum.OnToast);
        systemGroup.AddEvent(onToast);
        #endregion

        GameEventGroup charMgrGroup = new GameEventGroup(EventEnum.CharMgrGroup);
        RootGroup.AddEvent(charMgrGroup);
        #region CharMgrGroup

        GameEvent AllowGetPlayerInfoBarList = new GameEvent(EventEnum.AllowGetPlayerInfoBarList);
        charMgrGroup.AddEvent(AllowGetPlayerInfoBarList);

        GameEvent AllowGetPlayerModelList = new GameEvent(EventEnum.AllowGetPlayerModelList);
        charMgrGroup.AddEvent(AllowGetPlayerModelList);

        GameEvent AllowGetRecorderList = new GameEvent (EventEnum.AllowGetRecorderList);
        charMgrGroup.AddEvent(AllowGetRecorderList);

        #endregion

        GameEventGroup playerGroup = new GameEventGroup(EventEnum.PlayerGroup);
        RootGroup.AddEvent(playerGroup);
        #region PlayerGroup

        GameEventGroup playerMgrGroup = new GameEventGroup(EventEnum.PlayerMgrGroup);
        playerGroup.AddEvent(playerMgrGroup);

        #region PlayerGroup -> PlayerMgrGroup

        GameEvent OnPlayerFinalDamageChanged = new GameEvent(EventEnum.OnPlayerFinalDamageChanged);
        playerMgrGroup.AddEvent(OnPlayerFinalDamageChanged);

        GameEvent OnPlayerActorNumberChanged = new GameEvent(EventEnum.OnPlayerActorNumberChanged);
        playerMgrGroup.AddEvent(OnPlayerActorNumberChanged);

        GameEvent OnPlayerNameChanged = new GameEvent(EventEnum.OnPlayerNameChanged);
        playerMgrGroup.AddEvent(OnPlayerNameChanged);

        GameEvent OnPlayerTeamChanged = new GameEvent(EventEnum.OnPlayerTeamChanged);
        playerMgrGroup.AddEvent(OnPlayerTeamChanged);

        GameEvent OnPlayerKill = new GameEvent(EventEnum.OnPlayerKill);
        playerMgrGroup.AddEvent(OnPlayerKill);

        GameEvent OnPlayerKilled = new GameEvent(EventEnum.OnPlayerKilled);
        playerMgrGroup.AddEvent(OnPlayerKilled);

        GameEvent OnPlayerDead = new GameEvent(EventEnum.OnPlayerDead);
        playerMgrGroup.AddEvent(OnPlayerDead);

        GameEvent OnPlayerRespawn = new GameEvent(EventEnum.OnPlayerRespawn);
        playerMgrGroup.AddEvent(OnPlayerRespawn);

        GameEvent OnPlayerRespawning = new GameEvent(EventEnum.OnPlayerRespawning);
        playerMgrGroup.AddEvent(OnPlayerRespawning);

        GameEvent OnPlayerMoneyChanged = new GameEvent(EventEnum.OnPlayerMoneyChanged);
        playerMgrGroup.AddEvent(OnPlayerMoneyChanged);

        GameEvent OnPlayerCurrentExpChanged = new GameEvent(EventEnum.OnPlayerCurrentExpChanged);
        playerMgrGroup.AddEvent(OnPlayerCurrentExpChanged);

        GameEvent OnPlayerMaxExpChanged = new GameEvent(EventEnum.OnPlayerMaxExpChanged);
        playerMgrGroup.AddEvent(OnPlayerMaxExpChanged);

        GameEvent OnPlayerStateChanged = new GameEvent(EventEnum.OnPlayerStateChanged);
        playerMgrGroup.AddEvent(OnPlayerStateChanged);

        GameEvent OnPlayerBuffChanged = new GameEvent(EventEnum.OnPlayerBuffChanged);
        playerMgrGroup.AddEvent(OnPlayerBuffChanged);

        GameEvent OnPlayerLevelChanged = new GameEvent(EventEnum.OnPlayerLevelChanged);
        playerMgrGroup.AddEvent(OnPlayerLevelChanged);

        GameEvent OnPlayerLevelUp = new GameEvent(EventEnum.OnPlayerLevelUp);
        playerMgrGroup.AddEvent(OnPlayerLevelUp);

        GameEvent OnPlayerAttackChanged = new GameEvent(EventEnum.OnPlayerAttackChanged);
        playerMgrGroup.AddEvent(OnPlayerAttackChanged);

        GameEvent OnPlayerShieldChanged = new GameEvent(EventEnum.OnPlayerShieldChanged);
        playerMgrGroup.AddEvent(OnPlayerShieldChanged);

        GameEvent OnPlayerMaxHealthChanged = new GameEvent(EventEnum.OnPlayerMaxHealthChanged);
        playerMgrGroup.AddEvent(OnPlayerMaxHealthChanged);

        GameEvent OnPlayerCurrentHealthChanged = new GameEvent(EventEnum.OnPlayerCurrentHealthChanged);
        playerMgrGroup.AddEvent(OnPlayerCurrentHealthChanged);

        GameEvent OnPlayerCriticalHitChanged = new GameEvent(EventEnum.OnPlayerCriticalHitChanged);
        playerMgrGroup.AddEvent(OnPlayerCriticalHitChanged);

        GameEvent OnPlayerCriticalHitRateChanged = new GameEvent(EventEnum.OnPlayerCriticalHitRateChanged);
        playerMgrGroup.AddEvent(OnPlayerCriticalHitRateChanged);

        GameEvent OnPlayerDefenceChanged = new GameEvent(EventEnum.OnPlayerDefenceChanged);
        playerMgrGroup.AddEvent(OnPlayerDefenceChanged);

        GameEvent OnPlayerAttackSpeedChanged = new GameEvent(EventEnum.OnPlayerAttackSpeedChanged);
        playerMgrGroup.AddEvent(OnPlayerAttackSpeedChanged);

        GameEvent OnPlayerRestoreChanged = new GameEvent(EventEnum.OnPlayerRestoreChanged);
        playerMgrGroup.AddEvent(OnPlayerRestoreChanged);

        GameEvent OnPlayerRestoreing = new GameEvent(EventEnum.OnPlayerRestoreing);
        playerMgrGroup.AddEvent(OnPlayerRestoreing);

        GameEventGroup playerSkillGroup = new GameEventGroup(EventEnum.PlayerSkillGroup);
        playerMgrGroup.AddEvent(playerSkillGroup);
        #region PlayerGroup -> PlayerMgrGroup -> SkillGroup

        #endregion

        GameEventGroup playerEquipGroup = new GameEventGroup(EventEnum.PlayerEquipGroup);
        playerMgrGroup.AddEvent(playerEquipGroup);
        #region PlayerGroup -> PlayerMgrGroup -> EquipGroup

        #endregion

        GameEvent OnPlayerMoveSpeedChanged = new GameEvent(EventEnum.OnPlayerMoveSpeedChanged);
        playerMgrGroup.AddEvent(OnPlayerMoveSpeedChanged);

        GameEvent OnPlayerAttackRangeChanged = new GameEvent(EventEnum.OnPlayerAttackRangeChanged);
        playerMgrGroup.AddEvent(OnPlayerAttackRangeChanged);

        GameEvent OnPlayerRespawnTimeChanged = new GameEvent(EventEnum.OnPlayerRespawnTimeChanged);
        playerMgrGroup.AddEvent(OnPlayerRespawnTimeChanged);

        GameEvent OnPlayerRespawnCountDownStart = new GameEvent(EventEnum.OnPlayerRespawnCountDownStart);
        playerMgrGroup.AddEvent(OnPlayerRespawnCountDownStart);

        GameEvent OnPlayerRespawnCountDownEnd = new GameEvent(EventEnum.OnPlayerRespawnCountDownEnd);
        playerMgrGroup.AddEvent(OnPlayerRespawnCountDownEnd);

        GameEvent onPlayerDamaged = new GameEvent(EventEnum.OnPlayerDamaged);
        playerMgrGroup.AddEvent(onPlayerDamaged);

        #endregion

        GameEventGroup playerControlGroup = new GameEventGroup(EventEnum.PlayerControlGroup);
        playerGroup.AddEvent(playerControlGroup);

        #region PlayerGroup -> PlayerControlGroup

        GameEvent allowPlayerTowardChanged = new GameEvent(EventEnum.AllowPlayerTowardChanged);
        playerControlGroup.AddEvent(allowPlayerTowardChanged);

        GameEvent allowPlayerMove = new GameEvent(EventEnum.AllowPlayerMove);
        playerControlGroup.AddEvent(allowPlayerMove);

        GameEvent sendPlayerMoveSpeed = new GameEvent(EventEnum.SendPlayerMoveSpeed);
        playerControlGroup.AddEvent(sendPlayerMoveSpeed);

        GameEvent allowPlayerAttack = new GameEvent(EventEnum.AllowPlayerAttack);
        playerControlGroup.AddEvent(allowPlayerAttack);

        #endregion

        #endregion
    }

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="check"></param>
    public static void RegisterEvent(EventEnum eventName,GameEvent.CheckHandle check)
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
    public static void SubscribeEvent(EventEnum eventName,GameEvent.ResponseHandle response)
    {
        var target = RootGroup.GetEvent(eventName);
        if (target != null && target is GameEvent temp)
        {
            temp.AddResponse(response);
        }
    }

    /// <summary>
    /// 取消订阅事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="response"></param>
    public static void UnsubscribeEvent(EventEnum eventName, GameEvent.ResponseHandle response)
    {
        var target = RootGroup.GetEvent(eventName);
        if (target != null && target is GameEvent temp)
        {
            temp.RemoveResponse(response);
        }
    }

    /// <summary>
    /// 启动或禁用事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="enable"></param>
    public static void EnableEvent(EventEnum eventName,bool enable)
    {
        if (eventName == EventEnum.rootGroup) Debug.LogWarning("RootGroup不应使用此方法 无效启用");
        var target = RootGroup.GetEvent(eventName);
        if (target != null)
        {
            target.Enable = enable;
            if (target.Name != EventEnum.OnToast)
            {
                Debug.Log(target.Name + " " + enable);
            }
            
        }
    }

    public static void EnableAllEvents(EventEnum eventName, bool enable)
    {
        if (eventName == EventEnum.rootGroup) Debug.LogWarning("RootGroup不应使用此方法 无效启用");
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
