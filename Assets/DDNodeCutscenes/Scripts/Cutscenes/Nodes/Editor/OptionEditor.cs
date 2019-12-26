using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

[CustomNodeEditor((typeof(OptionNode)))]
public class OptionEditor : NodeEditor{

    public override void OnBodyGUI()
    {

        SerializedProperty inputProperty = serializedObject.FindProperty("input");
        NodeEditorGUILayout.PropertyField(inputProperty);

        SerializedProperty outputProperty = serializedObject.FindProperty("output");
        NodeEditorGUILayout.PropertyField(outputProperty);

        //literally just show the textbox but without the title "Option Text" since it takes up space and is redundant. 
        SerializedProperty textProp = serializedObject.FindProperty("optionText");
        EditorGUILayout.PropertyField(textProp, GUIContent.none);
    }
}
