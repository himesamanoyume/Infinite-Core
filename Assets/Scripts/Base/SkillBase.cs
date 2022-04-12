using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    /// <summary>
    /// ����λ������
    /// </summary>
    SkillType type;

    /// <summary>
    /// ��������
    /// </summary>
    string skillName;

    /// <summary>
    /// ���ܵȼ�
    /// </summary>
    int level = 1;

    /// <summary>
    /// �ü��ܵ�cd
    /// </summary>
    float cd;

    /// <summary>
    /// ��ǰcd
    /// </summary>
    float currentCd;

    /// <summary>
    /// �Ƿ�����ʩ��
    /// </summary>
    bool isRelease;

    //float skillRange;

    //float[] damage;    


   

    Dictionary<int, AttackCubeData> data;
        //����������꣬��С�����ʣ�

    /// <summary>
    /// ���ܹ�����
    /// </summary>
    /// <param name="type">����λ������</param>
    /// <param name="skillName">������</param>
    /// <param name="level">�ȼ� ����Ϊ1</param>
    /// <param name="cd">��ȴʱ��</param>
    /// <param name="data">����ʩ������</param>
    public SkillBase(SkillType type, string skillName, int level, float cd,Dictionary<int,AttackCubeData> data)
    {
        this.type = type;
        this.skillName = skillName;
        this.level = level;
        this.cd = cd;
        this.data = data;
        //this.skillRange = skillRange;
        //this.damage = damage;
    }

    public SkillType Type { get => type; set => type = value; }

    public string SkillName { get => skillName; set => skillName = value; }

    public int Level 
    { 
        get => level;
        set 
        {
            if (value>15||value<0)
            {
                Debug.LogWarning("�ȼ����Ϸ�");
            }
            else
            {
                level = value;
            }
        } 
    }

    public float Cd { get => cd; set => cd = value; }

    public float CurrentCd
    {
        get => currentCd;
        set
        {
            if (value>cd || value<0)
            {
                Debug.LogWarning("��ǰcd���Ϸ�");
            }
            else
            {
                currentCd = value;
            }
        }
    }

    public Dictionary<int, AttackCubeData> Data
    {
        get => data;
        set => data = value;
    }
    //public float SkillRange { get => skillRange; set => skillRange = value; }
    //public float[] Damage { get => damage; set => damage = value; }

    public bool IsRelease { get; set; }
}


