using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleCutscene : MonoBehaviour {

    CutsceneManager cutsceneManager;

    public Cutscene scene;
    private bool hasStarted = false;
    // Use this for initialization
    void Start()
    {
        cutsceneManager = GameObject.Find("CutsceneManager").GetComponent<CutsceneManager>();
        cutsceneManager.PlayCutscene(scene);
    }

    private void Update()
    {
        if (!hasStarted) {
            if (Input.GetButtonDown("Submit"))
            {
                //cutsceneManager.PlayCutscene(scene);
                Destroy(this.gameObject);
            }
        }
        
    }
}
