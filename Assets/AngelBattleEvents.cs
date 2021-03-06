﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelBattleEvents : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartOfBattle(){
        StartCoroutine(DelayCutscene());
        Debug.Log("Battle started");
    }
    public Cutscene cutscene;
    public void EndOfTurn(){
        
        StartCoroutine(DelayTurn());
        }
    IEnumerator DelayTurn(){
        GameManager.Instance.BattleManager.DelayNextTurn = true;
        yield return new WaitForSeconds(1f);
        GameManager.Instance.BattleManager.DelayNextTurn = false;
    }
    public void EndOfBattle(){Debug.Log("Battle ended");}
    public void StartOfTurn(){
        Debug.Log("Turn started");
    }

    void Start()
    {
        
    }
    public IEnumerator DelayCutscene()
    {
        yield return new WaitForSeconds(0.01f);
        CutsceneManager.Instance.PlayCutscene(cutscene);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
