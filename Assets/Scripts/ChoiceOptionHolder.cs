using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using XNode;
using UnityEngine.EventSystems;

public class ChoiceOptionHolder : MonoBehaviour
{
    private Button thisButton;
    public int indexOfOptions;
    public Node node;
    //private CutsceneManager CM;

    public void Start()
    {
        //CM = CutsceneManager.cutsceneManager;
    }
   public void ButtonClicked()
    {
        CutsceneManager.cutsceneManager.dialoguePanel.SetActive(true);
        foreach(Transform child in CutsceneManager.cutsceneManager.ChoicePanel.transform)
        {
            Destroy(child.gameObject);
        }
        CutsceneManager.cutsceneManager.ChoicePanel.SetActive(false);


        if (node)
        {

            NodePort scopedOutput = node.GetOutputPort("output");
            if (scopedOutput.IsConnected)
            {
            NodePort scopedNextInput = scopedOutput.Connection;
                List<NodePort> _outputList = new List<NodePort>();
                _outputList.Add(scopedOutput.Connection);

                CutsceneManager.cutsceneManager.GetNodeFunction(_outputList);
            }
        }
        else
        {
            Debug.Log("No node in button");
        }

    }

}
