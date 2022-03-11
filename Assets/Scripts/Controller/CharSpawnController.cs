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
    /// ���ڵ�һ���������
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

        //��һ������
        if (runId==-1)
        {
            SetPlayerRunId(charBase);
        }
        else
        {
            Debug.Log("SpawnPlayer�ǳ�״̬������");
        }
        
    }

    /// <summary>
    /// ������ҵĸ����֧
    /// </summary>
    /// <param name="charBase"></param>
    public void SpawnPlayer(CharBase charBase)
    {
        charBase.State = CharEnum.StateEnum.���;
        charBase.RespawnTime += 10f;
        GameObject playerModel = CharManager.instance.FindChildObjWithTag("PlayerModel", charBase.gameObject);
        playerModel.SetActive(true);

    }

    /// <summary>
    /// �����ѡ������λ�ú͹��ص������
    /// </summary>
    /// <param name="charBase"></param>
    public void SelectPos(CharBase charBase)
    {
        GameObject playerObject;
        switch (charBase.Pro)
        {
            case CharEnum.ProEnum.��ս:
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
            case CharEnum.ProEnum.Զ��:
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
            case CharEnum.ProEnum.����:
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
            case CharEnum.ProEnum.̹��:
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
        //�˴�playerObjectΪplayerModel
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
    /// �������
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
    /// ����һ�������ʱ����Ҹ���һ��runId
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
                    Debug.Log("�췽�����Ѵ����");
                    return;
                }
                break;
            case TeamEnum.playerTeam.Blue:
                if (blueCount > 4)
                {
                    Debug.Log("���������Ѵ����");
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
    /// ����ģ��,CharBase��Ϣ,���ɵص�,��Ҷ�Ӧ���������ֵ������Ҳ�ʹģ�ͳ�Ϊ������������������
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
        charBase.State = CharEnum.StateEnum.���;
        //��Ҫ������ٸ�ֵ
        CharManager.instance.GetPlayerInfo(component, charBase);
    }

}
