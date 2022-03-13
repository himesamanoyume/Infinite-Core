using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugConsole : MonoBehaviour
{
    bool showConsole;

    string input;

    public List<object> commandList;

    //�������
    public static DebugCommand TEST;
    public static DebugCommand<int> SPAWN_PLAYER;
    public static DebugCommand<int> GET_PLAYERNAME_BY_ID;
    public static DebugCommand<int,int> PLAYER_LEVEL_UP_BY_ID;
    public static DebugCommand GET_ALL_PLAYER_INFO;
    public static DebugCommand<int, int> SET_PLAYER_HP_BY_ID;
    public static DebugCommand<int, int, int> TO_GIVE_PLAYER_EXP;
    public static DebugCommand<int, int, int> TO_GIVE_PLAYER_MONEY;
    public static DebugCommand<int, int> TO_GIVE_PLAYER_HP;

    public void OnToggleDebug(InputValue value)
    {
        showConsole = !showConsole;
    }

    public void OnReturn(InputValue value)
    {
        if (showConsole)
        {
            HandleInput();
            input = "";
        }
    }

    private void HandleInput()
    {
        string[] properties = input.Split(' ');

        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (input.Contains(commandBase.commandId))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if (commandList[i] as DebugCommand<int> != null)
                {
                    (commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                }
                else if (commandList[i] as DebugCommand<int,int> != null)
                {
                    (commandList[i] as DebugCommand<int,int>).Invoke(int.Parse(properties[1]),int.Parse(properties[2]));
                }
                else if (commandList[i] as DebugCommand<int, int, int> != null)
                {
                    (commandList[i] as DebugCommand<int, int, int>).Invoke(int.Parse(properties[1]), int.Parse(properties[2]), int.Parse(properties[3]));
                }
            }
        }
    }

    private void Awake()
    {
        //ʵ�����

        //����
        TEST = new DebugCommand("Test", "���Hello World", "Test", () =>
        {
            Debug.Log("Hello World");
        });

        SPAWN_PLAYER = new DebugCommand<int>("SpawnPlayer", "����һ����Ϊ'test1',��ս,���ٱ��������,��ӪΪteam=(0,1),0Ϊ��,1Ϊ��", "SpawnPlayer <team>", (team) =>
        {
            if (team != 0 && team != 1)
            {
                Debug.LogError("����ֵ��0��1");
                return;
            }
            if (team == 0)//red
            {
                CharSpawnController.instance.SpawnPlayer(-1, "test1", CharEnum.ProEnum.��ս, SkillEnum.skillID.tempQ, SkillEnum.skillID.tempE, SkillEnum.skillID.tempR, SkillEnum.skillBurst.����, TeamEnum.playerTeam.Red);

            }
            else//blue
            {
                CharSpawnController.instance.SpawnPlayer(-1, "test1", CharEnum.ProEnum.��ս, SkillEnum.skillID.tempQ, SkillEnum.skillID.tempE, SkillEnum.skillID.tempR, SkillEnum.skillBurst.����, TeamEnum.playerTeam.Blue);

            }
        });

        GET_PLAYERNAME_BY_ID = new DebugCommand<int>("GetPlayerNameById", "����id��ȡ�������", "GetPlayerNameById <id>", (id) =>
        {
            CharManager.instance.GetPlayerNameById(id);
        });

        PLAYER_LEVEL_UP_BY_ID = new DebugCommand<int,int>("PlayerLevelUp", "����id��������ض�������1���ȼ�", "PlayerLevelUp <id> <count>", (id,count) =>
           {
               CharManager.instance.PlayerLevelUp(id,count);
           });

        GET_ALL_PLAYER_INFO = new DebugCommand("GetAllPlayerInfo", "��ȡ���������ɵ���ҵ���Ϣ", "GetAllPlayerInfo", () =>
        {
            //CharManager.instance.GetAllPlayer();
        });

        SET_PLAYER_HP_BY_ID = new DebugCommand<int, int>("SetPlayerHpById", "ͨ��id�޸���ҵ�ǰѪ��", "SetPlayerHpById <id> <health>", (id, health) =>
           {
               CharManager.instance.SetPlayerHealth(id, health);
           });

        TO_GIVE_PLAYER_EXP = new DebugCommand<int, int, int>("ToGivePlayerExp", "ͨ��id������������Χ�ڵľ���", "ToGivePlayerExp <id> <min> <max>", (id, min, max) =>
          {
              CharManager.instance.ExpChange(id, min, max);
          });

        TO_GIVE_PLAYER_MONEY = new DebugCommand<int, int, int>("ToGivePlayerMoney", "ͨ��id������������Χ�ڵľ���", "ToGivePlayerMoney <id> <min> <max>", (id, min, max) =>
        {
            CharManager.instance.MoneyChange(id, min, max);
        });

        TO_GIVE_PLAYER_HP = new DebugCommand<int, int>("ToGivePlayerHp", "ͨ��id�����۳����ָ��Ѫ��", "ToGivePlayerHp <id> <health>", (id, health) =>
           {
               CharManager.instance.HealthChange(id, health);
           });

        //������Ž��б� 
        //ע�����һ����������ж��� ������޷�����
        commandList = new List<object>
        {
            TEST,
            SPAWN_PLAYER,
            GET_PLAYERNAME_BY_ID,
            PLAYER_LEVEL_UP_BY_ID,
            //GET_ALL_PLAYER_INFO,
            SET_PLAYER_HP_BY_ID,
            TO_GIVE_PLAYER_EXP,
            TO_GIVE_PLAYER_MONEY,
            TO_GIVE_PLAYER_HP
        };
    }

    Vector2 scroll;

    private void OnGUI()
    {
        if (!showConsole) { return; }

        GUIStyle fontStyle = new GUIStyle();

        fontStyle.fontSize = (int)(Screen.height * 0.03f);

        fontStyle.normal.textColor = new Color(1, 1, 1);

        float y = 0f;

        GUI.Box(new Rect(0, y, Screen.width, Screen.height * 0.2f), "");

        Rect viewport = new Rect(0, 0, Screen.width*0.99f, Screen.height*0.05f * commandList.Count);

        scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, Screen.height * 0.19f), scroll, viewport);

        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase command = (DebugCommandBase)commandList[i];

            string label = command.commandFormat + " - " + command.commandDescription;

            Rect labelRect = new Rect(5, Screen.height * 0.05f * i, viewport.width - Screen.height * 0.2f, Screen.height * 0.05f);

            GUI.Label(labelRect, label, fontStyle);
        }

        GUI.EndScrollView();

        y += Screen.height * 0.2f;
        

        GUI.Box(new Rect(0, y, Screen.width, Screen.height * 0.05f), "");

        GUI.backgroundColor = new Color(0, 0, 0, 0);

        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - Screen.height * 0.2f, Screen.height * 0.05f), input, fontStyle);

    }
}
