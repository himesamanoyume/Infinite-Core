using Photon.Pun;

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

    //public Camera redPlayer1Camera;
    //public Camera redPlayer2Camera;
    //public Camera redPlayer3Camera;
    //public Camera redPlayer4Camera;
    //public Camera redPlayer5Camera;

    //public Camera bluePlayer1Camera;
    //public Camera bluePlayer2Camera;
    //public Camera bluePlayer3Camera;
    //public Camera bluePlayer4Camera;
    //public Camera bluePlayer5Camera;

    int redCount = 0;
    int blueCount = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
        
            SpawnPlayer(-1, "test1", ProEnum.��ս, SkillID.tempQ, SkillID.tempE, SkillID.tempR, SkillBurst.����, TeamEnum.Red);
        

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
    public void SpawnPlayer(int runId, string playerName, ProEnum pro, SkillID skillQ, SkillID skillE, SkillID skillR, SkillBurst skillBurst, TeamEnum playerTeam)
    {
        CharBase charBase = new CharBase(runId,playerName,pro,skillQ,skillE,skillR,skillBurst,playerTeam);

        //��һ������
        if (runId==-1)
        {
            Debug.Log("��һ������,SpawnPlayer������");
            SetPlayerRunId(charBase);
            
        }
        else
        {
            Debug.LogError("SpawnPlayer�ǳ�״̬������");
        }
        
    }

    /// <summary>
    /// ������ҵĸ����֧
    /// </summary>
    /// <param name="charBase"></param>
    public void SpawnPlayer(CharBase charBase)
    {
        charBase.State = StateEnum.���;
        charBase.RespawnTime += 5f;
        GameObject playerModel = CharManager.Instance.FindChildObjWithTag("PlayerModel", charBase.gameObject);
        playerModel.SetActive(true);
        playerModel.transform.position = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// �����ѡ������λ��
    /// </summary>
    /// <param name="charBase"></param>
    public void SelectPos(CharBase charBase)
    {
        GameObject playerObject;
        Debug.LogWarning("����������λ��");
        switch (charBase.Pro)
        {
            case ProEnum.��ս:
                playerObject = PhotonNetwork.Instantiate(solider.name, new Vector3(0, 0, 0), Quaternion.identity);
                
                switch (charBase.RunId)
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
            case ProEnum.Զ��:
                playerObject = PhotonNetwork.Instantiate(archer.name, new Vector3(0, 0, 0), Quaternion.identity);
                switch (charBase.RunId)
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
            case ProEnum.����:
                playerObject = PhotonNetwork.Instantiate(doctor.name, new Vector3(0, 0, 0), Quaternion.identity);
                switch (charBase.RunId)
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
            case ProEnum.̹��:
                playerObject = PhotonNetwork.Instantiate(tanker.name, new Vector3(0, 0, 0), Quaternion.identity);
                switch (charBase.RunId)
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

    public void RespawnSelectPos(GameObject playerObject, CharBase charBase)
    {
        //�˴�playerObjectΪplayerModel
        switch (charBase.RunId)
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
    /// �������
    /// </summary>
    /// <param name="runId"></param>
    /// <param name="playerName"></param>
    public void RespawnPlayer(int runId, string playerName)
    {
        CharBase charBase = CharManager.Instance.FindPlayerById(runId);
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
            case TeamEnum.Red:
                if (redCount > 4)
                {
                    Debug.Log("�췽�����Ѵ����");
                    return;
                }
                break;
            case TeamEnum.Blue:
                if (blueCount > 4)
                {
                    Debug.Log("���������Ѵ����");
                    return;
                }
                break;
        }
        
        switch (charBase.PlayerTeam)
        {
            case TeamEnum.Red:
                CharManager.Instance.Log("Red");
                charBase.RunId = redCount;
                redCount++;
                SelectPos(charBase);
                break;

            case TeamEnum.Blue:
                CharManager.Instance.Log("Blue");
                charBase.RunId = blueCount + 5;
                blueCount++;
                SelectPos(charBase);
                break;
        }
    }

    /// <summary>
    /// ����ģ��,CharBase��Ϣ,���ɵص�,��Ҷ�Ӧ��ֵ�������
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
        charBase.State = StateEnum.���;
        //��Ҫ������ٸ�ֵ
        CharManager.Instance.GetPlayerInfo(component, charBase);
    }
    //public void SpawnPlayerForPos(GameObject playerObject, CharBase charBase, GameObject spawnPos)
    //{
    //    CharBase component;

    //    playerObject.transform.position = spawnPos.transform.position;
    //    //playerObject.transform.parent = playerCamera.transform;

    //    component = mainCamera.GetComponent<CharBase>();
    //    charBase.State = CharEnum.StateEnum.���;
    //    //��Ҫ������ٸ�ֵ
    //    CharManager.instance.GetPlayerInfo(component, charBase);
    //}

    //public void SelectPos(CharBase charBase)
    //{
    //    GameObject playerObject;
    //    switch (charBase.Pro)
    //    {
    //        case CharEnum.ProEnum.��ս:
    //            playerObject = Instantiate(solider);
    //            switch (charBase.RunId)
    //            {
    //                case 0:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos1, redPlayer1Camera);
    //                    break;
    //                case 1:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos2, redPlayer2Camera);
    //                    break;
    //                case 2:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos3, redPlayer3Camera);
    //                    break;
    //                case 3:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos4, redPlayer4Camera);
    //                    break;
    //                case 4:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos5, redPlayer5Camera);
    //                    break;
    //                case 5:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos1, bluePlayer1Camera);
    //                    break;
    //                case 6:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos2, bluePlayer2Camera);
    //                    break;
    //                case 7:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos3, bluePlayer3Camera);
    //                    break;
    //                case 8:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos4, bluePlayer4Camera);
    //                    break;
    //                case 9:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos5, bluePlayer5Camera);
    //                    break;
    //            }
    //            break;
    //        case CharEnum.ProEnum.Զ��:
    //            playerObject = Instantiate(archer);
    //            switch (charBase.RunId)
    //            {
    //                case 0:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos1, redPlayer1Camera);
    //                    break;
    //                case 1:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos2, redPlayer2Camera);
    //                    break;
    //                case 2:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos3, redPlayer3Camera);
    //                    break;
    //                case 3:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos4, redPlayer4Camera);
    //                    break;
    //                case 4:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos5, redPlayer5Camera);
    //                    break;
    //                case 5:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos1, bluePlayer1Camera);
    //                    break;
    //                case 6:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos2, bluePlayer2Camera);
    //                    break;
    //                case 7:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos3, bluePlayer3Camera);
    //                    break;
    //                case 8:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos4, bluePlayer4Camera);
    //                    break;
    //                case 9:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos5, bluePlayer5Camera);
    //                    break;
    //            }
    //            break;
    //        case CharEnum.ProEnum.����:
    //            playerObject = Instantiate(doctor);
    //            switch (charBase.RunId)
    //            {
    //                case 0:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos1, redPlayer1Camera);
    //                    break;
    //                case 1:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos2, redPlayer2Camera);
    //                    break;
    //                case 2:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos3, redPlayer3Camera);
    //                    break;
    //                case 3:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos4, redPlayer4Camera);
    //                    break;
    //                case 4:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos5, redPlayer5Camera);
    //                    break;
    //                case 5:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos1, bluePlayer1Camera);
    //                    break;
    //                case 6:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos2, bluePlayer2Camera);
    //                    break;
    //                case 7:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos3, bluePlayer3Camera);
    //                    break;
    //                case 8:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos4, bluePlayer4Camera);
    //                    break;
    //                case 9:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos5, bluePlayer5Camera);
    //                    break;
    //            }
    //            break;
    //        case CharEnum.ProEnum.̹��:
    //            playerObject = Instantiate(tanker);
    //            switch (charBase.RunId)
    //            {
    //                case 0:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos1, redPlayer1Camera);
    //                    break;
    //                case 1:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos2, redPlayer2Camera);
    //                    break;
    //                case 2:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos3, redPlayer3Camera);
    //                    break;
    //                case 3:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos4, redPlayer4Camera);
    //                    break;
    //                case 4:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnRedPos5, redPlayer5Camera);
    //                    break;
    //                case 5:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos1, bluePlayer1Camera);
    //                    break;
    //                case 6:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos2, bluePlayer2Camera);
    //                    break;
    //                case 7:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos3, bluePlayer3Camera);
    //                    break;
    //                case 8:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos4, bluePlayer4Camera);
    //                    break;
    //                case 9:
    //                    SpawnPlayerForPos(playerObject, charBase, spawnBluePos5, bluePlayer5Camera);
    //                    break;
    //            }
    //            break;
    //    }

    //}
}
