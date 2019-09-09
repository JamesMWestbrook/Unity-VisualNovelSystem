using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using XNodeEditor;
using XNode;
using UnityEditor;
using UnityEngine.UI;


/*
 * Originally written since I had other functions on DialogueNodes, 
 * but am no longer using this since I have other nodes handle
 * Sprite changes. Keeping this for reference. 
 * */
[CustomNodeEditor(typeof(DialogueNode))]
public class DialogueNodeEditor : NodeEditor {


    public override void OnBodyGUI()
    {
        base.OnBodyGUI();
        SerializedProperty speakerProp = serializedObject.FindProperty("Speaker");
        SerializedProperty dialogueProp = serializedObject.FindProperty("Dialogue");
        SerializedProperty whoIsSpeaking = serializedObject.FindProperty("whoIsSpeaking");

        EditorGUILayout.LabelField("SpeakerName");
        EditorGUILayout.PropertyField(speakerProp, GUIContent.none);

        EditorGUILayout.LabelField("Dialogue");
        EditorGUILayout.PropertyField(dialogueProp, GUIContent.none);

        EditorGUILayout.LabelField("Speaker");
        EditorGUILayout.PropertyField(whoIsSpeaking, GUIContent.none);
    }
}
