using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("VN/CGHide")]
[NodeTint("#FCFF00")]
public class CGHide : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}
    [Input(ShowBackingValue.Never)] public float input;
    [Output] public float output;
    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}