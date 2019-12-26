using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using UnityEngine.UI;

[CreateNodeMenu(("VN/SetSpriteNode"))]
public class SetSpriteNode : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

    [Input(ShowBackingValue.Never)] public float input;
    [Output] public float output;
    //private CharacterSpriteHolder csh;
    [HideInInspector]public CharacterImages actor;

    public Sprite Outfit;
    public Sprite Face;

    public MovementNode.SpotOnScreen Spot;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}

public static class SetSpriteExtension
{
    /*
    public static int spotNumber(this SetSpriteNode sNode)
    {

        switch (sNode.actor.spot)
        {
            case MovementNode.SpotOnScreen.Left:
                return 1;

            case MovementNode.SpotOnScreen.MidLeft:
                return 2;

            case MovementNode.SpotOnScreen.MidRight:
                return 3;

            case MovementNode.SpotOnScreen.Right:
                return 4;
        }

        return 1;
    }
*/
}