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
            charBase.MaxHealth += 1000;
            charBase.Health += 1000;
            
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

        //GetAllPlayer();
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
        if (runId >= 10 || runId < 0)
        {
            Debug.Log("runIdֻ��Ϊ0~9");
            return null;
        }

        return playerCameraList[runId].GetComponent<CharBase>();

        //for (int i = 0; i < playerInfoList.Count; i++)
        //{
        //    if (playerInfoList[i].RunId == runId)
        //    {
        //        return playerInfoList[i];
        //    }
        //}
        //Log("δ�ҵ���id�����");
        //return null;
    }

    /// <summary>
    /// ��ȡ������ҵ�ȫ����Ϣ
    /// </summary>
    /// <returns></returns>
    //public List<CharBase> GetAllPlayer()
    //{
    //    CharBase charBaseComponent = new CharBase();
    //    playerInfoList.Clear();

    //    foreach (var item in playerInfoList)
    //    {
    //        charBaseComponent = item.GetComponent<CharBase>();
    //        playerInfoList.Add(charBaseComponent);
    //    }

    //    return playerInfoList;
    //}

    /// <summary>
    /// ����CharBase�����ȡ������Ϣ
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
        needTarget.IsRespawn = provider.IsRespawn;

        return;
    }


}
