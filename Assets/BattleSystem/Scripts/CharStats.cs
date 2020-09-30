using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


[System.Serializable]
public class CharStats
{
    public int HP = 50;
    public int MP = 30;

    //Modifiers
    public int Muscle = 3; //Phys Attack
    public int Vigor = 5; //Magic Attack
    public int Will = 4; //Defense, both magic and physical
    public int Instinct = 5; //Higher = less misses on enemy,
                             //more misses from enemy 
    public int Agility = 3; //Determines turn order



    //Elements
    public float Normal = 1;
    public float Currene = 1;
    public float Terrene = 1;
    public float Pyro = 1;
    public float Cyro = 1;
    public float Solar = 1;
    public float Lunar = 1;

}

public static class CStatsExt
{
    public static CharStats DeepStatsCopy<T>(this CharStats cStats, T other)
    {

        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, other);
            ms.Position = 0;
            return (CharStats)formatter.Deserialize(ms);
        }
    }

}