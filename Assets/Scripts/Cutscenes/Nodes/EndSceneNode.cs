using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint("#AF1F00")]
public class EndSceneNode : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

    [Input(ShowBackingValue.Never)] public float input;


    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}