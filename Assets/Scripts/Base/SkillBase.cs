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


   

    Dictionary<int, SkillData> data;
        //����������꣬��С�����ʣ�

    /// <summary>
    /// ���ܹ�����
    /// </summary>
    /// <param name="type">����λ������</param>
    /// <param name="skillName">������</param>
    /// <param name="level">�ȼ� ����Ϊ1</param>
    /// <param name="cd">��ȴʱ��</param>
    /// <param name="data">����ʩ������</param>
    public SkillBase(SkillType type, string skillName, int level, float cd,Dictionary<int,SkillData> data)
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

    public Dictionary<int, SkillData> Data
    {
        get => data;
        set => data = value;
    }
    //public float SkillRange { get => skillRange; set => skillRange = value; }
    //public float[] Damage { get => damage; set => damage = value; }

    public bool IsRelease { get; set; }
}

public class SkillData
{
    /// <summary>
    /// ��ʼ��ײ�� center
    /// </summary>
    Vector3 initCenter;
    /// <summary>
    /// ��ʼ��ײ�� size
    /// </summary>
    Vector3 initSize;
    /// <summary>
    /// ������ײ�� center
    /// </summary>
    Vector3 finalCenter;
    /// <summary>
    /// ������ײ�� size
    /// </summary>
    Vector3 finalSize;
    /// <summary>
    /// �仯ʱ��
    /// </summary>
    float initToFinalTime;
    /// <summary>
    /// ����������
    /// </summary>
    float ratio;

    public Vector3 InitCenter { get; set; }
    public Vector3 InitSize { get; set; }
    public Vector3 FinalCenter { get; set; }
    public Vector3 FinalSize { get; set; }
    public float InitToFinalTime { get; set; }
    public float Ratio { get; set; }

    public SkillData(Vector3 initCenter, Vector3 initSize, Vector3 finalCenter, Vector3 finalSize, float initToFinalTime, float ratio) 
    {
        this.initCenter = initCenter;
        this.initSize = initSize;
        this.finalCenter = finalCenter;
        this.finalSize = finalSize;
        this.initToFinalTime = initToFinalTime;
        this.ratio = ratio;

    }
}
