using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharEnum : MonoBehaviour
{
    public enum ProEnum
    {
        ��ս,//��ս Soilder
        Զ��,//Զ�� Archer
        ����,//���� Doctor
        ̹��//̹�� Tanker
    }

    public enum BuffEnum
    {
        ����,
        �ٻ�,
        ��Ѫ,
        ��Ĭ,
        ��ʴ,//ϵͳBuff
        �޺�//ӵ�и�buffʱ�޷�����
    }

    public enum StateEnum
    {
        ���,
        ��������,
        ������
        
    }

    public enum PlayerEnum
    {
        RedPlayer1,
        RedPlayer2,
        RedPlayer3,
        RedPlayer4,
        RedPlayer5,
        BluePlayer1,
        BluePlayer2,
        BluePlayer3,
        BluePlayer4,
        BluePlayer5
    }
}



