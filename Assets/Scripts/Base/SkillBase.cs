using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    /// <summary>
    /// 技能位置类型
    /// </summary>
    SkillType type;

    /// <summary>
    /// 技能名称
    /// </summary>
    string skillName;

    /// <summary>
    /// 技能等级
    /// </summary>
    int level = 1;

    /// <summary>
    /// 该技能的cd
    /// </summary>
    float cd;

    /// <summary>
    /// 当前cd
    /// </summary>
    float currentCd;

    /// <summary>
    /// 是否正在施放
    /// </summary>
    bool isRelease;

    //float skillRange;

    //float[] damage;    


   

    Dictionary<int, SkillData> data;
        //次序，相对坐标，大小，倍率，

    /// <summary>
    /// 技能构造器
    /// </summary>
    /// <param name="type">技能位置类型</param>
    /// <param name="skillName">技能名</param>
    /// <param name="level">等级 建议为1</param>
    /// <param name="cd">冷却时间</param>
    /// <param name="data">技能施放数据</param>
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
                Debug.LogWarning("等级不合法");
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
                Debug.LogWarning("当前cd不合法");
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
    /// 初始碰撞体 center
    /// </summary>
    Vector3 initCenter;
    /// <summary>
    /// 初始碰撞体 size
    /// </summary>
    Vector3 initSize;
    /// <summary>
    /// 最终碰撞体 center
    /// </summary>
    Vector3 finalCenter;
    /// <summary>
    /// 最终碰撞体 size
    /// </summary>
    Vector3 finalSize;
    /// <summary>
    /// 变化时间
    /// </summary>
    float initToFinalTime;
    /// <summary>
    /// 攻击力倍率
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
