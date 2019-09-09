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
    
    public static CutsceneManager cutsceneManager;
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
    [SerializeField] private TextMeshProUGUI dialogueTMP;
    [SerializeField] private TextMeshProUGUI speakerTMP;

    [SerializeField] private List<Image> activeImages;

    //points used for reference when moving characters
    [Header("")]
    [SerializeField] private Transform LeftSpot;
    [SerializeField] private Transform MidLeftSpot;
    [SerializeField] private Transform MidRightSpot;
    [SerializeField] private Transform RightSpot;

    //people in cutscene
    [Header("")]
    [SerializeField] private GameObject _charactersInScene;
    [SerializeField] private Image leftCharacter;
    [SerializeField] private Image midLeftCharacter;
    [SerializeField] private Image rightCharacter;
    [SerializeField] private Image midRightCharacter;


    
    
    // Use this for initialization
	void Awake () {
        cutsceneManager = this;
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
        Debug.Log(leftCharacter.GetComponent<CharacterSprite>().Outfit.color);

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



            if (_node.GetType() == typeof(MovementNode))
            {//manage character movement in the scene

                StartCoroutine(CharacterMovement(_node as MovementNode));
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

            if(_node.GetType() == typeof(EndSceneNode))
            {
                EndCutscene();
                return;
            }
            if(_node.GetType() == typeof(SetSpriteNode)){
                SetImage(_node as SetSpriteNode);
                
            }

            if (_node.GetType() == typeof(CGHide))
            {
                _cgGraphic.enabled = false;
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
        dialogueTMP.text = dialogue.Dialogue;
        
        if (dialogue.Speaker != null)
        {
            //speaker will be assumed to be previous speaker
            speakerTMP.text = currentDialogue.Speaker;
        }
        else
        {
            speakerTMP.text = "";
        }



        CharacterSprite speakerCharacter;
        CharacterSprite dimmedCharacter;
        
        if (dialogue.whoIsSpeaking.IsLeft())
        {
            speakerCharacter = leftCharacter.GetComponent<CharacterSprite>();
            dimmedCharacter = rightCharacter.GetComponent<CharacterSprite>();

            speakerCharacter.IsSpeaking = true;
            //speakerCharacter.Outfit.color = new Color()
//            speakerCharacter.Outfit.color = new Color32(160, 160, 160, 100);
            /*
            Color NotDimmed = speakerCharacter.Outfit.color;
            NotDimmed.r = 1;
            NotDimmed.g = 1;
            NotDimmed.b = 1;
            speakerCharacter.Outfit.color = NotDimmed;

            Color Dimmed = dimmedCharacter.Outfit.color;
            Dimmed.r = 0.5f;
            Dimmed.g = 0.5f;
            Dimmed.b = 0.5f;
            dimmedCharacter.Outfit.color = Dimmed;
            Debug.Log(dimmedCharacter.Outfit.color);
        */
        }
        else
        {
            
        }

        
    }

    public void showCG(CG cgNode)
    {
        _cgGraphic.enabled = true;
        if (cgNode.CGGraphic)
        {
            _cgGraphic.sprite = cgNode.CGGraphic;
        }
        else
        {
            Debug.Log("You need to assign a sprite first or it'll just be the same CG as last time this was used");
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
    private float _distance = 100f;

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


    IEnumerator CharacterMovement(MovementNode moveNode)
    {
        MovementNode.SpotOnScreen scopedSpotOnScreen = moveNode.spotOnScreen;
        MovementNode.EnterOrLeave movementType = moveNode.enterOrLeave;


        //manage face/outfit
        int _whichImage = 1;
        _whichImage = scopedSpotOnScreen.spotNumber();
        //SetSpriteOnScreen(_whichImage, scopedActor, scopedActor.baseClass.FaceID, scopedActor.baseClass.OutfitID);


        //setting up variables
        Image _image = rightCharacter;
        CharacterSprite charSprite = _image.GetComponent<CharacterSprite>();
        switch (_whichImage)
        {
            case 1:
                _image = leftCharacter;
                break;
            case 2:
                _image = midLeftCharacter;
                break;
            case 3:
                _image = midRightCharacter;
                break;
            case 4:
                _image = rightCharacter;
                break;
        }
        charSprite = _image.GetComponent<CharacterSprite>();

        float colorDim = 1f;
        if (!moveNode.IsSpeaking)
        {
            colorDim = 0.7f;
        }


        Vector3 _beginPoint = new Vector3(RightSpot.transform.position.x + _distance, _image.transform.position.y, _image.transform.position.z);
        Vector3 _endPoint = new Vector3(RightSpot.position.x, _beginPoint.y, _beginPoint.z);

        float _lerpTime = 1f;
        float _curLepTime = 0f;

        Image _face = charSprite.Face;
        Image _outfit = charSprite.Outfit;

        Color _faceColor = _face.color;
        Color _outfitColor = _outfit.color;

        float _startOpacity = 0;
        float _endOpacity = 1;


        
        if(!scopedSpotOnScreen.IsRight())
        {
            _beginPoint = new Vector3(LeftSpot.transform.position.x - _distance, _image.transform.position.y, _image.transform.position.z);
            _endPoint = new Vector3(LeftSpot.position.x, _beginPoint.y, _beginPoint.z);
            // _endPoint = _beginPoint + Vector3.right * distance;
            if (scopedSpotOnScreen.IsMiddle())
            {
                _beginPoint = new Vector3(midLeftCharacter.transform.position.x - _distance, _image.transform.position.y, _image.transform.position.z);
                _endPoint = new Vector3(MidLeftSpot.position.x, _beginPoint.y, _beginPoint.z);
            }
        }
        else if(scopedSpotOnScreen.IsMiddle())
        {
            _beginPoint = new Vector3(MidRightSpot.transform.position.x + _distance, _image.transform.position.y, _image.transform.position.z);
            _endPoint = new Vector3(MidRightSpot.position.x, _beginPoint.y, _beginPoint.z);
        }

        
        if (movementType.IsLeaving())
        {
            Vector3 _newStart = new Vector3(_endPoint.x, _endPoint.y, _endPoint.z);
            Vector3 _newEnd = new Vector3(_beginPoint.x, _endPoint.y, _endPoint.z);
            _beginPoint = _newStart;
            _endPoint = _newEnd;
            _startOpacity = 1f;
            _endOpacity = 0f;
            activeImages.Remove(_outfit);
            activeImages.Remove(_face);
        }
        else
        {
            activeImages.Add(_outfit);
            activeImages.Add(_face);
        }

        if (!moveNode.enterOrLeave.isMoving())
        {
            do
            {

                //here is where we put in return for if player hits skip

                _curLepTime += Time.deltaTime;
                float t = _curLepTime / _lerpTime;
                
                //distance moved
                _image.transform.position = Vector3.Lerp(_beginPoint, _endPoint, t);

                //calculate opacity
                float _currentAlpha = Mathf.Lerp(_startOpacity, _endOpacity, t);
                _faceColor.a = _currentAlpha;
                _outfitColor.a = _currentAlpha;
                _outfitColor.r = colorDim;
                _outfitColor.g = colorDim;
                _outfitColor.b = colorDim;

                _face.color = _faceColor;
                _outfit.color = _outfitColor;

               

                yield return 0;
            } while (_curLepTime < _lerpTime);
        }
        else
        {//when just moving to a different spot
            _image.transform.position = _endPoint;
        }
        
        
        yield return 0;
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

