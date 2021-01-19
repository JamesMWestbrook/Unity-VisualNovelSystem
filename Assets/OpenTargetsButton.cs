using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class OpenTargetsButton : MonoBehaviour, ISelectHandler
{
    public TextMeshProUGUI SkillName;
    public TextMeshProUGUI SkillCost;
    public Skills AssignedSkill;
    
    public void OnSelect(BaseEventData eventData)
    {
        GameManager.Instance.BattleManager.SkillDescText.text = AssignedSkill.Description;
    }
}
