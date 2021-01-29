using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class SpeechButton : MonoBehaviour
{
    public string ReadThis;
    public string altRead;
    private string temp;
    public bool value;
    public TitleMenu titleMenu;
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
            titleMenu.AccEnabled = true;
            UAP_AccessibilityManager.EnableAccessibility(true);
            
            value = true;
            gameObject.name = "Press Enter To Disable Accessibility";
            GetComponentInChildren<TextMeshProUGUI>().text = "Disable Accessibility";
            titleMenu.Reset();
        }
    }
}
