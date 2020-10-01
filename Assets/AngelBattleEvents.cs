using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelBattleEvents : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartOfBattle(){
        Debug.Log("Battle started");
    }

    public void EndOfTurn(){
        
        
        Debug.Log("Turn ended");
        StartCoroutine(DelayTurn());
        }
    IEnumerator DelayTurn(){
        GameManager.Instance.BattleManager.DelayNextTurn = true;
        yield return new WaitForSeconds(5f);
        GameManager.Instance.BattleManager.DelayNextTurn = false;
    }
    public void EndOfBattle(){Debug.Log("Battle ended");}
    public void StartOfTurn(){Debug.Log("Turn started");}

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
