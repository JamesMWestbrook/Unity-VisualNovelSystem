using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class Cutscene : NodeGraph {


    public CutsceneRootNode GetRootNode()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] as CutsceneRootNode) return nodes[i] as CutsceneRootNode;
        }
        return null;
    }
}