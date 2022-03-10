using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharManager : MonoBehaviour
{
    //public List<GameObject> playerGameObjectList;

    public List<CharBase> playerInfoList;

    int redCount = 0;
    int blueCount = 0;

    public static CharManager instance;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 生成一个新玩家时将玩家加入到列表中
    /// </summary>
    /// <param name="playerTeam"></param>
    /// <param name="player"></param>
    public CharBase AddPlayerToList(TeamEnum.playerTeam playerTeam, CharBase charBase)
    {
        switch (playerTeam)
        {
            case TeamEnum.playerTeam.Red:
                Log("Red");
                charBase.RunId = redCount;
                return charBase;
                //playerGameObjectList.Add(player);
            case TeamEnum.playerTeam.Blue:
                Log("Blue");
                charBase.RunId = blueCount + 5;
                return charBase;
                //playerGameObjectList.Add(player);
        }
        return null;
    }

    public void GetPlayerNameById(int runId)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }
        Log(runId, "名为"+charBase.PlayerName);
    }

    /// <summary>
    /// 通过id使特定玩家升级1级
    /// </summary>
    /// <param name="runId"></param>
    public void PlayerLevelUp(int runId,int count)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        for (int i = 0; i < count; i++)
        {
            charBase.Level += 1;
            charBase.Attack += 100;
            charBase.Health += 1000;
            charBase.MaxHealth += 1000;
            if (charBase.Exp >= charBase.MaxExp)
            {
                charBase.Exp -= charBase.MaxExp;
            }

            charBase.MaxExp += charBase.Level * 500;
            charBase.Restore += 20;
        }
        
        Log(runId, "level已经提升"+count+"级");
    }

    /// <summary>
    /// 通过id使玩家强制立即彻底死亡 【GM命令】
    /// </summary>
    /// <param name="runId"></param>
    public void PlayerDead(int runId)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }
        charBase.Health = 0;
        charBase.State = CharEnum.StateEnum.彻底死亡;
        Log(runId, "彻底死亡");
    }

    /// <summary>
    /// 通过id使玩家正常死亡
    /// </summary>
    /// <param name="runId"></param>
    public void PlayerKilled(int runId)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        charBase.Health = 0;
        for (int i = 0; i < charBase.Buff.Count; i++)
        {
            if (charBase.Buff[i].Equals(CharEnum.BuffEnum.无核))
            {
                charBase.State = CharEnum.StateEnum.彻底死亡;
                break;
            }
        }

        GetAllPlayer();
        PlayerRespawnCountDown(charBase.RunId, charBase.RespawnTime);
        charBase.State = CharEnum.StateEnum.复活中;
        
        Log(runId, "被击杀");
    }

    public void PlayerRespawnCountDown(int runId,float time)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        charBase.IsRespawn = true;
        Log(runId, "开始重生倒计时");
    }

    /// <summary>
    /// 不同于DebugConsole的SpawnPlayer函数只为测试,本函数为正常生成玩家的方式
    /// </summary>
    public void RespawnPlayer(int runId,string playerName)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        //CharSpawnController.instance.SpawnPlayer();
        charBase.IsRespawn = false;

    }

    /// <summary>
    /// 通过id修改玩家当前生命值
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="health"></param>
    public void SetPlayerHealth(int runId,int health)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        if (health>charBase.MaxHealth)
        {
            Log(runId,"预修改的值不得大于玩家最大生命");
            return;
        }
        if (health<0)
        {
            Log(runId, "预修改的值不得小于0");
            return;
        }
        charBase.Health = health;
        Log(runId, "血量已修改");
    }

    public void Log(int id,string text)
    {
        Debug.Log("id为" + id + "的玩家" + text);
    }

    public void Log(string text)
    {
        Debug.Log(text);
    }

    /// <summary>
    /// 通过ID获取特定玩家的组件,适用于将要进行特定属性进行修改时
    /// </summary>
    /// <param name="runId"></param>
    /// <returns></returns>
    public CharBase FindPlayerById(int runId)
    {
        if (runId >= 10)
        {
            Debug.Log("runId只能为0~9");
            return null;
        }

        for (int i = 0; i < playerInfoList.Count; i++)
        {
            if (playerInfoList[i].GetComponent<CharBase>().RunId == runId)
            {
                return playerInfoList[i].GetComponent<CharBase>();
            }
        }
        Log("未找到该id的玩家");
        return null;
    }

    /// <summary>
    /// 获取所有玩家的全部信息
    /// </summary>
    /// <returns></returns>
    public List<CharBase> GetAllPlayer()
    {
        CharBase charBaseComponent = new CharBase();
        playerInfoList.Clear();

        foreach (var item in playerInfoList)
        {
            charBaseComponent = item.GetComponent<CharBase>();
            playerInfoList.Add(charBaseComponent);
        }

        return playerInfoList;
    }

    /// <summary>
    /// 根据CharBase组件获取所有信息
    /// </summary>
    /// <param name="charBase"></param>
    /// <param name="component"></param>
    /// <returns></returns>
    public void GetPlayerInfo(CharBase charBase,CharBase component)
    {
        //charBase = component;

        //charBase.RunId = component.RunId;
        //charBase.PlayerName = component.PlayerName;
        //charBase.Kill = component.Kill;
        //charBase.Death = component.Death;
        //charBase.Money = component.Money;
        //charBase.Exp = component.Exp;
        //charBase.MaxExp = component.MaxExp;
        //charBase.State = component.State;
        //charBase.Buff = component.Buff;
        //charBase.Pro = component.Pro;
        //charBase.Level = component.Level;
        //charBase.Attack = component.Attack;
        //charBase.MaxHealth = component.MaxHealth;
        //charBase.Health = component.Health;
        //charBase.CriticalHit = component.CriticalHit;
        //charBase.CriticalHitRate = component.CriticalHitRate;
        //charBase.Defence = component.Defence;
        //charBase.AttackSpeed = component.AttackSpeed;
        //charBase.Restore = component.Restore;
        //charBase.SkillQ = component.SkillQ;
        //charBase.SkillE = component.SkillE;
        //charBase.SkillR = component.SkillR;
        //charBase.SkillBurst = component.SkillBurst;
        //charBase.HeadID = component.HeadID;
        //charBase.ArmorID = component.ArmorID;
        //charBase.HeadID = component.HeadID;
        //charBase.KneeID = component.KneeID;
        //charBase.TrousersID = component.TrousersID;
        //charBase.ShoesID = component.ShoesID;
        //charBase.MoveSpeed = component.MoveSpeed;
        //charBase.AttackRange = component.AttackRange;
        //charBase.RespawnTime = component.RespawnTime;
        //charBase.RespawnCountDown = component.RespawnCountDown;
        //charBase.IsRespawn = component.IsRespawn;

        return;
    }


}
