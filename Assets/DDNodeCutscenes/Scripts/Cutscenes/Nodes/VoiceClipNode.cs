using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class VoiceClipNode : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}
    [Input(ShowBackingValue.Never)] public float input;
    [Output] public float output;
    public SFXObject VoiceClip;
    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port) {
		return null; // Replace this
	}
    public void OnUse()
    {
        SFXManager.Main.Play(VoiceClip, 0.1f);
        ClipGroup group = new ClipGroup();
        NodePort _port = GetOutputPort("output");
        if (_port.IsConnected)
        {
            CutsceneManager.Instance. GetNodeFunction(_port.GetConnections());
        }
    }

}