using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharEnum : MonoBehaviour
{
    public enum ProEnum
    {
        近战,//近战 Soilder
        远程,//远程 Archer
        辅助,//辅助 Doctor
        坦克//坦克 Tanker
    }

    public enum BuffEnum
    {
        定身,
        迟缓,
        流血,
        沉默,
        侵蚀,//系统Buff
        无核//拥有该buff时无法复活
    }

    public enum StateEnum
    {
        存活,
        彻底死亡,
        复活中
        
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



