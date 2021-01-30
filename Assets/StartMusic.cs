using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : MonoBehaviour
{
    public Track Music;
    // Start is called before the first frame update
    void Start()
    {
        MusicManager.Main.Play(Music);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
