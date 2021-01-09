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
        ActorSlot Defender = user.GetComponent<ActorSlot>();
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
            modifier = BaseDamage * tempAttack - tempDefense; //This is where we'd plug elements in
            Defender.Actor.CurStats.HP -= modifier;
        }
        else
        {
            switch (hitType)
            {
                case Skills.HitType.Heal:
                    tempAttack = Attacker.Actor.CurStats.Vigor;
                    modifier = BaseDamage * tempAttack;
                    Defender.Actor.CurStats.HP += modifier;
                    break;
                case Skills.HitType.Status: //not touched in demo
                    break;
            }
        }
    }
}

