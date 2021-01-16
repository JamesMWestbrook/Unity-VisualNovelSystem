using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class Skills
{
    public string Name;
    public ResourcePathAsset<GameObject> Prefab;
    [HorizontalGroup]
    public int BaseDamage;
    [HorizontalGroup]
    public float DestructTimer;
    public Vector3 Rotation;
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
    public enum HitType
    {
        Physical,
        Magical,
        Heal,
        Status
    }
    public enum ElementType
    {
        Cyro,
        Solar,
        Terrene,
        Pyro,
        Lunar,
        Currene,
        Null
    }
    public void Action(List<GameObject> targets, GameObject user)
    {

        BattleManager bm = GameManager.Instance.BattleManager;
        bm.UpdateMove(GameManager.Instance.BattleManager.CurrentActor.Actor.Name);

        //activate skill

        //feed BM a prefab to spawn
        for (int i = 0; i < targets.Count; i++)
        {
            bm.SpawnGO(Prefab.Asset, targets[i].transform, DestructTimer);
            SkillProcess(targets[i], user, hitType);
            //run check for damage
        }

        //end turn
        bm.PostSkill(DestructTimer + 0.3f);
        //close all UI
        //advance to next turn
    }

    public void SkillProcess(GameObject target, GameObject user, Skills.HitType hitType)
    {
        int tempAttack = 0;
        int tempDefense = 0;
        int modifier;
        ActorSlot Attacker = user.GetComponent<ActorSlot>();
        ActorSlot Defender = target.GetComponent<ActorSlot>();
        if (hitType == Skills.HitType.Physical || hitType == Skills.HitType.Magical)
        {
            tempDefense = Defender.Actor.CurStats.Will;
            switch (hitType)
            {
                case Skills.HitType.Physical:
                    tempAttack = Attacker.Actor.CurStats.Muscle;
                    break;
                case Skills.HitType.Magical:
                    tempAttack = Attacker.Actor.CurStats.Vigor;
                    break;
            }
            Debug.Log(tempAttack);
            modifier = BaseDamage * tempAttack - tempDefense; //This is where we'd plug elements in
            if(modifier < 0) modifier = 0;

            //this is where we would calculate magic affecting the damage, here it would not matter if 
            //it was below 0, since magic damage can turn into negative
            Debug.Log("----");
            Debug.Log(string.Format(" {0} HP before: {1}", Name, Defender.Actor.CurStats.HP));
           // Debug.Log(Name + "HP Before using "  + "  " + Defender.Actor.CurStats.HP);
            Debug.Log("Modifier " + modifier);
            Defender.Actor.CurStats.HP -= modifier;
            if(Defender.Actor.CurStats.HP < 0) Defender.Actor.CurStats.HP = 0;
            if(Defender.Actor.CurStats.HP > Defender.Actor.MaxStats.HP) Defender.Actor.CurStats.HP = Defender.Actor.MaxStats.HP;
            Debug.Log("HP After " + Defender.Actor.CurStats.HP);

        }
        else
        {
            switch (hitType)
            {
                case Skills.HitType.Heal:
                    tempAttack = Attacker.Actor.CurStats.Vigor;
                    modifier = BaseDamage * tempAttack;
                    Debug.Log("HP Before " + Defender.Actor.CurStats.HP);
                    Defender.Actor.CurStats.HP += modifier;
                    Debug.Log("Modifier " + modifier);
                    Debug.Log("HP After " + Defender.Actor.CurStats.HP);
                    break;
                case Skills.HitType.Status: //not touched in demo
                    break;
            }
        }
    }
}

