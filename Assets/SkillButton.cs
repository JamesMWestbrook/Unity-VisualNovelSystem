using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    public Skills AssignedSkill;
    public List<GameObject> Targets;
    public GameObject user;
    public void OnClick(){
        AssignedSkill.Action(Targets, user);
    }
}
