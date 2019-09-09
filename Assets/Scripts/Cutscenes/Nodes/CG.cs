using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNode;

[CreateNodeMenu("VN/CG")]
[NodeTint("#FCFF00")]
public class CG : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

    [Input(ShowBackingValue.Never)] public float input;

    [Output] public float output;

    public Sprite CGGraphic;
    public bool waitForInput;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}