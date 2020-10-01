using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterCharacters : MonoBehaviour
{
    public CharacterBase Luisella = new CharacterBase()
    {
            Name = "Luisella",
            MaxStats = new CharStats(){
                HP = 300,
                MP = 50
            },
            FacePath = "Luisella Face",
            BattleOutfitPath = "BattleGraphics/Luisella/Body/LuisellaBattle",
            BattleFacePath = "BattleGraphics/Luisella/Face/LuisellaSmile"
    };
    List<Skills> LuiSkills = new List<Skills>();
    public Skills LuiCyro = new Skills(){
        Name = "Cyro I",
        Prefab = "KY_effects/MagicEffectsPackFree/prefab/ErikiBall2",
        BaseDamage = 2,
        elementType = Skills.ElementType.Cyro,
        hitType = Skills.HitType.Magical
    };
    public Skills LuisellaHeal = new Skills(){
        Name = "Heal I",
        Prefab = "KY_effects/MagicEffectsPackFree/prefab/GreenCore",
        BaseDamage = -3,
        hitType = Skills.HitType.Heal,

    };
//-----------------------------------------------------------------------
    public CharacterBase Margherita = new CharacterBase()
    {
            Name = "Margherita",
            MaxStats = new CharStats(){
                HP = 250,
                MP = 30
            },
            FacePath = "Margherita Face",
            BattleOutfitPath = "BattleGraphics/Margherita/Body/MargheritaBattle",
            BattleFacePath = "BattleGraphics/Margherita/Face/MarghNormal"
    };
    public Skills MargPunch = new Skills(){
        Name = "Square Punch",
        Prefab = "KY_effects/MagicEffectsPackFree/prefab/skillAttack2",
        BaseDamage = 2,
        elementType = Skills.ElementType.Null,
        hitType = Skills.HitType.Physical
    };
    public Skills MargShock = new Skills(){
        Name = "Currene I",
        Prefab = "KY_effects/MagicEffectsPackFree/prefab/Spark",
        BaseDamage = 2,
        elementType = Skills.ElementType.Null,
        hitType = Skills.HitType.Physical
    };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
