using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Characters : MonoBehaviour
{
    public CharacterBase Christine = new CharacterBase()
    {
        Name = "Christine",
        NickName = "Cray Cray",
        MaxStats = new CharStats()
        {
            HP = 50,
            MP = 30,
            Muscle = 3,
            Vigor = 5,
            Will = 4,
            Instinct = 5,
            Agility = 3,

            Normal = 1,
            Currene = 1,
            Terrene = 1,
            Pyro = 1,
            Cyro = 1,
            Solar = 1,
            Lunar = 1,
        },
        Lvl = 3
    };


}
