using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<RectTransform>().localScale = new Vector3(0,0,0);
        LeanTween.scale(gameObject, Vector3.one, 0.2f);
        LeanTween.scale(gameObject, new Vector3(0,0,0), 0.2f).setDelay(2);
        StartCoroutine(SelfDestruct());
    }


    public IEnumerator SelfDestruct(){
        yield return new WaitForSeconds(2.2f);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
