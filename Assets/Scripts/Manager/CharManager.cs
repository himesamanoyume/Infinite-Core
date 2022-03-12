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
            charBase.Restore += 10;
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
        charBase.Death++;
        //------------------------
        //��Ϊ��������Ķ�����
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
                if (charBase.Buff[i].Equals(CharEnum.BuffEnum.�޺�))
                {
                    charBase.State = CharEnum.StateEnum.��������;
                    break;
                }
            }
        }
        Log(runId, "����ɱ");
        PlayerRespawnCountDown(charBase.RunId);
        charBase.State = CharEnum.StateEnum.������;
        
        
    }

    /// <summary>
    /// �����������ʱ
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="time"></param>
    public void PlayerRespawnCountDown(int runId)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        charBase.State = CharEnum.StateEnum.������;
        charBase.RespawnCountDown = charBase.RespawnTime;
        Log(runId, "��ʼ��������ʱ");
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

    /// <summary>
    /// ����Ҿ���
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="minExp"></param>
    /// <param name="maxExp"></param>
    public void GetExp(int runId,float minExp,float maxExp)
    {
        if (minExp< 0 || maxExp <0)
        {
            Log(runId, "�ľ���ֵ�䶯����Ϊ����");
            return;
        }

        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        float currentExp = ToGivePlayerSomething(runId, minExp, maxExp);
        charBase.Exp += currentExp;

        Log(runId, "�����"+currentExp+"����");
    }

    /// <summary>
    /// ����/�۳���ҽ�Ǯ ���ֵ����Сֵ��Ϊ���� ��Ϊ��Ǯ
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="minMoney"></param>
    /// <param name="maxMoney"></param>
    public void GetMoney(int runId, int minMoney, int maxMoney)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        int currentMoney = ToGivePlayerSomething(runId, minMoney, maxMoney);
        charBase.Money += currentMoney;

        if (currentMoney>=0)
        {
            Log(runId, "�����" + currentMoney + "��Ǯ");
        }
        else
        {
            Log(runId, "�۳���" + currentMoney + "��Ǯ");
        }
    }

    // --------------------------------------

    /// <summary>
    /// ��һ�ȡ����,��ͨ��װ�����������ֵ��üӳ�ʱͨ�ú���
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="lowerLimit"></param>
    /// <param name="upperLimit"></param>
    /// <returns>����ֵ���ڸ�װ����ʾ������ֵ</returns>
    public float ToGivePlayerSomething(int runId,float lowerLimit,float upperLimit)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return 0; }

        float value = Random.Range(lowerLimit, upperLimit);
        return value;
    }
    public int ToGivePlayerSomething(int runId, int lowerLimit, int upperLimit)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return 0; }

        int value = Random.Range(lowerLimit, upperLimit);
        return value;
    }
    //----------------------------------

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
    }

    public GameObject FindPlayerCameraById(int runId)
    {
        if (runId >= 10 || runId < 0)
        {
            Debug.Log("runIdֻ��Ϊ0~9");
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

        return;
    }


}
