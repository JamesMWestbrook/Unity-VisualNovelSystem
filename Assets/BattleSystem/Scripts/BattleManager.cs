using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public TextMeshProUGUI MoveText;
    public List<ActorSlot> ActorSlots;

    public void UpdateMove(string moveName){
        MoveText.text = moveName;
        MoveText.maxVisibleCharacters = 0;
        LeanTween.value(MoveText.gameObject, (float x) => MoveText.maxVisibleCharacters = (int)x, 0,MoveText.text.Length, 2f);
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateMove("smh smh smh smh");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
