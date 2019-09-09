using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[CreateNodeMenu("VN/Movement")]
[NodeTint("#00FEFB")]
public class MovementNode : Node
{

    [Input(ShowBackingValue.Never)] public float input;
    [Output] public float output;

    public EnterOrLeave enterOrLeave;
    public SpotOnScreen spotOnScreen;
    public bool IsSpeaking = false;

    public enum EnterOrLeave
    {
        enterFrom,
        leaveTo,
        moveTo
    }

    public enum SpotOnScreen
    {
        Left,
        MidLeft,
        MidRight,
        Right
    }

    public override object GetValue(NodePort port)
    {
        return base.GetValue(port);
    }
}

public static class MovementNodeExtension
{
    public static bool isMoving(this MovementNode.EnterOrLeave e)
    {
        return e == MovementNode.EnterOrLeave.moveTo;
    }
    public static int spotNumber(this MovementNode.SpotOnScreen s)
    {
        switch (s)
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

    public static bool IsLeaving(this MovementNode.EnterOrLeave s)
    {
        return s == MovementNode.EnterOrLeave.leaveTo;
    }

    public static bool IsRight(this MovementNode.SpotOnScreen s)
    {
        return s == MovementNode.SpotOnScreen.Right || s == MovementNode.SpotOnScreen.MidRight;
    }

    public static bool IsMiddle(this MovementNode.SpotOnScreen s)
    {
        return s == MovementNode.SpotOnScreen.MidLeft || s == MovementNode.SpotOnScreen.MidRight;
    }
}