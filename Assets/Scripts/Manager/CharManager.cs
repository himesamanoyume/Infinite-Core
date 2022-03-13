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


    /// <summary>
    /// ͨ��id��ȡ�������
    /// </summary>
    /// <param name="runId"></param>
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
    /// ��Ҵ���˻�ɱ ���ݻ�ɱ��id����һ��ɱ����
    /// </summary>
    /// <param name="runId"></param>
    public void PlayerKill(int runId)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        charBase.Kill++;
        Log(runId, "��ɱ�˵���");
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
    /// ͨ��idֱ���޸���ҵ�ǰ����ֵ
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
    public void ExpChange(int runId,float minExp,float maxExp)
    {
        if (minExp< 0 || maxExp <0)
        {
            Log(runId, "�ľ���ֵ�䶯����Ϊ����");
            return;
        }

        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        float current = ToGivePlayerSomething(runId, minExp, maxExp);
        charBase.Exp += current;

        Log(runId, "�����"+ current + "����");
    }

    /// <summary>
    /// ����/�۳���ҽ�Ǯ ���ֵ����Сֵ��Ϊ���� ��Ϊ��Ǯ
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="minMoney"></param>
    /// <param name="maxMoney"></param>
    public void MoneyChange(int runId, int minMoney, int maxMoney)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(runId, minMoney, maxMoney);
        charBase.Money += current;

        if (current>=0)
        {
            Log(runId, "�����" + current + "��Ǯ");
        }
        else
        {
            Log(runId, "�۳���" + current + "��Ǯ");
        }
    }

    /// <summary>
    /// ����/�۳����Ѫ�� ��ֵ��Ϊ����
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="health"></param>
    public void HealthChange(int runId,int health)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(runId, health, health);
        charBase.Health += current;
        if (current >= 0)
        {
            Log(runId, "�ظ���" + current + "��Ѫ��");
        }
        else
        {
            Log(runId, "�۳���" + current + "��Ѫ��");
        }
    }

    /// <summary>
    /// ����/�۳�������Ѫ�� ��ֵ��Ϊ����
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="minMaxHealth"></param>
    /// <param name="maxMaxHealth"></param>
    public void MaxHealthChange(int runId, int minMaxHealth, int maxMaxHealth)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(runId, minMaxHealth, maxMaxHealth);
        charBase.MaxHealth += current;
        if (current >= 0)
        {
            Log(runId, "�����" + current + "���������ֵ");
        }
        else
        {
            Log(runId, "�۳���" + current + "���������ֵ");
        }
    }

    /// <summary>
    /// ����/�۳���һ���ֵ ��ֵ��Ϊ����
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="shield"></param>
    public void ShieldChange(int runId,int shield)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(runId, shield, shield);
        charBase.Shield += current;
        if (shield >= 0)
        {
            Log(runId, "�ظ���" + current + "�Ļ���ֵ");
        }
        else
        {
            Log(runId, "�۳���" + current + "�Ļ���ֵ");
        }
    }

    /// <summary>
    /// ����/�۳���ұ��� ��ֵ��Ϊ����
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="minCriticalHit"></param>
    /// <param name="maxCriticalHit"></param>
    public void CriticalHitChange(int runId, int minCriticalHit, int maxCriticalHit)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(runId, minCriticalHit, maxCriticalHit);
        charBase.CriticalHit += current;
        if (current >= 0)
        {
            Log(runId, "�ظ���" + current + "�ı���");
        }
        else
        {
            Log(runId, "�۳���" + current + "�ı���");
        }
    }

    /// <summary>
    /// ����/�۳���ұ����� ��ֵ��Ϊ����
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="minCriticalHitRate"></param>
    /// <param name="maxCriticalHitRate"></param>
    public void CriticalHitRateChange(int runId, int minCriticalHitRate, int maxCriticalHitRate)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(runId, minCriticalHitRate, maxCriticalHitRate);
        charBase.CriticalHitRate += current;
        if (current >= 0)
        {
            Log(runId, "�ظ���" + current + "�ı�����");
        }
        else
        {
            Log(runId, "�۳���" + current + "�ı�����");
        }
    }

    /// <summary>
    /// ����/�۳�������� ��ֵ��Ϊ����
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="minMoveSpeedChange"></param>
    /// <param name="maxMoveSpeedChange"></param>
    public void MoveSpeedChange(int runId, int minMoveSpeed, int maxMoveSpeed)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(runId, minMoveSpeed, maxMoveSpeed);
        charBase.MoveSpeed += current;
        if (current >= 0)
        {
            Log(runId, "�ظ���" + current + "������");
        }
        else
        {
            Log(runId, "�۳���" + current + "������");
        }
    }

    /// <summary>
    /// ����/�۳���ҹ��� ��ֵ��Ϊ����
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="minAttackSpeed"></param>
    /// <param name="maxAttackSpeed"></param>
    public void AttackSpeedChange(int runId, int minAttackSpeed, int maxAttackSpeed)
    {
        CharBase charBase = FindPlayerById(runId);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(runId, minAttackSpeed, maxAttackSpeed);
        charBase.AttackSpeed += current;
        if (current >= 0)
        {
            Log(runId, "�ظ���" + current + "�Ĺ���");
        }
        else
        {
            Log(runId, "�۳���" + current + "�Ĺ���");
        }
    }
    // --------------------------------------

    /// <summary>
    /// ��һ�ȡ����,��ͨ��װ�����������ֵ�Ȼ�üӳ�ʱ��ͨ�ú���
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
    /// <summary>
    /// ��һ�ȡ����,��ͨ��װ�����������ֵ�Ȼ�üӳ�ʱ��ͨ�ú���
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="lowerLimit"></param>
    /// <param name="upperLimit"></param>
    /// <returns>����ֵ���ڸ�װ����ʾ������ֵ</returns>
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

    /// <summary>
    /// ͨ��id��ĳ�������
    /// </summary>
    /// <param name="runId"></param>
    /// <returns></returns>
    public GameObject FindPlayerCameraById(int runId)
    {
        if (runId >= 10 || runId < 0)
        {
            Debug.Log("runIdֻ��Ϊ0~9");
            return null;
        }
        return playerCameraList[runId];
    }

    /// <summary>
    /// ͨ��tag�Ҹ������ĳ������������
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
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
