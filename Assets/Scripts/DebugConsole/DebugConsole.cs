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
    public static DebugCommand<int> GET_NAME;
    public static DebugCommand<int,int> SET_MOVESPEED;
    public static DebugCommand<int,int> LEVEL_UP;
    public static DebugCommand<int, int> SET_HP;
    public static DebugCommand<int, int, int> GIVE_EXP;
    public static DebugCommand<int, int, int> GIVE_MONEY;

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
        CharManager charManager = GameObject.Find("CharManager").GetComponent<CharManager>();
        //����
        TEST = new DebugCommand("Test", "���Hello World", "Test", () =>
        {
            Debug.Log("Hello World");
        });

        GET_NAME = new DebugCommand<int>("GetName", "����id��ȡ�������", "GetName <id>", (id) =>
        {
            charManager.GetPlayerName(id);
        });

        LEVEL_UP = new DebugCommand<int,int>("LevelUp", "����id��������ض�������1���ȼ�", "LevelUp <id> <count>", (id,count) =>
           {
               charManager.SetPlayerLevel(id, count);
           });

        SET_HP = new DebugCommand<int, int>("SetHp", "ͨ��id�޸���ҵ�ǰѪ��", "SetHp <id> <health>", (id, health) =>
           {
               charManager.SetPlayerCurrentHealth(id, health);
           });

        GIVE_EXP = new DebugCommand<int, int, int>("GiveExp", "ͨ��id������������Χ�ڵľ���", "GiveExp <id> <min> <max>", (id, min, max) =>
          {
              charManager.ToGivePlayerCurrentExp(id, min, max);
          });

        GIVE_MONEY = new DebugCommand<int, int, int>("GiveMoney", "ͨ��id������������Χ�ڵľ���", "GiveMoney <id> <min> <max>", (id, min, max) =>
        {
            charManager.SetPlayerMoney(id, min, max);
        });

        SET_MOVESPEED = new DebugCommand<int, int>("SetMoveSpeed", "ͨ��id�����ƶ��ٶ�", "SetMoveSpeed <id> <speed>", (id, speed) =>
          {
              charManager.SetPlayerMoveSpeed(id, speed);
          });

        //������Ž��б� 
        //ע�����һ����������ж��� ������޷�����
        commandList = new List<object>
        {
            TEST,
            GET_NAME,
            LEVEL_UP,
            SET_HP,
            GIVE_EXP,
            GIVE_MONEY,
            SET_MOVESPEED
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
