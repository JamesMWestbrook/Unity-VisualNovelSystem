using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu(("VN/RootNode"))]
[NodeTint("#AF1F00")]
public class CutsceneRootNode : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

    [Output] public float beginScene;



	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}