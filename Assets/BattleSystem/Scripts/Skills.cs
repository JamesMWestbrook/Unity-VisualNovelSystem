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
            if(targets[i].GetComponent<ActorSlot>().IsAI)bm.SpawnGO(Prefab.Asset, targets[i].GetComponent<ActorSlot>().EffectTrans, DestructTimer);
            else bm.SpawnGO(Prefab.Asset, bm.EffectTrans, DestructTimer);
            //run check for damage
        }

        //end turn
        bm.PostSkill(DestructTimer + 0.3f);
        //close all UI
        //advance to next turn
    }

    void SkillProcess()
    {
        switch (hitType)
        {
            case HitType.Physical:

                break;
            case HitType.Magical:

                break;
            case HitType.Heal:

                break;
            case HitType.Status:

                break;

        }
    }

}
