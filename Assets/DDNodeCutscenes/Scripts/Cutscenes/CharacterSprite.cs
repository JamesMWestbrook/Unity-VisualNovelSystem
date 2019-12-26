using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSprite : MonoBehaviour {

    public Image Outfit;
    [HideInInspector] public Sprite CurrentOutfit;
    public Image Face;
    [HideInInspector] public Sprite CurrentFace;
    [HideInInspector] public bool IsSpeaking = false;
    [HideInInspector] public bool InScene = false;
    public bool battle = false;

    
    private void Start()
    {
        if (battle)
        {
        //    Transform newLocation = gameObject.transform;
         //   float tempY = BattleCanvas.BC.TempLocations[3].position.y;
          //  newLocation.position = new Vector3(newLocation.position.x, tempY, newLocation.position.z);
            //newLocation.transform.position    = BattleCanvas.BC.TempLocations[3].position;

            
        }
        else {
            if(!Outfit){
                Outfit = gameObject.transform.Find("outfit").GetComponent<Image>();
                Face = gameObject.transform.Find("face").GetComponent<Image>();

            }
                Outfit.enabled = false;

                Face.enabled = false;
            
        }

       // currentFace = face;
        //currentOutfit = outfit;
        

       // UIStats = gameObject.transform.GetComponentInChildren<UIContainer>();
    }
}
