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

        GameEventManager.SubscribeEvent(EventName.onPlayerLevelUp, PlayerLevelUp);
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

    /// <summary>
    /// ��ȡ����������Ҽ�¼��
    /// </summary>
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

        Toast(new object[2] { actorNumber, "��Ϊ" + charBase.PlayerName });
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
            GameEventManager.EnableEvent(EventName.onPlayerLevelUp, false);
            Toast(new object[2] { actorNumber, "level�Ѿ�����" + count + "��" });
        }
    }

    /// <summary>
    /// ͨ��idʹ���ǿ�������������� ��GM���
    /// </summary>
    /// <param name="actorNumber"></param>
    public void SetPlayerDead(int actorNumber)
    {
        CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
        if (charBase == null) { return; }


        charBase.CurrentHealth = 0;
        charBase.State = StateEnum.Dead;

        GameEventManager.EnableEvent(EventName.onPlayerDead, false);
        Toast(new object[2] { actorNumber, "��������" });
        
    }

    /// <summary>
    /// ͨ��idʹ��ұ���ɱ
    /// </summary>
    /// <param name="actorNumber"></param>
    public void OnPlayerKilled(object[] args)
    {
        int actorNumber;

        if(args.Length == 1)
        {
            actorNumber = (int)args[0];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            charBase.State = StateEnum.Respawning;
            charBase.CurrentHealth = 0;
            charBase.Death++;

            //��仰��Ҫ��Ϊȫ�ִݻ�
            playerModel.SetActive(false);

            Toast(new object[2] { actorNumber, "����ɱ" });
            GameEventManager.EnableEvent(EventName.onPlayerKilled, false);
            GameEventManager.EnableEvent(EventName.onPlayerRespawnCountDownStart, true);
        }     
        //OnPlayerRespawnCountDownStart(charBase.ActorNumber);
        
    }

    /// <summary>
    /// ��Ҵ���˻�ɱ ���ݻ�ɱ��id����һ��ɱ����
    /// </summary>
    /// <param name="actorNumber"></param>
    public void OnPlayerKill(object[] args)
    {
        int actorNumber;
        if(args.Length == 1)
        {
            actorNumber = (int)args[0];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            charBase.Kill++;

            GameEventManager.EnableEvent(EventName.onPlayerKill, false);
            Toast(new object[2] { actorNumber, "��ɱ�˵���" });
        }
    }

    /// <summary>
    /// ��һ�Ѫ
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerRestore(object[] args)
    {

    }

    /// <summary>
    /// ��Ҵ����޺�״̬�³�������
    /// </summary>
    /// <param name="args"></param>
    public void OnPlayerDead(object[] args)
    {
        int actorNumber;
        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            charBase.State = StateEnum.Dead;
            GameEventManager.EnableEvent(EventName.onPlayerDead, false);
            Toast(new object[2] { actorNumber, "��������" });
        }
    }

    /// <summary>
    /// �����������ʱ
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="time"></param>
    public void OnPlayerRespawnCountDownStart(object[] args)
    {
        int actorNumber;
        if(args.Length == 1)
        {
            actorNumber= (int)args[0];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            charBase.State = StateEnum.Respawning;
            charBase.RespawnCountDown = charBase.RespawnTime;

            GameEventManager.EnableEvent(EventName.onPlayerRespawnCountDownStart, false);
            GameEventManager.EnableEvent(EventName.onPlayerRespawning,true);
            Toast(new object[2] { actorNumber, "��ʼ��������ʱ" });
        }

    }

    public void OnPlayerRespawning(object[] args)
    {
        int actorNumber;

        if( args.Length == 1)
        {
            actorNumber = (int)args[0];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            if (charBase.State == StateEnum.Respawning)
            {
                charBase.RespawnCountDown -= Time.deltaTime;
            }

            if (charBase.RespawnCountDown == 0)
            {
                GameEventManager.EnableEvent(EventName.onPlayerRespawning,false);
                GameEventManager.EnableEvent(EventName.onPlayerRespawnCountDownEnd, true);
            }
        }

    }

    public void OnPlayerRespawnCountDownEnd(object[] args)
    {
        int actorNumber;

        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            charBase.State = StateEnum.Alive;

            GameEventManager.EnableEvent(EventName.onPlayerRespawn, true);
            GameEventManager.EnableEvent(EventName.onPlayerRespawnCountDownEnd, false);
            
        }
    }

    public void OnPlayerRespawn(object[] args)
    {
        int actorNumber;
        if (args.Length == 1)
        {
            actorNumber = (int)args[0];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            charBase.CurrentHealth = charBase.MaxHealth;
            //�����߼�

            //
            GameEventManager.EnableEvent(EventName.onPlayerRespawn, false);
        }
       
    }

    /// <summary>
    /// ͨ��idֱ���޸���ҵ�ǰ����ֵ
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="health"></param>
    public void SetPlayerHealth(object[] args)
    {
        int actorNumber,health;
        if(args.Length == 2)
        {
            actorNumber = (int)args [0];
            health = (int)args[1];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            if (health > charBase.MaxHealth)
            {
                Toast(new object[2] { actorNumber, "Ԥ�޸ĵ�ֵ���ô�������������" });
                return;
            }
            if (health < 0)
            {
                Toast(new object[2] { actorNumber, "Ԥ�޸ĵ�ֵ����С��0" });
                return;
            }
            charBase.CurrentHealth = health;

            GameEventManager.EnableEvent(EventName.onPlayerCurrentHealthChanged, false);
            Toast(new object[2] { actorNumber, "Ѫ�����޸�" });
        }
    }

    /// <summary>
    /// ����Ҿ���
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minExp"></param>
    /// <param name="maxExp"></param>
    public void ExpChange(object[] args)
    {
        int actorNumber; float minExp, maxExp;

        if(args.Length == 3)
        {
            actorNumber = (int)args[0];
            minExp = (float)args[1];
            maxExp = (float)args[2];

            if (minExp < 0 || maxExp < 0)
            {
                Toast(new object[2] { actorNumber, "�ľ���ֵ�䶯����Ϊ����" });
                return;
            }

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            float current = ToGivePlayerSomething(args);
            charBase.CurrentExp += current;

            GameEventManager.EnableEvent(EventName.onPlayerCurrentExpChanged, false);
            Toast(new object[2] { actorNumber, "�����" + current + "����" });
        }

        
    }

    /// <summary>
    /// ����/�۳���ҽ�Ǯ ���ֵ����Сֵ��Ϊ���� ��Ϊ��Ǯ
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minMoney"></param>
    /// <param name="maxMoney"></param>
    public void MoneyChange(object[] args)
    {
        int actorNumber, minMoney, maxMoney;

        if( args.Length == 3)
        {
            actorNumber = (int)args [0];
            minMoney = (int)args[1];
            maxMoney = (int)args[2];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            int current = ToGivePlayerSomething(args);
            charBase.Money += current;

            if (current >= 0)
            {
                Toast(new object[2] { actorNumber, "�����" + current + "��Ǯ" });
            }
            else
            {
                Toast(new object[2] { actorNumber, "�۳���" + current + "��Ǯ" });
            }
        }

    }

    /// <summary>
    /// ����/�۳����Ѫ�� ��ֵ��Ϊ����
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="health"></param>
    public void HealthChange(object[] args)
    {
        int actorNumber, health;

        if(args.Length == 2)
        {
            actorNumber = (int)args[0];
            health = (int)args[1];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            int current = ToGivePlayerSomething(new object[] { actorNumber, health, health });
            charBase.CurrentHealth += current;

            if (current >= 0)
            {
                Toast(new object[2] { actorNumber, "�ظ���" + current + "��Ѫ��" });
            }
            else
            {
                Toast(new object[2] { actorNumber, "�۳���" + current + "��Ѫ��" });
            }
        }
    }

    /// <summary>
    /// ����/�۳�������Ѫ�� ��ֵ��Ϊ����
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minMaxHealth"></param>
    /// <param name="maxMaxHealth"></param>
    public void MaxHealthChange(object[] args)
    {
        int actorNumber, minMaxHealth, maxMaxHealth;

        if(args.Length == 3)
        {
            actorNumber = (int)args[0];
            minMaxHealth = (int)args[1];
            maxMaxHealth = (int)args[2];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            int current = ToGivePlayerSomething(args);
            charBase.MaxHealth += current;
            if (current >= 0)
            {
                Toast(new object[2] { actorNumber, "�����" + current + "���������ֵ" });
            }
            else
            {
                Toast(new object[2] { actorNumber, "�۳���" + current + "���������ֵ" });
            }
        }

        
    }

    /// <summary>
    /// ����/�۳���һ���ֵ ��ֵ��Ϊ����
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="shield"></param>
    public void ShieldChange(object[] args)
    {
        int actorNumber, shield;

        if(args.Length == 2)
        {
            actorNumber = (int)args[0];
            shield = (int)args[1];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            int current = ToGivePlayerSomething(new object[] { actorNumber, shield, shield });
            charBase.Shield += current;
            if (shield >= 0)
            {
                Toast(new object[2] { actorNumber, "�ظ���" + current + "�Ļ���ֵ" });
            }
            else
            {
                Toast(new object[2] { actorNumber, "�۳���" + current + "�Ļ���ֵ" });
            }
        } 
    }

    /// <summary>
    /// ����/�۳���ұ��� ��ֵ��Ϊ����
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minCriticalHit"></param>
    /// <param name="maxCriticalHit"></param>
    public void CriticalHitChange(object[] args)
    {
        int actorNumber, minCriticalHit, maxCriticalHit;

        if(args.Length == 3)
        {
            actorNumber = (int)args[0];
            minCriticalHit = (int)args[1];
            maxCriticalHit = (int)args[2];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            int current = ToGivePlayerSomething(args);
            charBase.CriticalHit += current;
            if (current >= 0)
            {
                Toast(new object[2] { actorNumber, "�ظ���" + current + "�ı���" });
            }
            else
            {
                Toast(new object[2] { actorNumber, "�۳���" + current + "�ı���" });
            }
        }
    }

    /// <summary>
    /// ����/�۳���ұ����� ��ֵ��Ϊ����
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minCriticalHitRate"></param>
    /// <param name="maxCriticalHitRate"></param>
    public void CriticalHitRateChange(object[] args)
    {
        int actorNumber, minCriticalHitRate, maxCriticalHitRate;
        if (args.Length == 3)
        {
            actorNumber = (int)args[0];
            minCriticalHitRate = (int)args[1];
            maxCriticalHitRate = (int)args[2];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            int current = ToGivePlayerSomething(args);
            charBase.CriticalHitRate += current;
            if (current >= 0)
            {
                Toast(new object[2] { actorNumber, "�ظ���" + current + "�ı�����" });
            }
            else
            {
                Toast(new object[2] { actorNumber, "�۳���" + current + "�ı�����" });
            }
        }


    }

    /// <summary>
    /// ����/�۳�������� ��ֵ��Ϊ����
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minMoveSpeedChange"></param>
    /// <param name="maxMoveSpeedChange"></param>
    public void MoveSpeedChange(object[] args)
    {
        int actorNumber, minMoveSpeed, maxMoveSpeed;

        if(args.Length == 3)
        {
            actorNumber = (int)args[0];
            minMoveSpeed = (int)args[1];
            maxMoveSpeed = (int)args[2];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            int current = ToGivePlayerSomething(args);
            charBase.MoveSpeed += current;
            if (current >= 0)
            {
                Toast(new object[2] { actorNumber, "�ظ���" + current + "������" });
            }
            else
            {
                Toast(new object[2] { actorNumber, "�۳���" + current + "������" });
            }
        }

    }

    /// <summary>
    /// ����/�۳���ҹ��� ��ֵ��Ϊ����
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="minAttackSpeed"></param>
    /// <param name="maxAttackSpeed"></param>
    public void AttackSpeedChange(object[] args)
    {
        int actorNumber, minAttackSpeed, maxAttackSpeed;

        if(args.Length == 3)
        {
            actorNumber = (int)args[0];
            minAttackSpeed = (int)args[1];
            maxAttackSpeed = (int)args[2];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return; }

            int current = ToGivePlayerSomething(args);
            charBase.AttackSpeed += current;
            if (current >= 0)
            {
                Toast(new object[2] { actorNumber, "�ظ���" + current + "�Ĺ���" });
            }
            else
            {
                Toast(new object[2] { actorNumber, "�۳���" + current + "�Ĺ���" });
            }
        }

        
    }

    /// <summary>
    /// (int)��һ�ȡ����,��ͨ��װ�����������ֵ�Ȼ�üӳ�ʱ��ͨ�ú���
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="lowerLimit"></param>
    /// <param name="upperLimit"></param>
    /// <returns>����ֵ���ڸ�װ����ʾ������ֵ</returns>
    public int ToGivePlayerSomething(object[] args)
    {
        int actorNumber, lowerLimit, upperLimit;

        if(args.Length == 3)
        {
            actorNumber = (int)args[0];
            lowerLimit = (int)args[1];
            upperLimit = (int)args[2];

            CharBase charBase = FindPlayerByActorNumber(actorNumber, out GameObject playerModel);
            if (charBase == null) { return 0; }

            int value = Random.Range(lowerLimit, upperLimit);
            return value;
        }
        else
        {
            Toast(new object[1] { "�����ֵ��������" });
            return 0;
        }
        
    }

    /// <summary>
    /// ��˿
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <param name="text"></param>
    public void Toast(object[] args)
    {
        int actorNumber;
        string text;
        if (args.Length == 2)
        {
            actorNumber = (int)args[0];
            text = args[1].ToString();
            Debug.Log("ActorNumberΪ" + actorNumber + "�����" + text);
        }
        if (args.Length == 1)
        {
            text = args[0].ToString();
            Debug.Log(text);
        }
        GameEventManager.EnableEvent(EventName.onToast, false);
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
