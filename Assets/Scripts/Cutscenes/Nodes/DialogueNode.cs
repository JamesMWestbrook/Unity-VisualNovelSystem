using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using UnityEngine.UI;

[CreateNodeMenu("VN/DialogueNode")]
public class DialogueNode : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}
    [Input(ShowBackingValue.Never)] public float input;
    [Output] public float output;
    [HideInInspector] public WhoIsSpeaking whoIsSpeaking;
    [HideInInspector] public string Speaker;
    [HideInInspector] public string Dialogue;
    public bool IsMoving = false;

    public enum WhoIsSpeaking
    {
        Left,Right
    }

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {


	    return null; // Replace this
	}

}

public static class DialogueNodeExtension
{
    public static bool IsLeft(this DialogueNode.WhoIsSpeaking w)
    {

        return w == DialogueNode.WhoIsSpeaking.Left;
    }
}