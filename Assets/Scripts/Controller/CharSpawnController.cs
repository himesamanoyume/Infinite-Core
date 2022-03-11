using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSpawnController : MonoBehaviour
{
    public static CharSpawnController instance;

    public CharBase charBaseScript;

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

    public Camera redPlayer1Camera;
    public Camera redPlayer2Camera;
    public Camera redPlayer3Camera;
    public Camera redPlayer4Camera;
    public Camera redPlayer5Camera;

    public Camera bluePlayer1Camera;
    public Camera bluePlayer2Camera;
    public Camera bluePlayer3Camera;
    public Camera bluePlayer4Camera;
    public Camera bluePlayer5Camera;

    int redCount = 0;
    int blueCount = 0;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 用于第一次生成玩家
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="playerName"></param>
    /// <param name="pro"></param>
    /// <param name="skillQ"></param>
    /// <param name="skillE"></param>
    /// <param name="skillR"></param>
    /// <param name="skillBurst"></param>
    /// <param name="playerTeam"></param>
    public void SpawnPlayer(int runId, string playerName, CharEnum.ProEnum pro, SkillEnum.skillID skillQ, SkillEnum.skillID skillE, SkillEnum.skillID skillR, SkillEnum.skillBurst skillBurst, TeamEnum.playerTeam playerTeam)
    {
        CharBase charBase = new CharBase(runId,playerName,pro,skillQ,skillE,skillR,skillBurst,playerTeam);

        //第一次生成
        if (runId==-1)
        {
            SetPlayerRunId(charBase);
        }
        else
        {
            Debug.Log("SpawnPlayer非常状态发生了");
        }
        
    }

    /// <summary>
    /// 生成玩家的复活分支
    /// </summary>
    /// <param name="charBase"></param>
    public void SpawnPlayer(CharBase charBase)
    {
        charBase.State = CharEnum.StateEnum.存活;
        charBase.RespawnTime += 10f;
        GameObject playerModel = CharManager.instance.FindChildObjWithTag("PlayerModel", charBase.gameObject);
        playerModel.SetActive(true);

    }

    /// <summary>
    /// 给玩家选择生成位置和挂载的摄像机
    /// </summary>
    /// <param name="charBase"></param>
    public void SelectPos(CharBase charBase)
    {
        GameObject playerObject;
        switch (charBase.Pro)
        {
            case CharEnum.ProEnum.近战:
                playerObject = Instantiate(solider);
                switch (charBase.RunId)
                {
                    case 0:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos1, redPlayer1Camera);
                        break;
                    case 1:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos2, redPlayer2Camera);
                        break;
                    case 2:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos3, redPlayer3Camera);
                        break;
                    case 3:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos4, redPlayer4Camera);
                        break;
                    case 4:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos5, redPlayer5Camera);
                        break;
                    case 5:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos1, bluePlayer1Camera);
                        break;
                    case 6:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos2, bluePlayer2Camera);
                        break;
                    case 7:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos3, bluePlayer3Camera);
                        break;
                    case 8:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos4, bluePlayer4Camera);
                        break;
                    case 9:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos5, bluePlayer5Camera);
                        break;
                }
                break;
            case CharEnum.ProEnum.远程:
                playerObject = Instantiate(archer);
                switch (charBase.RunId)
                {
                    case 0:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos1, redPlayer1Camera);
                        break;
                    case 1:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos2, redPlayer2Camera);
                        break;
                    case 2:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos3, redPlayer3Camera);
                        break;
                    case 3:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos4, redPlayer4Camera);
                        break;
                    case 4:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos5, redPlayer5Camera);
                        break;
                    case 5:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos1, bluePlayer1Camera);
                        break;
                    case 6:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos2, bluePlayer2Camera);
                        break;
                    case 7:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos3, bluePlayer3Camera);
                        break;
                    case 8:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos4, bluePlayer4Camera);
                        break;
                    case 9:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos5, bluePlayer5Camera);
                        break;
                }
                break;
            case CharEnum.ProEnum.辅助:
                playerObject = Instantiate(doctor);
                switch (charBase.RunId)
                {
                    case 0:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos1, redPlayer1Camera);
                        break;
                    case 1:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos2, redPlayer2Camera);
                        break;
                    case 2:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos3, redPlayer3Camera);
                        break;
                    case 3:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos4, redPlayer4Camera);
                        break;
                    case 4:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos5, redPlayer5Camera);
                        break;
                    case 5:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos1, bluePlayer1Camera);
                        break;
                    case 6:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos2, bluePlayer2Camera);
                        break;
                    case 7:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos3, bluePlayer3Camera);
                        break;
                    case 8:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos4, bluePlayer4Camera);
                        break;
                    case 9:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos5, bluePlayer5Camera);
                        break;
                }
                break;
            case CharEnum.ProEnum.坦克:
                playerObject = Instantiate(tanker);
                switch (charBase.RunId)
                {
                    case 0:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos1, redPlayer1Camera);
                        break;
                    case 1:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos2, redPlayer2Camera);
                        break;
                    case 2:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos3, redPlayer3Camera);
                        break;
                    case 3:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos4, redPlayer4Camera);
                        break;
                    case 4:
                        SpawnPlayerForPos(playerObject, charBase, spawnRedPos5, redPlayer5Camera);
                        break;
                    case 5:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos1, bluePlayer1Camera);
                        break;
                    case 6:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos2, bluePlayer2Camera);
                        break;
                    case 7:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos3, bluePlayer3Camera);
                        break;
                    case 8:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos4, bluePlayer4Camera);
                        break;
                    case 9:
                        SpawnPlayerForPos(playerObject, charBase, spawnBluePos5, bluePlayer5Camera);
                        break;
                }
                break;
        }
        
    }

    public void RespawnSelectPos(GameObject playerObject, CharBase charBase)
    {
        //此处playerObject为playerModel
        switch (charBase.RunId)
        {
            case 0:
                SpawnPlayerForPos(playerObject, charBase, spawnRedPos1, redPlayer1Camera);
                break;
            case 1:
                SpawnPlayerForPos(playerObject, charBase, spawnRedPos2, redPlayer2Camera);
                break;
            case 2:
                SpawnPlayerForPos(playerObject, charBase, spawnRedPos3, redPlayer3Camera);
                break;
            case 3:
                SpawnPlayerForPos(playerObject, charBase, spawnRedPos4, redPlayer4Camera);
                break;
            case 4:
                SpawnPlayerForPos(playerObject, charBase, spawnRedPos5, redPlayer5Camera);
                break;
            case 5:
                SpawnPlayerForPos(playerObject, charBase, spawnBluePos1, bluePlayer1Camera);
                break;
            case 6:
                SpawnPlayerForPos(playerObject, charBase, spawnBluePos2, bluePlayer2Camera);
                break;
            case 7:
                SpawnPlayerForPos(playerObject, charBase, spawnBluePos3, bluePlayer3Camera);
                break;
            case 8:
                SpawnPlayerForPos(playerObject, charBase, spawnBluePos4, bluePlayer4Camera);
                break;
            case 9:
                SpawnPlayerForPos(playerObject, charBase, spawnBluePos5, bluePlayer5Camera);
                break;
        }
    }

    /// <summary>
    /// 复活玩家
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="playerName"></param>
    public void RespawnPlayer(int runId, string playerName)
    {
        CharBase charBase = CharManager.instance.FindPlayerById(runId);
        if (charBase == null) { return; }

        SpawnPlayer(charBase);

    }

    /// <summary>
    /// 生成一个新玩家时给玩家赋予一个runId
    /// </summary>
    /// <param name="playerTeam"></param>
    /// <param name="player"></param>
    public void SetPlayerRunId(CharBase charBase)
    {
        switch (charBase.PlayerTeam)
        {
            case TeamEnum.playerTeam.Red:
                if (redCount > 4)
                {
                    Debug.Log("红方人数已达最大");
                    return;
                }
                break;
            case TeamEnum.playerTeam.Blue:
                if (blueCount > 4)
                {
                    Debug.Log("蓝方人数已达最大");
                    return;
                }
                break;
        }
        
        switch (charBase.PlayerTeam)
        {
            case TeamEnum.playerTeam.Red:
                CharManager.instance.Log("Red");
                charBase.RunId = redCount;
                redCount++;
                SelectPos(charBase);
                break;

            case TeamEnum.playerTeam.Blue:
                CharManager.instance.Log("Blue");
                charBase.RunId = blueCount + 5;
                blueCount++;
                SelectPos(charBase);
                break;
        }
    }

    /// <summary>
    /// 根据模型,CharBase信息,生成地点,玩家对应的摄像机将值赋给玩家并使模型成为其玩家摄像机的子物体
    /// </summary>
    /// <param name="playerObject"></param>
    /// <param name="charBase"></param>
    /// <param name="spawnPos"></param>
    /// <param name="playerCamera"></param>
    public void SpawnPlayerForPos(GameObject playerObject,CharBase charBase,GameObject spawnPos,Camera playerCamera)
    {
        CharBase component;

        playerObject.transform.position = spawnPos.transform.position;
        playerObject.transform.parent = playerCamera.transform;

        component = playerCamera.GetComponent<CharBase>();
        charBase.State = CharEnum.StateEnum.存活;
        //需要方便快速赋值
        CharManager.instance.GetPlayerInfo(component, charBase);
    }

}
