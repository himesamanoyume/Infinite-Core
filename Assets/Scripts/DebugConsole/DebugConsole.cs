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

    //声明命令处
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
        //实现命令处

        //测试
        TEST = new DebugCommand("Test", "输出Hello World", "Test", () =>
        {
            Debug.Log("Hello World");
        });

        SPAWN_PLAYER = new DebugCommand<int>("SpawnPlayer", "生成一个名为'test1',近战,超速爆发的玩家,阵营为team=(0,1),0为红,1为蓝", "SpawnPlayer <team>", (team) =>
        {
            if (team != 0 && team != 1)
            {
                Debug.LogError("输入值非0或1");
                return;
            }
            if (team == 0)//red
            {
                CharSpawnController.instance.SpawnPlayer(-1, "test1", CharEnum.ProEnum.近战, SkillEnum.skillID.tempQ, SkillEnum.skillID.tempE, SkillEnum.skillID.tempR, SkillEnum.skillBurst.超速, TeamEnum.playerTeam.Red);

            }
            else//blue
            {
                CharSpawnController.instance.SpawnPlayer(-1, "test1", CharEnum.ProEnum.近战, SkillEnum.skillID.tempQ, SkillEnum.skillID.tempE, SkillEnum.skillID.tempR, SkillEnum.skillBurst.超速, TeamEnum.playerTeam.Blue);

            }
        });

        GET_PLAYERNAME_BY_ID = new DebugCommand<int>("GetPlayerNameById", "根据id获取玩家姓名", "GetPlayerNameById <id>", (id) =>
        {
            CharManager.instance.GetPlayerNameById(id);
        });

        PLAYER_LEVEL_UP_BY_ID = new DebugCommand<int,int>("PlayerLevelUp", "根据id提升玩家特定次数的1级等级", "PlayerLevelUp <id> <count>", (id,count) =>
           {
               CharManager.instance.PlayerLevelUp(id,count);
           });

        GET_ALL_PLAYER_INFO = new DebugCommand("GetAllPlayerInfo", "获取所有已生成的玩家的信息", "GetAllPlayerInfo", () =>
        {
            //CharManager.instance.GetAllPlayer();
        });

        SET_PLAYER_HP_BY_ID = new DebugCommand<int, int>("SetPlayerHpById", "通过id修改玩家当前血量", "SetPlayerHpById <id> <health>", (id, health) =>
           {
               CharManager.instance.SetPlayerHealth(id, health);
           });

        TO_GIVE_PLAYER_EXP = new DebugCommand<int, int, int>("ToGivePlayerExp", "通过id给予玩家随机范围内的经验", "ToGivePlayerExp <id> <min> <max>", (id, min, max) =>
          {
              CharManager.instance.ExpChange(id, min, max);
          });

        TO_GIVE_PLAYER_MONEY = new DebugCommand<int, int, int>("ToGivePlayerMoney", "通过id给予玩家随机范围内的经验", "ToGivePlayerMoney <id> <min> <max>", (id, min, max) =>
        {
            CharManager.instance.MoneyChange(id, min, max);
        });

        TO_GIVE_PLAYER_HP = new DebugCommand<int, int>("ToGivePlayerHp", "通过id给予或扣除玩家指定血量", "ToGivePlayerHp <id> <health>", (id, health) =>
           {
               CharManager.instance.HealthChange(id, health);
           });

        //将命令放进列表 
        //注意最后一个命令后不能有逗号 否则会无法调用
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
