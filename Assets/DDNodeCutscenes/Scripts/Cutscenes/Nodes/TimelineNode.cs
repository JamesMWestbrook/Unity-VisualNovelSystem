using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CreateNodeMenu("VN/Timeline")]
public class TimelineNode : Node
{
    [SerializeField]
    public TimelineAsset playable;


    // Use this for initialization
    protected override void Init()
    {
        base.Init();

    }
    [Input(ShowBackingValue.Never)] public float input;
    [Output] public float output;

    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port)
    {


        return null; // Replace this
    }


    private PlayableDirector director;
    public void PlayTimeLine()
    {
        Debug.Log("Plays");
        director = CutsceneManager.Instance.GetComponent<PlayableDirector>();
        director.Play(playable,DirectorWrapMode.Hold);
    }

}
