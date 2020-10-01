using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

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

            // +100
            Transform dest = CutsceneManager.Instance.RightSpot;
            Image rightImage = CutsceneManager.Instance.rightCharacter;

            Vector3 _beginPoint = new Vector3(dest.position.x + 100, rightImage.transform.position.y, rightImage.transform.position.z);
            Vector3 _endPoint = new Vector3(CutsceneManager.Instance.RightSpot.position.x, _beginPoint.y, _beginPoint.z);

            float time = 0.4f;

            Image _face = charSprite.Face;
            Image _outfit = charSprite.Outfit;
            rightImage.transform.position = _beginPoint;
            LeanTween.move(rightImage.gameObject,  _endPoint, time);

            MovementNode.ColorChange(_outfit.gameObject, new Color(0.7f,0.7f,0.7f,1), new Color(1,1,1,1),time);
            MovementNode.ColorChange(_face.gameObject, new Color(0.7f,0.7f,0.7f,1), new Color(1,1,1,1),time);
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
