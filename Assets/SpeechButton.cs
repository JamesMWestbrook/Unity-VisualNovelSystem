using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class SpeechButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string ReadThis;
    public string altRead;
    private string temp;
    public bool value;

    public void Switch()
    {
        if (value)
        {
            UAP_AccessibilityManager.EnableAccessibility(false);
            value = false;
            gameObject.name = "Press Enter To Enable Accessibility";
            GetComponentInChildren<TextMeshProUGUI>().text = "Enable Accessibility";

        }
        else
        {
            UAP_AccessibilityManager.EnableAccessibility(true);
            UAP_AccessibilityManager.Say("Accessibility is enabled.");
            value = true;
            gameObject.name = "Press Enter To Disable Accessibility";
            GetComponentInChildren<TextMeshProUGUI>().text = "Disable Accessibility";
        }
    }
}
