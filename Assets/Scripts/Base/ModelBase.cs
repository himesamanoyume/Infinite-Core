using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelBase : MonoBehaviour
{
    /// <summary>
    /// 用于记录该模型其主人的runId;
    /// </summary>
    private int runId;

    public int RunId { get => runId; set => runId = value; }
}
