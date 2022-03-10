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

    private void Awake()
    {
        instance = this;
    }

    public void SpawnPlayer(int runId, string playerName, CharEnum.ProEnum pro, SkillEnum.skillID skillQ, SkillEnum.skillID skillE, SkillEnum.skillID skillR, SkillEnum.skillBurst skillBurst, TeamEnum.playerTeam playerTeam,CharEnum.PlayerEnum playerEnum)
    {
        
        CharBase charBase = new CharBase(runId,playerName,pro,skillQ,skillE,skillR,skillBurst,playerTeam);


        GameObject playerObject;
        //第一次生成
        if (runId==-1)
        {
            charBase = CharManager.instance.AddPlayerToList(playerTeam, charBase);
        }
        else//复活
        {

        }
        Debug.Log(charBase.RunId);
        switch (pro)
        {
            case CharEnum.ProEnum.近战:
                playerObject = Instantiate(solider);
                switch (playerEnum)
                {
                    case CharEnum.PlayerEnum.RedPlayer1:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos1, redPlayer1Camera);
                        break;
                    case CharEnum.PlayerEnum.RedPlayer2:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos2, redPlayer2Camera);
                        break;
                    case CharEnum.PlayerEnum.RedPlayer3:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos3, redPlayer3Camera);
                        break;
                    case CharEnum.PlayerEnum.RedPlayer4:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos4, redPlayer4Camera);
                        break;
                    case CharEnum.PlayerEnum.RedPlayer5:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos5, redPlayer5Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer1:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos1, bluePlayer1Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer2:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos2, bluePlayer2Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer3:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos3, bluePlayer3Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer4:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos4, bluePlayer4Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer5:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos5, bluePlayer5Camera);
                        break;
                }
                break;
            case CharEnum.ProEnum.远程:
                playerObject = Instantiate(archer);
                switch (playerEnum)
                {
                    case CharEnum.PlayerEnum.RedPlayer1:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos1, redPlayer1Camera);
                        break;
                    case CharEnum.PlayerEnum.RedPlayer2:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos2, redPlayer2Camera);
                        break;
                    case CharEnum.PlayerEnum.RedPlayer3:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos3, redPlayer3Camera);
                        break;
                    case CharEnum.PlayerEnum.RedPlayer4:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos4, redPlayer4Camera);
                        break;
                    case CharEnum.PlayerEnum.RedPlayer5:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos5, redPlayer5Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer1:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos1, bluePlayer1Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer2:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos2, bluePlayer2Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer3:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos3, bluePlayer3Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer4:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos4, bluePlayer4Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer5:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos5, bluePlayer5Camera);
                        break;
                }
                break;
            case CharEnum.ProEnum.辅助:
                playerObject = Instantiate(doctor);
                switch (playerEnum)
                {
                    case CharEnum.PlayerEnum.RedPlayer1:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos1, redPlayer1Camera);
                        break;
                    case CharEnum.PlayerEnum.RedPlayer2:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos2, redPlayer2Camera);
                        break;
                    case CharEnum.PlayerEnum.RedPlayer3:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos3, redPlayer3Camera);
                        break;
                    case CharEnum.PlayerEnum.RedPlayer4:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos4, redPlayer4Camera);
                        break;
                    case CharEnum.PlayerEnum.RedPlayer5:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos5, redPlayer5Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer1:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos1, bluePlayer1Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer2:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos2, bluePlayer2Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer3:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos3, bluePlayer3Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer4:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos4, bluePlayer4Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer5:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos5, bluePlayer5Camera);
                        break;
                }
                break;
            case CharEnum.ProEnum.坦克:
                playerObject = Instantiate(tanker);
                switch (playerEnum)
                {
                    case CharEnum.PlayerEnum.RedPlayer1:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos1, redPlayer1Camera);
                        break;
                    case CharEnum.PlayerEnum.RedPlayer2:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos2, redPlayer2Camera);
                        break;
                    case CharEnum.PlayerEnum.RedPlayer3:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos3, redPlayer3Camera);
                        break;
                    case CharEnum.PlayerEnum.RedPlayer4:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos4, redPlayer4Camera);
                        break;
                    case CharEnum.PlayerEnum.RedPlayer5:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnRedPos5, redPlayer5Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer1:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos1, bluePlayer1Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer2:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos2, bluePlayer2Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer3:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos3, bluePlayer3Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer4:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos4, bluePlayer4Camera);
                        break;
                    case CharEnum.PlayerEnum.BluePlayer5:
                        SpawnPlayerForPos(playerObject, playerEnum, charBase, spawnBluePos5, bluePlayer5Camera);
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// 根据模型,玩家位号,CharBase信息,生成地点,玩家对应的摄像机将值赋给玩家并使模型成为其玩家摄像机的子物体
    /// </summary>
    /// <param name="playerObject"></param>
    /// <param name="playerEnum"></param>
    /// <param name="charBase"></param>
    /// <param name="spawnPos"></param>
    /// <param name="playerCamera"></param>
    public void SpawnPlayerForPos(GameObject playerObject,CharEnum.PlayerEnum playerEnum,CharBase charBase,GameObject spawnPos,Camera playerCamera)
    {
        CharBase component;

        playerObject.transform.position = spawnPos.transform.position;
        playerObject.transform.parent = playerCamera.transform;

        component = playerCamera.GetComponent<CharBase>();


        //需要方便快速赋值

        //component.RunId = charBase.RunId;
        //component.PlayerName = charBase.PlayerName;



    }
}
