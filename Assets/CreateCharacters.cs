using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCharacters : MonoBehaviour
{
    public CharacterSO Luisella;
    public CharacterSO Margh;
    // Start is called before the first frame update
    void Awake()
    {
        List<ActorSlot> _actors = GetComponent<BattleManager>().Party;
        _actors[0].Actor = Luisella.Character.Clone(Luisella.Character);
        _actors[1].Actor = Margh.Character.Clone(Margh.Character);

        for (int i = 0; i < _actors.Count; i++)
        {
            CharacterBase _curActor = _actors[i].Actor;
            _curActor.CurStats = _curActor.MaxStats.DeepStatsCopy(_curActor.MaxStats);

            _actors[i].SetGraphics();
            _actors[i].UpdateStats();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
