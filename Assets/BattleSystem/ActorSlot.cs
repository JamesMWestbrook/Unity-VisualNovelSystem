using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ActorSlot : MonoBehaviour
{
    public Image Face;
    public CharacterBase Actor;
    public void SetGraphics(){
        Debug.Log(Actor.FacePath);
        Face.sprite = Resources.Load<Sprite>(Actor.FacePath);
    }


    public TextMeshProUGUI HP;
    public TextMeshProUGUI MP;
    public void UpdateStats(){
        HP.text = Actor.CurStats.HP.ToString();
        MP.text = Actor.CurStats.MP.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
