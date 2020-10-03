using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class Skills 
{
    public string Name;
    public string Prefab;
    [HorizontalGroup]
    public float BaseDamage;
    [HorizontalGroup]
    public float DestructTimer;
    public bool Friendly;
    [EnumToggleButtons]
    public TargetCount targetCount;
    [EnumToggleButtons]
    public HitType hitType;
    [EnumToggleButtons]
    public ElementType elementType;
    //Affects dead party member
    bool AffectsDead;
    public enum TargetCount
    {
        Single,
        Multiple
    }
    public enum HitType{
        Physical,
        Magical,
        Heal
    }
    public enum ElementType { 
        Cyro,
        Solar,
        Terrene,
        Pyro,
        Lunar,
        Currene,
        Null
    }
    public void Action(List<GameObject> targets, GameObject user){
        
        Debug.Log("Clicked");
        BattleManager bm = GameManager.Instance.BattleManager;
        bm.UpdateMove(GameManager.Instance.BattleManager.CurrentActor.Actor.Name);
        //end turn
        bm.PostSkill(DestructTimer + 0.3f);

        //close all UI
        //advance to next turn
    }
    

}
