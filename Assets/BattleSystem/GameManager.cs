using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public BattleManager BattleManager;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance != null) Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UAP_AccessibilityManager.IsEnabled())
            {
                UAP_AccessibilityManager.EnableAccessibility(false);
                UAP_AccessibilityManager.Say("Accessibility is disabled.");
            }
            else
            {
                UAP_AccessibilityManager.EnableAccessibility(true);
                UAP_AccessibilityManager.Say("Accessibility is enabled.");
            }
        }
       }
}
