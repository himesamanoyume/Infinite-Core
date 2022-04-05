using Photon.Pun;
using Photon.Realtime;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSpawnController : MonoBehaviour
{
    public static CharSpawnController instance;

    public GameObject archer;
    public GameObject doctor;
    public GameObject solider;
    public GameObject tanker;

    public GameObject spawnRedPos1;
    public GameObject spawnRedPos2;
    public GameObject spawnRedPos3;
    public GameObject spawnRedPos4;
    public GameObject spawnRedPos5;

    public GameObject spawnBluePos1;
    public GameObject spawnBluePos2;
    public GameObject spawnBluePos3;
    public GameObject spawnBluePos4;
    public GameObject spawnBluePos5;

    //public GameObject mainCamera;

    int redCount = 0;
    int blueCount = 0;

    List<GameObject> redPosList;
    List<GameObject> bluePosList;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        redPosList = new List<GameObject>();
        bluePosList = new List<GameObject>();
        redPosList.Add(spawnRedPos1);
        redPosList.Add(spawnRedPos2);
        redPosList.Add(spawnRedPos3);
        redPosList.Add(spawnRedPos4);
        redPosList.Add(spawnRedPos5);
        bluePosList.Add(spawnBluePos1);
        bluePosList.Add(spawnBluePos2);
        bluePosList.Add(spawnBluePos3);
        bluePosList.Add(spawnBluePos4);
        bluePosList.Add(spawnBluePos5);


        SpawnPlayer(PhotonNetwork.LocalPlayer);


    }


    public void SpawnPlayer(Player player)
    {
        //player只能是自己

        if(player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_TEAM,out object team))
        {
            if ((TeamEnum)team == TeamEnum.Null) return;

            object pro;
            switch ((TeamEnum)team)
            {
                case TeamEnum.Red:

                    player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_PRO, out pro);

                    GameObject playerModel = PhotonNetwork.Instantiate(((ProEnum)pro).ToString(), redPosList[Random.Range(0,5)].transform.position, Quaternion.identity);

                    playerModel.name = player.NickName + " (My)";

                    GetPlayerInfo(playerModel.GetComponent<CharBase>(), player);
                    break;
                case TeamEnum.Blue:
 
                    player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_PRO, out pro);

                    playerModel = PhotonNetwork.Instantiate(((ProEnum)pro).ToString(), bluePosList[Random.Range(0,5)].transform.position, Quaternion.identity);

                    playerModel.name = player.NickName;

                    GetPlayerInfo(playerModel.GetComponent<CharBase>(), player);

                    break;
            }
        }
    }

    /// <summary>
    /// 【待重写】生成玩家的复活分支
    /// </summary>
    /// <param name="charBase"></param>
    public void RespawnPlayer(CharBase charBase)
    {
        charBase.State = StateEnum.Alive;
        charBase.RespawnTime += 5f;
        GameObject playerModel = CharManager.Instance.FindChildObjWithTag("PlayerModel", charBase.gameObject);
        playerModel.SetActive(true);
        playerModel.transform.position = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// 【已弃用】给玩家选择生成位置
    /// </summary>
    /// <param name="charBase"></param>
    public void SelectPos(CharBase charBase)
    {
        GameObject playerObject;
        Debug.LogWarning("调用了生成位置");
        switch (charBase.Pro)
        {
            case ProEnum.Soilder:
                playerObject = PhotonNetwork.Instantiate(solider.name, new Vector3(0, 0, 0), Quaternion.identity);
                
                switch (charBase.ActorNumber)
                {
                    case 0:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos1);
                        break;
                    case 1:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos2);
                        break;
                    case 2:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos3);
                        break;
                    case 3:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos4);
                        break;
                    case 4:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos5);
                        break;
                    case 5:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos1);
                        break;
                    case 6:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos2);
                        break;
                    case 7:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos3);
                        break;
                    case 8:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos4);
                        break;
                    case 9:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos5);
                        break;
                }
                break;
            case ProEnum.Archer:
                playerObject = PhotonNetwork.Instantiate(archer.name, new Vector3(0, 0, 0), Quaternion.identity);
                switch (charBase.ActorNumber)
                {
                    case 0:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos1);
                        break;
                    case 1:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos2);
                        break;
                    case 2:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos3);
                        break;
                    case 3:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos4);
                        break;
                    case 4:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos5);
                        break;
                    case 5:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos1);
                        break;
                    case 6:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos2);
                        break;
                    case 7:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos3);
                        break;
                    case 8:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos4);
                        break;
                    case 9:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos5);
                        break;
                }
                break;
            case ProEnum.Doctor:
                playerObject = PhotonNetwork.Instantiate(doctor.name, new Vector3(0, 0, 0), Quaternion.identity);
                switch (charBase.ActorNumber)
                {
                    case 0:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos1);
                        break;
                    case 1:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos2);
                        break;
                    case 2:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos3);
                        break;
                    case 3:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos4);
                        break;
                    case 4:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos5);
                        break;
                    case 5:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos1);
                        break;
                    case 6:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos2);
                        break;
                    case 7:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos3);
                        break;
                    case 8:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos4);
                        break;
                    case 9:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos5);
                        break;
                }
                break;
            case ProEnum.Tanker:
                playerObject = PhotonNetwork.Instantiate(tanker.name, new Vector3(0, 0, 0), Quaternion.identity);
                switch (charBase.ActorNumber)
                {
                    case 0:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos1);
                        break;
                    case 1:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos2);
                        break;
                    case 2:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos3);
                        break;
                    case 3:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos4);
                        break;
                    case 4:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos5);
                        break;
                    case 5:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos1);
                        break;
                    case 6:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos2);
                        break;
                    case 7:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos3);
                        break;
                    case 8:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos4);
                        break;
                    case 9:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos5);
                        break;
                }
                break;
        }
        
    }

    /// <summary>
    /// 待重写 已弃用
    /// </summary>
    /// <param name="playerObject"></param>
    /// <param name="charBase"></param>
    public void RespawnSelectPos(GameObject playerObject, CharBase charBase)
    {
        //此处playerObject为playerModel
        switch (charBase.ActorNumber)
        {
            case 0:
                SpawnPlayerForPos(playerObject, charBase, spawnRedPos1);
                break;
            case 1:
                SpawnPlayerForPos(playerObject, charBase, spawnRedPos2);
                break;
            case 2:
                SpawnPlayerForPos(playerObject, charBase, spawnRedPos3);
                break;
            case 3:
                SpawnPlayerForPos(playerObject, charBase, spawnRedPos4);
                break;
            case 4:
                SpawnPlayerForPos(playerObject, charBase, spawnRedPos5);
                break;
            case 5:
                SpawnPlayerForPos(playerObject, charBase, spawnBluePos1);
                break;
            case 6:
                SpawnPlayerForPos(playerObject, charBase, spawnBluePos2);
                break;
            case 7:
                SpawnPlayerForPos(playerObject, charBase, spawnBluePos3);
                break;
            case 8:
                SpawnPlayerForPos(playerObject, charBase, spawnBluePos4);
                break;
            case 9:
                SpawnPlayerForPos(playerObject, charBase, spawnBluePos5);
                break;
        }
    }

    /// <summary>
    /// 【已弃用】生成一个新玩家时给玩家赋予一个runId
    /// </summary>
    /// <param name="playerTeam"></param>
    /// <param name="player"></param>
    public void SetPlayerRunId(CharBase charBase)
    {
        switch (charBase.PlayerTeam)
        {
            case TeamEnum.Red:
                if (redCount > 4)
                {
                    Debug.Log("红方人数已达最大");
                    return;
                }
                break;
            case TeamEnum.Blue:
                if (blueCount > 4)
                {
                    Debug.Log("蓝方人数已达最大");
                    return;
                }
                break;
        }
        
        switch (charBase.PlayerTeam)
        {
            case TeamEnum.Red:
                CharManager.Instance.Log("Red");
                charBase.ActorNumber = redCount;
                redCount++;
                SelectPos(charBase);
                break;

            case TeamEnum.Blue:
                CharManager.Instance.Log("Blue");
                charBase.ActorNumber = blueCount + 5;
                blueCount++;
                SelectPos(charBase);
                break;
        }
    }

    /// <summary>
    /// 根据模型,CharBase信息,生成地点,玩家对应的值赋给玩家
    /// </summary>
    /// <param name="playerObject"></param>
    /// <param name="charBase"></param>
    /// <param name="spawnPos"></param>
    public void SpawnPlayerForPos(GameObject playerObject, CharBase charBase, GameObject spawnPos)
    {
        CharBase component;

        
        playerObject.transform.parent = spawnPos.transform;
        playerObject.transform.position = new Vector3(0,0,0);

        component = spawnPos.GetComponent<CharBase>();
        charBase.State = StateEnum.Alive;
        //需要方便快速赋值
        CharManager.Instance.GetPlayerInfo(component, charBase);
    }

    /// <summary>
    /// 根据玩家属性获取所有信息
    /// </summary>
    /// <param name="needTarget"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    public void GetPlayerInfo(CharBase needTarget,Player player)
    {

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_ACTOR_NUMBER, out object actorNumber);
        needTarget.ActorNumber = (int)actorNumber;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_NAME,out object playerName);
        needTarget.PlayerName = (string)playerName;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_TEAM, out object team);
        needTarget.PlayerTeam = (TeamEnum)team;

        needTarget.State = StateEnum.Alive;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_KILL, out object kill);
        //needTarget.Kill = (int)kill;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_DEATH, out object death);
        //needTarget.Death = (int)death;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_MONEY, out object money);
        //needTarget.Money = (int)money;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_CURRENT_EXP , out object currentExp);
        //needTarget.CurrentExp = (float)currentExp;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_MAX_EXP , out object maxExp);
        //needTarget.MaxExp = (float)maxExp;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_STATE , out object state);
        //needTarget.State = (StateEnum)state;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_BUFF , out object buff);
        //needTarget.Buff = (BuffEnum[])buff;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_PRO , out object pro);
        needTarget.Pro = (ProEnum)pro;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_LEVEL , out object level);
        //needTarget.Level = (int)level;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_ATTACK , out object attack);
        needTarget.Attack = (float)attack;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_MAX_HEALTH , out object maxHealth);
        needTarget.MaxHealth = (float)maxHealth;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_CURRENT_HEALTH , out object currentHealth);
        needTarget.CurrentHealth = (float)currentHealth;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_CRITICALHIT_HIT , out object criticalHit);
        needTarget.CriticalHit = (float)criticalHit;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_CRITICAL_HIT_RATE , out object criticalHitRate);
        needTarget.CriticalHitRate = (float)criticalHitRate;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_DEFENCE , out object defence);
        needTarget.Defence = (float)defence;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_ATTACK_SPEED , out object attackSpeed);
        needTarget.AttackSpeed = (float)attackSpeed;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_RESTORE , out object restore);
        needTarget.Restore = (float)restore;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_SKILL_Q , out object skillQ);
        needTarget.SkillQ = (SkillQ)skillQ;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_SKILL_E , out object skillE);
        needTarget.SkillE = (SkillE)skillE;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_SKILL_R , out object skillR);
        needTarget.SkillR = (SkillR)skillR;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_SKILL_BURST , out object skillBurst);
        needTarget.SkillBurst = (SkillBurst)skillBurst;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_HEAD_ID , out object headId);
        //needTarget.HeadID = (EquipHead)headId;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_ARMOR_ID , out object armorId);
        //needTarget.ArmorID = (EquipArmor)armorId;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_HAND_ID , out object handId);
        //needTarget.HandID = (EquipHand)handId;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_KNEE_ID , out object kneeId);
        //needTarget.KneeID = (EquipKnee)kneeId;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_TROUSERS_ID , out object trousersId);
        //needTarget.TrousersID = (EquipTrousers)trousersId;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_BOOTS , out object bootsId);
        //needTarget.BootsID = (EquipBoots)bootsId;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_MOVE_SPEED , out object moveSpeed);
        needTarget.MoveSpeed = (float)moveSpeed;

        player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_ATTACK_RANGE , out object attackRange);
        needTarget.AttackRange = (float)attackRange;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_RESPAWN_TIME , out object respawnTime);
        //needTarget.RespawnTime = (float)respawnTime;

        //player.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_RESPAWN_COUNTDOWN , out object respawnCountDown);
        //needTarget.RespawnCountDown = (float)respawnCountDown;

        return;
    }

}
