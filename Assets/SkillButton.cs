using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SkillButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Skills AssignedSkill;
    public List<GameObject> Targets;
    public GameObject user;
    public void OnClick()
    {
        AssignedSkill.Action(Targets, user);
        OnDeselect(null);
    }
    public void OnSelect(BaseEventData eventData)
    {
        if (Targets[0].GetComponent<ActorSlot>().IsAI)
        {
            foreach (GameObject go in Targets)
            {
                go.GetComponent<ActorSlot>().HP.GetComponentInParent<Outline>().enabled = true;
            }
        }
        else
        {
            foreach (GameObject go in Targets)
            {
                go.GetComponent<ActorSlot>().GetComponentInChildren<Outline>().enabled = true;
            }
        }
    }
    public void OnDeselect(BaseEventData eventData)
    {
        if (Targets[0].GetComponent<ActorSlot>().IsAI)
        {
            foreach (GameObject go in Targets)
            {
                go.GetComponent<ActorSlot>().HP.GetComponentInParent<Outline>().enabled = false;
            }
        }
        else
        {
            foreach (GameObject go in Targets)
            {
                go.GetComponent<ActorSlot>().GetComponentInChildren<Outline>().enabled = false;
            }
        }
    }
}
