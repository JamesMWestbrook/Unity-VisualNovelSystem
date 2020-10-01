using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public ActorSlot CurrentActor;

    public TextMeshProUGUI MoveText;
    public List<ActorSlot> Party;

    public List<ActorSlot> Actors = new List<ActorSlot>();
    public List<ActorSlot> Enemies = new List<ActorSlot>();
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
    void DimText(){
        
    }
    
public IEnumerator DelayAction(Action action, float secondsToWait){
    yield return new WaitForSeconds(secondsToWait);
    action();
}
    // Update is called once per frame
    void Update()
    {
        
    }
}
