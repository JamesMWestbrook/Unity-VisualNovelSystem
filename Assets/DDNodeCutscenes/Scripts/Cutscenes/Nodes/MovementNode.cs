﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using UnityEngine.UI;
[CreateNodeMenu("VN/Movement")]
[NodeTint("#00FEFB")]
#pragma warning disable 0219
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

    public void CharacterMovement()
    {
        MovementNode moveNode = this;
        MovementNode.SpotOnScreen scopedSpotOnScreen = moveNode.spotOnScreen;
        MovementNode.EnterOrLeave movementType = moveNode.enterOrLeave;
        float _distance = CutsceneManager.Instance.MoveDistance;

        //manage face/outfit
        int _whichImage = 1;
        _whichImage = scopedSpotOnScreen.spotNumber();
        //SetSpriteOnScreen(_whichImage, scopedActor, scopedActor.baseClass.FaceID, scopedActor.baseClass.OutfitID);


        //setting up variables
        Image _image = CutsceneManager.Instance.rightCharacter;
        CharacterSprite charSprite = _image.GetComponent<CharacterSprite>();
        switch (_whichImage)
        {
            case 1:
                _image = CutsceneManager.Instance.leftCharacter;
                break;
            case 2:
                _image = CutsceneManager.Instance.midLeftCharacter;
                break;
            case 3:
                _image = CutsceneManager.Instance.midRightCharacter;
                break;
            case 4:
                _image = CutsceneManager.Instance.rightCharacter;
                break;
        }
        charSprite = _image.GetComponent<CharacterSprite>();




        Vector3 _beginPoint = new Vector3(CutsceneManager.Instance.RightSpot.transform.position.x + _distance, _image.transform.position.y, _image.transform.position.z);
        Vector3 _endPoint = new Vector3(CutsceneManager.Instance.RightSpot.position.x, _beginPoint.y, _beginPoint.z);

        float _lerpTime = 0.5f;
        float _curLerpTime = 0f;

        Image _face = charSprite.Face;
        Image _outfit = charSprite.Outfit;

        Color _faceColor = _face.color;
        Color _outfitColor = _outfit.color;


        float _startOpacity = 0;
        float _endOpacity = 1;

        float colorDim = 1f;
        if (!moveNode.IsSpeaking)
        {
            colorDim = 0.7f;
        }

        float _startDim = charSprite.Outfit.color.r;
        float _endDim = colorDim;

        if (!scopedSpotOnScreen.IsRight())
        {
            Transform LeftSpot = CutsceneManager.Instance.LeftSpot;

            _beginPoint = new Vector3(LeftSpot.transform.position.x - _distance, _image.transform.position.y, _image.transform.position.z);
            _endPoint = new Vector3(LeftSpot.position.x, _beginPoint.y, _beginPoint.z);
            // _endPoint = _beginPoint + Vector3.right * distance;
            if (scopedSpotOnScreen.IsMiddle())
            {
            }
        }
        else if (scopedSpotOnScreen.IsMiddle())
        {
        }

        bool inScene = false;
        if (movementType.IsLeaving())
        {
            Vector3 _newStart = new Vector3(_endPoint.x, _endPoint.y, _endPoint.z);
            Vector3 _newEnd = new Vector3(_beginPoint.x, _endPoint.y, _endPoint.z);
            _beginPoint = _newStart;
            _endPoint = _newEnd;
            _startOpacity = 1f;
            _endOpacity = 0f;
            CutsceneManager.Instance.activeImages.Remove(_outfit);
            CutsceneManager.Instance.activeImages.Remove(_face);
        }
        else
        {
            CutsceneManager.Instance.activeImages.Add(_outfit);
            CutsceneManager.Instance.activeImages.Add(_face);
            inScene = true;
        }
        //move
        _image.transform.position = _beginPoint;
        LeanTween.move(_image.gameObject, _endPoint, _lerpTime);
        // _face.color = new Color(1,1,1,0);
        // _outfit.color = new Color(1,1,1,0);
        
        LeanTween.value(_face.gameObject, new Color(1, 1, 1, _startOpacity), new Color(1, 1, 1, _endOpacity), _lerpTime).setOnUpdate(
            (Color val) =>
            {
                UnityEngine.UI.Image image = (UnityEngine.UI.Image)_face.gameObject.GetComponent(typeof(UnityEngine.UI.Image));
                image.color = val;
            }
        );
        LeanTween.value(_outfit.gameObject, new Color(1, 1, 1, _startOpacity), new Color(1, 1, 1, _endOpacity), _lerpTime).setOnUpdate(
            (Color val) =>
            {
                UnityEngine.UI.Image image = (UnityEngine.UI.Image)_outfit.gameObject.GetComponent(typeof(UnityEngine.UI.Image));
                image.color = val;
            }
        );
    }
    public static void ColorChange(GameObject game_object, Color from, Color to, float time)
    {
        LeanTween.value(game_object, from, to, time).setOnUpdate(
            (Color val) =>
            {
                UnityEngine.UI.Image image = (UnityEngine.UI.Image)game_object.GetComponent(typeof(UnityEngine.UI.Image));
                image.color = val;
            }
        );
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