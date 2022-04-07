using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// �ṩ�Խ�ɫ��ֵ�Ŀ��ƺ��� �����������������
/// </summary>
public class CharManager : MonoBehaviour
{
    public GameObject total;

    public Dictionary<int, GameObject> playerModelList;
    public Dictionary<int, GameObject> recorders;

    public static CharManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerModelList = new Dictionary<int, GameObject>();
        recorders = new Dictionary<int, GameObject>();

        GameEventManager.SubscribeEvent(GameEventManager.EVENT_ON_PLAYER_LEVEL_UP, PlayerLevelUp);
    }

    private void Update()
    {
        GetPlayerModelList();
        GetRecorderList();

       
    }

    /// <summary>
    /// ��ȡ�����������ģ��
    /// </summary>
    void GetPlayerModelList()
    {
        if (PhotonNetwork.PlayerList.Length == playerModelList.Count)
        {
            return;
        }
        else
        {
            playerModelList.Clear();
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("PlayerModel");
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                foreach (GameObject obj in gameObjects)
                {

                    if (obj.GetPhotonView().OwnerActorNr == p.ActorNumber)
                    {
                        obj.name = (obj.GetPhotonView().OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)?p.NickName + " (My)": p.NickName;
                        playerModelList.Add(p.ActorNumber, obj);
                        break;
                    }
                }

            }
            Debug.LogError("������");
        }
    }

    void GetRecorderList()
    {
        if (recorders.Count == PhotonNetwork.PlayerList.Length)
        {
            return;
        }
        else
        {
            recorders.Clear();
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("PlayerRecorder");
            foreach (GameObject obj in gameObjects)
            {
                obj.name = (obj.GetPhotonView().OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber) ? obj.GetPhotonView().Owner.NickName + " Recorder (My)" : obj.GetPhotonView().Owner.NickName + " Recorder";

                obj.transform.SetParent(total.transform);

                recorders.Add(obj.GetComponent<CharBase>().ActorNumber, obj);

            }

            Debug.LogWarning("����Ӽ�¼��");
        }

    }

    /// <summary>
    /// ͨ��id��ȡ�������
    /// </summary>
    /// <param name="actorNumber"></param>
    public void GetPlayerNameById(int actorNumber)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber,out GameObject playerModel);
        if (charBase == null) { return; }

        Log(actorNumber, "��Ϊ" + charBase.PlayerName);
    }

    /// <summary>
    /// ͨ��idʹ�ض��������1��
    /// </summary>
    /// <param name="actorNumber"></param>
    public void PlayerLevelUp(object[] args)
    {
        int actorNumber,count;
        if (args.Length == 2)
        {
            actorNumber = (int)args[0];
            count = (int)args[1];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }


            for (int i = 0; i < count; i++)
            {
                if (charBase.CurrentExp >= charBase.MaxExp)
                {
                    charBase.CurrentExp = charBase.CurrentExp -  charBase.MaxExp;
                }
                charBase.MaxExp += charBase.Level * 500;
                charBase.Level += 1;
                charBase.Attack += 100;
                charBase.MaxHealth += 1000;
                charBase.CurrentHealth += 1000;

                
                charBase.Restore += 10;
            }

            Log(actorNumber, "level�Ѿ�����" + count + "��");
        }
    }

    /// <summary>
    /// ͨ��idʹ���ǿ�������������� ��GM���
    /// </summary>
    /// <param name="actorNumber"></param>
    public void OnPlayerDead(int actorNumber)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }


        charBase.CurrentHealth = 0;
        charBase.State = StateEnum.Dead;
        Log(actorNumber, "��������");
    }

    /// <summary>
    /// ͨ��idʹ��ұ���ɱ
    /// </summary>
    /// <param name="actorNumber"></param>
    public void OnPlayerKilled(int actorNumber)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        charBase.State = StateEnum.Respawning;
        charBase.CurrentHealth = 0;
        charBase.Death++;
        
        playerModel.SetActive(false);

        Log(actorNumber, "����ɱ");
        OnPlayerRespawnCountDownStart(charBase.ActorNumber);
        
    }

    /// <summary>
    /// ��Ҵ���˻�ɱ ���ݻ�ɱ��id����һ��ɱ����
    /// </summary>
    /// <param name="actorNumber"></param>
    public void OnPlayerKill(int actorNumber)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        charBase.Kill++;
        Log(actorNumber, "��ɱ�˵���");
    }

    /// <summary>
    /// �����������ʱ
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="time"></param>
    public void OnPlayerRespawnCountDownStart(int actorNumber)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        charBase.State = StateEnum.Respawning;
        charBase.RespawnCountDown = charBase.RespawnTime;
        Log(actorNumber, "��ʼ��������ʱ");
    }

    /// <summary>
    /// ͨ��idֱ���޸���ҵ�ǰ����ֵ
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="health"></param>
    public void SetPlayerHealth(int actorNumber,int health)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        if (health>charBase.MaxHealth)
        {
            Log(actorNumber,"Ԥ�޸ĵ�ֵ���ô�������������");
            return;
        }
        if (health<0)
        {
            Log(actorNumber, "Ԥ�޸ĵ�ֵ����С��0");
            return;
        }
        charBase.CurrentHealth = health;
        Log(actorNumber, "Ѫ�����޸�");
    }

    /// <summary>
    /// ����Ҿ���
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minExp"></param>
    /// <param name="maxExp"></param>
    public void ExpChange(int actorNumber,float minExp,float maxExp)
    {
        if (minExp< 0 || maxExp <0)
        {
            Log(actorNumber, "�ľ���ֵ�䶯����Ϊ����");
            return;
        }

        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        float current = ToGivePlayerSomething(actorNumber, minExp, maxExp);
        charBase.CurrentExp += current;

        Log(actorNumber, "�����"+ current + "����");
    }

    /// <summary>
    /// ����/�۳���ҽ�Ǯ ���ֵ����Сֵ��Ϊ���� ��Ϊ��Ǯ
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minMoney"></param>
    /// <param name="maxMoney"></param>
    public void MoneyChange(int actorNumber, int minMoney, int maxMoney)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(actorNumber, minMoney, maxMoney);
        charBase.Money += current;

        if (current>=0)
        {
            Log(actorNumber, "�����" + current + "��Ǯ");
        }
        else
        {
            Log(actorNumber, "�۳���" + current + "��Ǯ");
        }
    }

    /// <summary>
    /// ����/�۳����Ѫ�� ��ֵ��Ϊ����
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="health"></param>
    public void HealthChange(int actorNumber,int health)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(actorNumber, health, health);
        charBase.CurrentHealth += current;

        if (current >= 0)
        {
            Log(actorNumber, "�ظ���" + current + "��Ѫ��");
        }
        else
        {
            Log(actorNumber, "�۳���" + current + "��Ѫ��");
        }
    }

    /// <summary>
    /// ����/�۳�������Ѫ�� ��ֵ��Ϊ����
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minMaxHealth"></param>
    /// <param name="maxMaxHealth"></param>
    public void MaxHealthChange(int actorNumber, int minMaxHealth, int maxMaxHealth)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(actorNumber, minMaxHealth, maxMaxHealth);
        charBase.MaxHealth += current;
        if (current >= 0)
        {
            Log(actorNumber, "�����" + current + "���������ֵ");
        }
        else
        {
            Log(actorNumber, "�۳���" + current + "���������ֵ");
        }
    }

    /// <summary>
    /// ����/�۳���һ���ֵ ��ֵ��Ϊ����
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="shield"></param>
    public void ShieldChange(int actorNumber,int shield)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(actorNumber, shield, shield);
        charBase.Shield += current;
        if (shield >= 0)
        {
            Log(actorNumber, "�ظ���" + current + "�Ļ���ֵ");
        }
        else
        {
            Log(actorNumber, "�۳���" + current + "�Ļ���ֵ");
        }
    }

    /// <summary>
    /// ����/�۳���ұ��� ��ֵ��Ϊ����
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minCriticalHit"></param>
    /// <param name="maxCriticalHit"></param>
    public void CriticalHitChange(int actorNumber, int minCriticalHit, int maxCriticalHit)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(actorNumber, minCriticalHit, maxCriticalHit);
        charBase.CriticalHit += current;
        if (current >= 0)
        {
            Log(actorNumber, "�ظ���" + current + "�ı���");
        }
        else
        {
            Log(actorNumber, "�۳���" + current + "�ı���");
        }
    }

    /// <summary>
    /// ����/�۳���ұ����� ��ֵ��Ϊ����
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minCriticalHitRate"></param>
    /// <param name="maxCriticalHitRate"></param>
    public void CriticalHitRateChange(int actorNumber, int minCriticalHitRate, int maxCriticalHitRate)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(actorNumber, minCriticalHitRate, maxCriticalHitRate);
        charBase.CriticalHitRate += current;
        if (current >= 0)
        {
            Log(actorNumber, "�ظ���" + current + "�ı�����");
        }
        else
        {
            Log(actorNumber, "�۳���" + current + "�ı�����");
        }
    }

    /// <summary>
    /// ����/�۳�������� ��ֵ��Ϊ����
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minMoveSpeedChange"></param>
    /// <param name="maxMoveSpeedChange"></param>
    public void MoveSpeedChange(int actorNumber, int minMoveSpeed, int maxMoveSpeed)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(actorNumber, minMoveSpeed, maxMoveSpeed);
        charBase.MoveSpeed += current;
        if (current >= 0)
        {
            Log(actorNumber, "�ظ���" + current + "������");
        }
        else
        {
            Log(actorNumber, "�۳���" + current + "������");
        }
    }

    /// <summary>
    /// ����/�۳���ҹ��� ��ֵ��Ϊ����
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minAttackSpeed"></param>
    /// <param name="maxAttackSpeed"></param>
    public void AttackSpeedChange(int actorNumber, int minAttackSpeed, int maxAttackSpeed)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }

        int current = ToGivePlayerSomething(actorNumber, minAttackSpeed, maxAttackSpeed);
        charBase.AttackSpeed += current;
        if (current >= 0)
        {
            Log(actorNumber, "�ظ���" + current + "�Ĺ���");
        }
        else
        {
            Log(actorNumber, "�۳���" + current + "�Ĺ���");
        }
    }

    /// <summary>
    /// (float)��һ�ȡ����,��ͨ��װ�����������ֵ�Ȼ�üӳ�ʱ��ͨ�ú���
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="lowerLimit"></param>
    /// <param name="upperLimit"></param>
    /// <returns>����ֵ���ڸ�װ����ʾ������ֵ</returns>
    public float ToGivePlayerSomething(int actorNumber,float lowerLimit,float upperLimit)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return 0; }

        float value = Random.Range(lowerLimit, upperLimit);
        return value;
    }

    /// <summary>
    /// (int)��һ�ȡ����,��ͨ��װ�����������ֵ�Ȼ�üӳ�ʱ��ͨ�ú���
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="lowerLimit"></param>
    /// <param name="upperLimit"></param>
    /// <returns>����ֵ���ڸ�װ����ʾ������ֵ</returns>
    public int ToGivePlayerSomething(int actorNumber, int lowerLimit, int upperLimit)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return 0; }

        int value = Random.Range(lowerLimit, upperLimit);
        return value;
    }


    public void Log(int actorNumber,string text)
    {
        Debug.Log("ActorNumberΪ" + actorNumber + "�����" + text);
    }

    public void Log(string text)
    {
        Debug.Log(text);
    }

    /// <summary>
    /// ͨ��ActorNumber��ȡ�ض���ҵ����,�����ڽ�Ҫ�����ض����Խ����޸�ʱ
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <returns></returns>
    public CharBase FindPlayerByActorNumber(int actorNumber,out GameObject playerModel)
    {

        CharBase charBase;

        foreach (var recorder in recorders)
        {
            charBase = recorder.Value.GetComponent<CharBase>();
            if (charBase.ActorNumber == actorNumber)
            {
                
                playerModel = playerModelList[charBase.ActorNumber];
                return charBase;
            }
            
        }
        Debug.Log("δ�õ�ָ��actorNumber�����");
        playerModel = null;
        return null;
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

        needTarget.ActorNumber = provider.ActorNumber;
        needTarget.PlayerName = provider.PlayerName;
        needTarget.Kill = provider.Kill;
        needTarget.Death = provider.Death;
        needTarget.Money = provider.Money;
        needTarget.CurrentExp = provider.CurrentExp;
        needTarget.MaxExp = provider.MaxExp;
        needTarget.State = provider.State;
        //needTarget.Buff = provider.Buff;
        needTarget.Pro = provider.Pro;
        needTarget.Level = provider.Level;
        needTarget.Attack = provider.Attack;
        needTarget.MaxHealth = provider.MaxHealth;
        needTarget.CurrentHealth = provider.CurrentHealth;
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
        needTarget.BootsID = provider.BootsID;
        needTarget.MoveSpeed = provider.MoveSpeed;
        needTarget.AttackRange = provider.AttackRange;
        needTarget.RespawnTime = provider.RespawnTime;
        needTarget.RespawnCountDown = provider.RespawnCountDown;

        return;
    }

}
