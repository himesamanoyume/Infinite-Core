using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{

    SkillType type;

    string skillName;

    int level = 1;

    float cd;

    float skillRange;

    float[] damage;

    public SkillBase(SkillType type, string skillName, int level, float cd, float skillRange, float[] damage)
    {
        this.type = type;
        this.skillName = skillName;
        this.level = level;
        this.cd = cd;
        this.skillRange = skillRange;
        this.damage = damage;
    }

    public SkillType Type { get => type; set => type = value; }
    public string SkillName { get => skillName; set => skillName = value; }
    public int Level { get => level; set => level = value; }
    public float Cd { get => cd; set => cd = value; }
    public float SkillRange { get => skillRange; set => skillRange = value; }
    public float[] Damage { get => damage; set => damage = value; }


}
