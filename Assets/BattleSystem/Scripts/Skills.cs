using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skills 
{
    public string Name;
    public string Prefab;
    public float DestructTimer;
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
        
        GameManager.Instance.BattleManager.UpdateMove(user.GetComponent<CharacterBase>().Name);
        //end turn
        GameManager.Instance.BattleManager.PostSkill(DestructTimer + 0.3f);
    }
    

}
