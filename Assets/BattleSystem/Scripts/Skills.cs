using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skills 
{
    public string Name;
    public string Prefab;

    public float BaseDamage;
    
    public HitType hitType;
    public ElementType elementType;
    public enum HitType{
        Physical,
        Magical,
        Heal
    }
    public enum ElementType{
        Null,
        Cyro,
        Currene,
        Solar
    }
    public void Action(List<GameObject> targets, GameObject user){
        
        //end turn        
    }


}
