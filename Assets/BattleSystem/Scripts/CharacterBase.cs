using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Sirenix.Serialization;

[System.Serializable]
public class CharacterBase
{
    public string Name = "Empty";
    public string NickName = "EmptyNN";
    public int Lvl = 1;
    public int EXP;
    public StatType statType;
    public CharStats MaxStats = new CharStats();
    public CharStats CurStats = new CharStats();
    [OdinSerialize] public List<Skills> Skills = new List<Skills>();
    public ResourcePathAsset<Sprite> ShowTest;
    public ResourcePathAsset<Sprite> FacePath;
    public string SpriteGUI;
    public ResourcePathAsset<Sprite> BattleOutfitPath;
    public ResourcePathAsset<Sprite> BattleFacePath;
    public ResourcePathAsset<SFXObject> DefaultAttackSound;
    //[SerializeField] public Dictionary<int, BaseBattleActions> BaseBattleActions;

    //public Weapon WeaponOne;

    //public EquipmentBase Head;
    //public EquipmentBase Body;
    //public EquipmentBase AccOne;
    //public EquipmentBase AccTwo;
    public enum StatType
    {
        Healer,
        Aggressive
    }
    public CharacterBase DeepClone()
    {
        CharacterBase cb = new CharacterBase();
        DeepClone(cb);
        return cb;
    }
    public virtual void DeepClone(CharacterBase dst)
    {
        dst.Name = Name;
        dst.ShowTest = ShowTest;
    }

    public virtual void Test(string test)
    {
        Debug.Log(this.Name);
    }
    public int EXPToLevel()
    {
        int EXPNeeded = 0;
        EXPNeeded = Lvl + Lvl * 20;
        return EXPNeeded;
    }
    public void LevelUp(){
        do{
        EXP -= EXPToLevel();
        Lvl++;
        }while(EXP > EXPToLevel());
        StatIncrease();
    }
    public void StatIncrease()
    {
        switch (statType)
        {
            case StatType.Aggressive:
                MaxStats.HP = GetGrowth(22, 7);
                MaxStats.MP = GetGrowth(40, 4);
                MaxStats.Muscle = GetGrowth(3,2);
                MaxStats.Vigor = GetGrowth(2);
                MaxStats.Will = GetGrowth(2);
                MaxStats.Instinct = GetGrowth(3);
                MaxStats.Agility = GetGrowth(2);

                break;
            case StatType.Healer:
                MaxStats.HP = GetGrowth(22, 8);
                MaxStats.MP = GetGrowth(40, 6);
                MaxStats.Muscle = GetGrowth(2);
                MaxStats.Vigor = GetGrowth(3,2);
                MaxStats.Will = GetGrowth(3);
                MaxStats.Instinct = GetGrowth(3);
                MaxStats.Agility = GetGrowth(3);
                break;
        }
    }
    int GetGrowth(int input, int inputTwo = 0)
    {
        int NewStat = input * Lvl + inputTwo * Lvl;
        return NewStat;
    }
}


public static class CharacterExtensions
{
    public static CharacterBase Clone<T>(this CharacterBase data, T other)
    {

        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, other);
            ms.Position = 0;
            return (CharacterBase)formatter.Deserialize(ms);
        }
    }

}