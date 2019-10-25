using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor.Experimental.UIElements.GraphView;
using XNode;
using Node = XNode.Node;

/*CutsceneManager
 * Does all of the calculations involved when running cutscenes. 
 * Yes paper the naming scheme isn't that great(publics not being capitalized) I'll fix it while ya'll are testing it :P
 * Rn I'm just gonna focus on making sure it's good enough for testing, and it's clear what I'm doing. 
    */
public class CutsceneManager : MonoBehaviour {
    
    public static CutsceneManager Instance;
    //private GameManager GM; //not necessary for purely cutscene stuff

    private bool inCutscene = false;
    [SerializeField] private DialogueNode currentDialogue;
    private CG currentCGNode;
    private bool CGWaitingForInput = false;

    [Header("")]
    public GameObject ChoicePanel;
    [SerializeField] private Button choiceButton;

    private List<XNode.Node> nodeOptions;
    public bool selecting;//will not be needed until I implement keyboard controls
    public GameObject currentlySelected;// ^

    [Header("")]
    [SerializeField] private Image _cgGraphic;
    [SerializeField] public GameObject dialoguePanel;
    [SerializeField] public GameObject speakerPanel;
    [SerializeField] private TextMeshProUGUI dialogueTMP;
    [SerializeField] private TextMeshProUGUI speakerTMP;

    [SerializeField] public List<Image> activeImages;

    //points used for reference when moving characters
    [Header("")]
    [SerializeField] public Transform LeftSpot;
    [SerializeField] private Transform MidLeftSpot;
    [SerializeField] private Transform MidRightSpot;
    [SerializeField] public Transform RightSpot;

    //people in cutscene
    [Header("")]
    [SerializeField] private GameObject _charactersInScene;
    [SerializeField] public Image leftCharacter;
    [SerializeField] public Image midLeftCharacter;
    [SerializeField] public Image rightCharacter;
    [SerializeField] public Image midRightCharacter;

    //default move speed



    // Use this for initialization
    void Awake () {

        if(Instance != null)
        {
            GameObject.Destroy(this);
            return;
        }
        Instance = this;


        /*if (GM == null)
	    {
	       // GM = GameObject.Find("GameManager").GetComponent<GameManager>();

	    }
*/
        DontDestroyOnLoad(gameObject);

        //disable UI
	    if (_cgGraphic)
	    {
	        _cgGraphic.enabled = false;
	    }

	    if (ChoicePanel)
	    {
            ChoicePanel.SetActive(false);
	    }

        if(dialoguePanel){
            dialoguePanel.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update ()
	{
	    PlayerInput();
    }

    public void PlayerInput()
    {
        if (inCutscene)
        {
            if (Input.GetButtonDown("Submit"))
            {
                if (CGWaitingForInput)
                {
                    if (currentCGNode.GetOutputPort("output").IsConnected)
                    {
                        dialoguePanel.SetActive(true);
                        _charactersInScene.SetActive(true);
                        NodePort _port = currentCGNode.GetOutputPort("output");
                        GetNodeFunction(_port.GetConnections());
                        CGWaitingForInput = false;
                    }
                }
                else
                {//when not relying on CG input and simply waiting on dialogue
                    if (currentDialogue.GetOutputPort("output").IsConnected)
                    {
                        NodePort _port = currentDialogue.GetOutputPort("output");
                        GetNodeFunction(_port.GetConnections()); //GetConnections returns the list of ports connected to _port
                    }
                }
            }
        }
    }

    public void PlayCutscene(Cutscene cutscene)
    {
        inCutscene = true;

        CutsceneRootNode _rootNode = cutscene.GetRootNode();
        NodePort _port = _rootNode.GetOutputPort("beginScene"); 
        if (_port.IsConnected)
        {
            GetNodeFunction(_port.GetConnections());
        }
        else
        {
            Debug.Log("No root node");
        }
        
    }

    public void GetNodeFunction(List<NodePort> _ports )
    {
       
        //cannot find a way to use a switch, so we'll have to stick with a bunch of if statements :notlikethis:
        foreach(NodePort _nodePort in _ports)
        {
            XNode.Node _node = _nodePort.node;
            if (_node.GetType() == typeof(DialogueNode))
            {
                dialoguePanel.SetActive(true);
                currentDialogue = _node as DialogueNode;
                ShowDialogue(currentDialogue);
                return; //do NOT grab the node after this, and wait for the player to advance the dialogue
            }



            if (_node.GetType() == typeof(EndSceneNode))
            {
                EndCutscene();
                return;
            }
            if (_node is MovementNode movementNode)
            {
                StartCoroutine(movementNode.CharacterMovement());
            }

            if (_node.GetType() == typeof(CloseDialogue))
            {
                dialoguePanel.SetActive(false);
                speakerPanel.SetActive(false);
            }

            if (_node.GetType() == typeof(CG))
            {
                CG scopedCGNode = _node as CG;
                showCG(_node as CG);

                if (scopedCGNode.waitForInput)
                {
                    currentCGNode = scopedCGNode;
                    dialoguePanel.gameObject.SetActive(false);
                    _charactersInScene.SetActive(false); 
                    CGWaitingForInput = true;
                    return;
                }
            }

            if(_node.GetType() == typeof(SetSpriteNode)){
                SetImage(_node as SetSpriteNode);
            }

            if (_node.GetType() == typeof(CGHide))
            {
                _cgGraphic.enabled = false;
            }

            if (_node.GetType() == typeof(WaitFor))
            {
                WaitFor _waitForNode = _node as WaitFor;
                StartCoroutine(WaitToAdvance(_waitForNode));
                //do not get next node automatically
                return;
            }
            if (_node.GetType() == typeof(Choices))
            {
                ChoicePanel.SetActive(true);

                nodeOptions = new List<Node>();
                int listIndex = 0;

                Choices _choiceNode = _node as Choices;
                NodePort _choicePort = _choiceNode.GetOutputPort("output");
                List<NodePort> _nodePorts = _choicePort.GetConnections();
                foreach (NodePort _NP in _nodePorts)
                {
                    nodeOptions.Add(_NP.node);
                    Button _newOption = Instantiate(choiceButton) as Button;
                    _newOption.transform.SetParent(ChoicePanel.transform);
                    listIndex++;//on this button it where on the list it is, using this
                    
                    //ChoiceOptionHolder _holder = _newOption.GetComponent<ChoiceOptionHolder>();
                    OptionNode optionNode = _NP.node as OptionNode;
                        _newOption.GetComponentInChildren<Text>().text = optionNode.optionText;
                    _newOption.GetComponent<ChoiceOptionHolder>().node = _NP.node;
                   // _holder.node = _NP.node;
                   // _holder.indexOfOptions = listIndex;
                }

                return; //do not load next node, since we want the player input to decide the branch to go down
            }


            //get next node(s)
            NodePort _port = _node.GetOutputPort("output");
            if (_port.IsConnected)
            {
                GetNodeFunction(_port.GetConnections());
            }
        }
    }

    
    public void ShowDialogue(DialogueNode dialogue)
    {
        dialoguePanel.SetActive(true);

        //set dialogue text
        //        dialogueTMP.text = dialogue.Dialogue;
        dialogueTMP.text = "";
        StartCoroutine(AnimateText(dialogue.Dialogue));

        if (dialogue.Speaker != "")
        {
            speakerPanel.SetActive(true);
            speakerTMP.text = currentDialogue.Speaker;
        }
        else
        {
            speakerPanel.SetActive(false);
        }

        CharacterSprite speakerCharacter;
        CharacterSprite dimmedCharacter;
        if (dialogue.whoIsSpeaking.IsLeft())
        {
            speakerCharacter = leftCharacter.GetComponent<CharacterSprite>();
            dimmedCharacter = rightCharacter.GetComponent<CharacterSprite>();
        }
        else
        {
            speakerCharacter = rightCharacter.GetComponent<CharacterSprite>();
            dimmedCharacter = leftCharacter.GetComponent<CharacterSprite>();
        }
        if (!dialogue.IsMoving)
        {
            speakerCharacter.IsSpeaking = true;
            dimmedCharacter.IsSpeaking = false;
            speakerCharacter.Outfit.color = new Color(1f, 1f, 1f);

        }
        
        if (dimmedCharacter.InScene)
        {
            dimmedCharacter.Outfit.color = new Color(0.7f, 0.7f, 0.7f);
            StartCoroutine(AnimateDim(true, speakerCharacter));
        }
        //StartCoroutine(AnimateDim(false, speakerCharacter));
        //StartCoroutine(AnimateDim(true, dimmedCharacter));
    }

    private IEnumerator AnimateDim(bool isDimmed, CharacterSprite cSprite)
    {
        float _lerpTime = 1f;
        float _curLerpTime = 0f;


        float _startDim = 0f;
        float _endDim = 0.7f;

        if (!isDimmed)
        {
            _endDim = 1f;
        }

        Color _outfitColor = new Color();

        do
        {
            _curLerpTime += Time.deltaTime;
            float t = _curLerpTime / _lerpTime;
            float _currentDim = Mathf.Lerp(_startDim, _endDim, t);
            
            _outfitColor.r = _currentDim;
            _outfitColor.g = _currentDim;
            _outfitColor.b = _currentDim;

            cSprite.Face.color = _outfitColor;

            yield return null;
        } while (_curLerpTime < _lerpTime);
    }

    private IEnumerator AnimateText(string dialogue)
    {
        Debug.Log("AnimateText");
        dialogueTMP.text = dialogue;
        dialogueTMP.maxVisibleCharacters = 0;
        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueTMP.maxVisibleCharacters++;
            yield return new WaitForSeconds(0.05f);
        }
    }


    public void showCG(CG cgNode)
    {
        _cgGraphic.enabled = true;
        
        StartCoroutine(AnimateCG(cgNode.CGGraphic));
    }
    public IEnumerator AnimateCG(Sprite newCG)
    {
        float _curLerpTime = 0f;
        float _lerpTime = 1f;
        Color _opacity = new Color() { a = 0 };
        do
        {
            _curLerpTime += Time.deltaTime;
            float t = _curLerpTime / _lerpTime;

            _opacity.a = Mathf.Lerp(1, 0, t);

            yield return null;
        } while (_curLerpTime < _lerpTime);

        _cgGraphic.sprite = newCG;
        _curLerpTime = 0f;
        _lerpTime = 1f;
        _opacity = new Color() { a = 0 };
        do
        {
            _curLerpTime += Time.deltaTime;
            float t = _curLerpTime / _lerpTime;

            _opacity.a = Mathf.Lerp(0, 1, t);

            yield return null;
        } while (_curLerpTime < _lerpTime);
    }
    IEnumerator WaitToAdvance(WaitFor _waitFor)
    {
        yield return new WaitForSeconds(_waitFor.TimeToPause);
        NodePort _port = _waitFor.GetOutputPort("output");
        if (_port.IsConnected)
        {
            GetNodeFunction(_port.GetConnections());

        }
    }
    public void EndCutscene()
    {
        //GameManager.gm.CanMove = true; 
        dialoguePanel.SetActive(false);
        //int indexCutscene = GameManager.gm.cutscenes.IndexOf(currentCutscene);
        inCutscene = false;

        foreach(Image _image in activeImages)
        {
            _image.enabled = false;
        }
        //GameManager.gm.Data.cutsceneBools[indexCutscene] = true;
    }

   /* void SetSpriteOnScreen(int whichImage, Actor_SO _actor, int faceInt, int outfitInt, bool active = true)
    {
        
        Image _characterImage = leftCharacter;
        switch (whichImage)
        {
            case 1:
                _characterImage = leftCharacter;
                break;
            case 2:
                _characterImage = midLeftCharacter;
                break;
            case 3:
                _characterImage = midRightCharacter;
                break;
            case 4:
                _characterImage = rightCharacter;
                break;
        }

        CharacterSprite _characterSprite = _characterImage.GetComponent<CharacterSprite>();
        _characterSprite.Face.enabled = true;
        _characterSprite.Face.sprite = _actor.Faces[faceInt];
        _characterSprite.Outfit.enabled = true;
        _characterSprite.Outfit.sprite = _actor.Outfits[outfitInt];

    }
    */
    public float MoveDistance = 100f;

    void SetImage(SetSpriteNode setSpriteNode){

        int whichImage = 1;
        Image characterImage = leftCharacter;
        switch (setSpriteNode.Spot)
        {

            case (MovementNode.SpotOnScreen.Left):
                whichImage = 1;
                break;
            case MovementNode.SpotOnScreen.MidLeft:
                whichImage = 2;
                break;
            case MovementNode.SpotOnScreen.MidRight:
                whichImage = 3;
                break;
            case MovementNode.SpotOnScreen.Right:
                whichImage = 4;
                break;
               
        }
        switch (whichImage)
        {
            case 1:
                characterImage = leftCharacter;
                break;
            case 2:
                characterImage = midLeftCharacter;
                break;
            case 3:
                characterImage = midRightCharacter;
                break;
            case 4:
                characterImage = rightCharacter;
                break;
        }
        CharacterSprite charSprite = characterImage.GetComponent<CharacterSprite>();

        charSprite.Face.enabled = true;
        charSprite.Outfit.enabled = true;


        charSprite.CurrentFace = setSpriteNode.Face;
        charSprite.CurrentOutfit = setSpriteNode.Outfit;

        if(setSpriteNode.Face)
            charSprite.Face.sprite = setSpriteNode.Face;

        if(setSpriteNode.Outfit)
            charSprite.Outfit.sprite = setSpriteNode.Outfit;

        //charSprite.Face.enabled = false;
        //charSprite.Outfit.enabled = false;
    }

    //This isn't used anywhere, and I'm not sure if this even worked.
    //Just keeping this here for the time being
    public static Cutscene CutsceneDeepCopy<T>(T other)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, other);
            ms.Position = 0;
            return (Cutscene)formatter.Deserialize(ms);
        }
    }
    
}

