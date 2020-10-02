using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenedPanels : MonoBehaviour
{
    public List<GameObject> ButtonsToTween;
    public void BeginTweening(){
        Transform beginPoint = GameObject.Find("Outside of screen").transform;
        foreach(GameObject button in ButtonsToTween){
            Vector3 endPoint = button.transform.position;
            
            button.transform.position = new Vector3( 
                beginPoint.position.x,
                endPoint.y,
                endPoint.z
            );
            LeanTween.move(button, endPoint, 0.8f);
            button.GetComponent<RectTransform>().LeanScale( new Vector3(0,0,0), 0f);
            LeanTween.scale(button, new Vector3(1,1,1), 0.8f);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        BeginTweening();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
