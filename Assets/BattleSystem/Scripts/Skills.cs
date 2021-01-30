using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class Skills
{
    public string Name;
    public ResourcePathAsset<GameObject> Prefab;
    public int Cost;
    public int LevelLearned;
    public bool Learned;
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
    public string AnimTrigger;
    //Affects dead party member
    public bool AffectsDead;
    [TextArea] public string Description;
    public ResourcePathAsset<SFXObject> SoundEffect;
    public float EffectDelay;
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
        bool ai = user.GetComponent<ActorSlot>().IsAI;
        BattleManager bm = GameManager.Instance.BattleManager;
        bm.UpdateMove(GameManager.Instance.BattleManager.CurrentActor.Actor.Name);
        bm.InTargetMenu = false;
        bm.buttonState = BattleManager.ButtonState.First;
        //feed BM a prefab to spawn
        //ally spawn
        if (Prefab.Asset && !targets[0].GetComponent<ActorSlot>().IsAI)
        {
            bm.SpawnGO(Prefab.Asset, GameManager.Instance.BattleManager.EffectTrans, DestructTimer);
        }
        //enemy spawn
        for (int i = 0; i < targets.Count; i++)
        {
            if (Prefab.Asset && targets[0].GetComponent<ActorSlot>().IsAI)
            {
                bm.SpawnGO(Prefab.Asset, targets[i].transform.Find("EffectTrans"), DestructTimer);
            }
            SkillProcess(targets[i], user, hitType);
            //run check for damage
        }
        GameManager.Instance.BattleManager.Defender = targets[0].GetComponent<ActorSlot>();

        if (ai)
        {
            Animator anim = user.GetComponent<Animator>();
            anim.SetTrigger(AnimTrigger);
        }
        //end turn
        bm.PostSkill(DestructTimer );
        bm.PlaySingleSFX(this);
    }

    public void SkillProcess(GameObject target, GameObject user, Skills.HitType hitType)
    {
        BattleManager bm = GameManager.Instance.BattleManager;

        bm.IsHealing = false;
        ActorSlot actor = user.GetComponent<ActorSlot>();
        if (!actor.IsAI)
        {
            actor.Actor.CurStats.MP -= Cost;
            float curMP = actor.Actor.CurStats.MP;
            float result = curMP / actor.Actor.MaxStats.MP;
            actor.MPForeground.fillAmount = result;
            actor.MP.text = curMP.ToString();

        }


        int tempAttack = 0;
        int tempDefense = 0;
        int modifier;
        int popupText;
        ActorSlot Attacker = user.GetComponent<ActorSlot>();
        ActorSlot Defender = target.GetComponent<ActorSlot>();
        string readThis = "";
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
            if (modifier < 0) modifier = 0;

            //this is where we would calculate magic affecting the damage, here it would not matter if 
            //it was below 0, since magic damage can turn into negative
            // Debug.Log(Name + "HP Before using "  + "  " + Defender.Actor.CurStats.HP);
            Defender.Actor.CurStats.HP -= modifier;
            if (Defender.Actor.CurStats.HP < 0) Defender.Actor.CurStats.HP = 0;
            if (Defender.Actor.CurStats.HP > Defender.Actor.MaxStats.HP) Defender.Actor.CurStats.HP = Defender.Actor.MaxStats.HP;

            popupText = modifier * -1;

            if (UAP_AccessibilityManager.IsEnabled())
            { bm.StartSpawn(0f, popupText, Defender); }
            else {
                bm.StartSpawn(DestructTimer, popupText, Defender);
                if (!actor.IsAI) {
                    if (Name == "Attack") SFXManager.Main.Play(actor.Actor.AttackVoices.Get());
                    else SFXManager.Main.Play(actor.Actor.DefaultSpecialVoices.Get());
                }
            }


            readThis = string.Format("{0} hits {1} for {2} damage.", user.GetComponent<ActorSlot>().Actor.Name, Defender.Actor.Name, modifier.ToString());
            //UAP_AccessibilityManager.Say()
        }
        else
        {
            switch (hitType)
            {
                case Skills.HitType.Heal:
                    bm.IsHealing = true;
                    tempAttack = Attacker.Actor.CurStats.Vigor;
                    modifier = BaseDamage * tempAttack;
                    popupText = Defender.Actor.CurStats.HP;
                    Defender.Actor.CurStats.HP += modifier;
                    if (Defender.Actor.CurStats.HP > Defender.Actor.MaxStats.HP) Defender.Actor.CurStats.HP = Defender.Actor.MaxStats.HP;
                    popupText = Defender.Actor.CurStats.HP - popupText;
                    if (UAP_AccessibilityManager.IsEnabled())
                    {
                        bm.StartSpawn(0f, popupText, Defender);
                    }
                    else
                    {
                        if (!actor.IsAI)
                        {
                            SFXManager.Main.Play(actor.Actor.HealingVoices.Get());
                        }
                        bm.StartSpawn(DestructTimer, popupText, Defender);
                    }
                    readThis = string.Format("{0} heals {1} for {2} health.", user.GetComponent<ActorSlot>().Actor.Name, Defender.Actor.Name, modifier.ToString());
                    break;
                case Skills.HitType.Status: //not touched in demo
                    break;
            }
        }
        UAP_AccessibilityManager.Say(readThis, false);
    }

}

