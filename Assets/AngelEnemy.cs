using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelEnemy : MonoBehaviour, IEnemy
{
    public void AI()
    {
        List <GameObject> targets = new List<GameObject>();
        int targetIndex = Random.Range(0, 2);
        Debug.Log(targetIndex);
        targets.Add(GameObject.Find("BattleManager").GetComponent<BattleManager>().Party[targetIndex].gameObject);
        NormalAttack.Action(targets, gameObject);
    }


    public Skills NormalAttack = new Skills(){
    };

        public Skills DualSlash = new Skills(){

    };    public Skills BlindingSolar = new Skills(){
            Name = "Blinding Solar Light"
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
