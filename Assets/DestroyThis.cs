using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyThis : MonoBehaviour
{
    public float Timer;

    // Start is called before the first frame update
    void Start()
    {
            StartCoroutine(DestroyGO());
    }
    public IEnumerator DestroyGO()
    {
        yield return new WaitForSeconds(Timer);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
