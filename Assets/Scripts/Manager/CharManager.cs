using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharManager : MonoBehaviour
{
    //public List<GameObject> playerGameObjectList;

    public GameObject[] playerCameraList = new GameObject[10];

    

    public static CharManager instance;

    private void Awake()
    {
        instance = this;
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
            charBase.MaxHealth += 1000;
            charBase.Health += 1000;
            
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
        //------------------------
        //或为联机代码改动部分
        GameObject charCamera = FindPlayerCameraById(runId);
        FindChildObjWithTag("PlayerModel", charCamera).SetActive(false);

        //------------------------
        if (charBase.Buff == null)
        {

        }
        else
        {
            for (int i = 0; i < charBase.Buff.Count; i++)
            {
                if (charBase.Buff[i].Equals(CharEnum.BuffEnum.无核))
                {
                    charBase.State = CharEnum.StateEnum.彻底死亡;
                    break;
                }
            }
        }

        PlayerRespawnCountDown(charBase.RunId);
        charBase.State = CharEnum.StateEnum.复活中;
        
        Log(runId, "被击杀");
    }

    /// <summary>
    /// 玩家重生倒计时
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="time"></param>
    public void PlayerRespawnCountDown(int runId)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        charBase.State = CharEnum.StateEnum.复活中;
        charBase.RespawnCountDown = charBase.RespawnTime;
        Log(runId, "开始重生倒计时");
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
        if (runId >= 10 || runId < 0)
        {
            Debug.Log("runId只能为0~9");
            return null;
        }

        return playerCameraList[runId].GetComponent<CharBase>();
    }

    public GameObject FindPlayerCameraById(int runId)
    {
        if (runId >= 10 || runId < 0)
        {
            Debug.Log("runId只能为0~9");
            return null;
        }
        return playerCameraList[runId];
    }

    public GameObject FindChildObjWithTag(string tag,GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
        }
        return null;
    }

    /// <summary>
    /// 根据CharBase组件获取所有信息
    /// </summary>
    /// <param name="needTarget"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public void GetPlayerInfo(CharBase needTarget,CharBase provider)
    {
        //charBase = component;

        needTarget.RunId = provider.RunId;
        needTarget.PlayerName = provider.PlayerName;
        needTarget.Kill = provider.Kill;
        needTarget.Death = provider.Death;
        needTarget.Money = provider.Money;
        needTarget.Exp = provider.Exp;
        needTarget.MaxExp = provider.MaxExp;
        needTarget.State = provider.State;
        needTarget.Buff = provider.Buff;
        needTarget.Pro = provider.Pro;
        needTarget.Level = provider.Level;
        needTarget.Attack = provider.Attack;
        needTarget.MaxHealth = provider.MaxHealth;
        needTarget.Health = provider.Health;
        needTarget.CriticalHit = provider.CriticalHit;
        needTarget.CriticalHitRate = provider.CriticalHitRate;
        needTarget.Defence = provider.Defence;
        needTarget.AttackSpeed = provider.AttackSpeed;
        needTarget.Restore = provider.Restore;
        needTarget.SkillQ = provider.SkillQ;
        needTarget.SkillE = provider.SkillE;
        needTarget.SkillR = provider.SkillR;
        needTarget.SkillBurst = provider.SkillBurst;
        needTarget.HeadID = provider.HeadID;
        needTarget.ArmorID = provider.ArmorID;
        needTarget.HeadID = provider.HeadID;
        needTarget.KneeID = provider.KneeID;
        needTarget.TrousersID = provider.TrousersID;
        needTarget.ShoesID = provider.ShoesID;
        needTarget.MoveSpeed = provider.MoveSpeed;
        needTarget.AttackRange = provider.AttackRange;
        needTarget.RespawnTime = provider.RespawnTime;
        needTarget.RespawnCountDown = provider.RespawnCountDown;

        return;
    }


}
