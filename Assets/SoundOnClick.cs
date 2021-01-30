using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SoundOnClick : MonoBehaviour, ISelectHandler
{
    public SFXObject Sound;
    public bool FirstButton;
    public void OnSelect(BaseEventData data)
    {
       if(!FirstButton) SFXManager.Main.Play(Sound,0);
        FirstButton = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
