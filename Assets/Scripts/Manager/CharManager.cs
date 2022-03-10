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
    /// ����һ�������ʱ����Ҽ��뵽�б���
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
        Log(runId, "��Ϊ"+charBase.PlayerName);
    }

    /// <summary>
    /// ͨ��idʹ�ض��������1��
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
        
        Log(runId, "level�Ѿ�����"+count+"��");
    }

    /// <summary>
    /// ͨ��idʹ���ǿ�������������� ��GM���
    /// </summary>
    /// <param name="runId"></param>
    public void PlayerDead(int runId)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }
        charBase.Health = 0;
        charBase.State = CharEnum.StateEnum.��������;
        Log(runId, "��������");
    }

    /// <summary>
    /// ͨ��idʹ�����������
    /// </summary>
    /// <param name="runId"></param>
    public void PlayerKilled(int runId)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        charBase.Health = 0;
        for (int i = 0; i < charBase.Buff.Count; i++)
        {
            if (charBase.Buff[i].Equals(CharEnum.BuffEnum.�޺�))
            {
                charBase.State = CharEnum.StateEnum.��������;
                break;
            }
        }

        GetAllPlayer();
        PlayerRespawnCountDown(charBase.RunId, charBase.RespawnTime);
        charBase.State = CharEnum.StateEnum.������;
        
        Log(runId, "����ɱ");
    }

    public void PlayerRespawnCountDown(int runId,float time)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        charBase.IsRespawn = true;
        Log(runId, "��ʼ��������ʱ");
    }

    /// <summary>
    /// ��ͬ��DebugConsole��SpawnPlayer����ֻΪ����,������Ϊ����������ҵķ�ʽ
    /// </summary>
    public void RespawnPlayer(int runId,string playerName)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        //CharSpawnController.instance.SpawnPlayer();
        charBase.IsRespawn = false;

    }

    /// <summary>
    /// ͨ��id�޸���ҵ�ǰ����ֵ
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="health"></param>
    public void SetPlayerHealth(int runId,int health)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        if (health>charBase.MaxHealth)
        {
            Log(runId,"Ԥ�޸ĵ�ֵ���ô�������������");
            return;
        }
        if (health<0)
        {
            Log(runId, "Ԥ�޸ĵ�ֵ����С��0");
            return;
        }
        charBase.Health = health;
        Log(runId, "Ѫ�����޸�");
    }

    public void Log(int id,string text)
    {
        Debug.Log("idΪ" + id + "�����" + text);
    }

    public void Log(string text)
    {
        Debug.Log(text);
    }

    /// <summary>
    /// ͨ��ID��ȡ�ض���ҵ����,�����ڽ�Ҫ�����ض����Խ����޸�ʱ
    /// </summary>
    /// <param name="runId"></param>
    /// <returns></returns>
    public CharBase FindPlayerById(int runId)
    {
        if (runId >= 10)
        {
            Debug.Log("runIdֻ��Ϊ0~9");
            return null;
        }

        for (int i = 0; i < playerInfoList.Count; i++)
        {
            if (playerInfoList[i].GetComponent<CharBase>().RunId == runId)
            {
                return playerInfoList[i].GetComponent<CharBase>();
            }
        }
        Log("δ�ҵ���id�����");
        return null;
    }

    /// <summary>
    /// ��ȡ������ҵ�ȫ����Ϣ
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
    /// ����CharBase�����ȡ������Ϣ
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
