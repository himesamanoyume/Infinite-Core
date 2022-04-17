using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Skill : MonoBehaviour 
{
    
    
    private void Start()
    {
        #region TempQ1
        Dictionary<int, AttackCubeData> tempQ1data = new Dictionary<int, AttackCubeData>();

        tempQ1data.Add(1, new AttackCubeData(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), 0.5f, 1,0,3));

        SkillBase tempQ1 = new SkillBase(SkillType.Q, "TempQ1", 1, 20, tempQ1data);
        #endregion
    }

    



}

