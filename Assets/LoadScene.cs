using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public bool DebugSkip;
   [SerializeField] public string scene;
    // Start is called before the first frame update
    void Start()
    {
        if (DebugSkip) SceneManager.LoadScene(scene, LoadSceneMode.Single);
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Load()
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
