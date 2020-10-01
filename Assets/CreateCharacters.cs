using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCharacters : MonoBehaviour
{
    private StarterCharacters Starter = new StarterCharacters();
    // Start is called before the first frame update
    void Awake()
    {
        List<ActorSlot> _actors = GetComponent<BattleManager>().Party;
        _actors[0].Actor = Starter.Luisella;

        List<Skills> LuiSkills = new List<Skills>(){
            Starter.LuiCyro,
            Starter.LuisellaHeal
        };
        _actors[0].Actor.Skills = LuiSkills;


        _actors[1].Actor = Starter.Margherita;

        for (int i = 0; i < _actors.Count; i++)
        {
           CharacterBase _curActor = _actors[i].Actor;
            _curActor.CurStats = _curActor.MaxStats.DeepStatsCopy( _curActor.MaxStats);

            _actors[i].SetGraphics();
            _actors[i].UpdateStats();
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
