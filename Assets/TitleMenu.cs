using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TitleMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject StartSelect;
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(StartSelect);
    }
    public GameObject CurrentlySelected;
    void Update()
    {
        if (UAP_AccessibilityManager.IsEnabled()) return;
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            CurrentlySelected = EventSystem.current.currentSelectedGameObject;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(CurrentlySelected);
        }
    }
}
