using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SkillDrop : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
