using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    /// <summary>
    /// skill ID
    /// </summary>
    public int skillID;

    public SkillEnum.skillType skillType;

    public int level;

    public float cd;

    public float skillRange;
}