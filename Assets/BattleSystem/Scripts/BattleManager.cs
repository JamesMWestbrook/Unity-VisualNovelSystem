using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{
    public ActorSlot CurrentActor;
    public int TurnCounter;
    [Header(header: "Current actor's turn")]
    public int ActorCounter;

    public TextMeshProUGUI MoveText;
    public List<ActorSlot> Party;

    public List<ActorSlot> Actors;
    public List<ActorSlot> Enemies;
    public void UpdateMove(string moveName)
    {
        MoveText.text = moveName;
        MoveText.maxVisibleCharacters = 0;
        LeanTween.value(MoveText.gameObject, (float x) => MoveText.maxVisibleCharacters = (int)x, 0, MoveText.text.Length, 2f);
    }
    // Start is called before the first frame update

    public UnityEvent StartOfBattle;
    public UnityEvent StartOfTurn;
    public UnityEvent EndOfTurn;
    public UnityEvent EndOfBattle;



    void Start()
    {
        StartBattle();
    }
    void DimText()
    {

    }
    public void StartBattle()
    {
        StartOfBattle.Invoke();
    }
    public void NextActor()
    {
        ActorCounter++;
        if (ActorCounter >= Actors.Count)
        {
            ActorCounter = 0;
            StartCoroutine(DelayTurn());
        }
        else CurrentActor = Actors[ActorCounter];
        StartCoroutine(DelayActor());
    }
    public bool DelayNextTurn;

    public IEnumerator DelayTurn()
    {
        EndOfTurn.Invoke();
        do
        {
            yield return null;
        } while (DelayNextTurn);
        NextTurn();
    }
    public void StartActor()
    {
        Debug.Log(CurrentActor);
        if (!CurrentActor.IsAI)
        {

            CharacterSprite charSprite = CutsceneManager.Instance.rightCharacter.GetComponent<CharacterSprite>();
            charSprite.Face.enabled = true;
            charSprite.Outfit.enabled = true;

            charSprite.Face.sprite = Resources.Load<Sprite>(CurrentActor.Actor.BattleFacePath);
            charSprite.Outfit.sprite = Resources.Load<Sprite>(CurrentActor.Actor.BattleOutfitPath);

            
            //set sprite
            //move actor image in

            /*  SetSpriteNode SetSprite = new SetSpriteNode();
              SetSprite.Face = Resources.Load<Sprite>(CurrentActor.Actor.BattleFacePath);
              SetSprite.Outfit = Resources.Load<Sprite>(CurrentActor.Actor.BattleOutfitPath);
              SetSprite.Spot = MovementNode.SpotOnScreen.Right;

              CutsceneManager.Instance.SetImage(SetSprite); */
        }
    }
    public IEnumerator DelayActor()
    {
        do
        {
            yield return null;
        } while (DelayNextTurn);
        StartActor();
    }
    public void NextTurn()
    {
        CurrentActor = Actors[ActorCounter];
        TurnCounter++;
        StartOfTurn.Invoke();
    }
    public IEnumerator DelayAction(Action action, float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        action();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
