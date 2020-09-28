using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterCharacters
{
    public CharacterBase Luisella = new CharacterBase()
    {
            Name = "Luisella",
            MaxStats = new CharStats(){
                HP = 300,
                MP = 50
            },
            FacePath = "Luisella Face"
    };
    public CharacterBase Margherita = new CharacterBase()
    {
            Name = "Margherita",
            MaxStats = new CharStats(){
                HP = 250,
                MP = 30
            },
            FacePath = "Margherita Face"
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
