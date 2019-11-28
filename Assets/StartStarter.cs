using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStarter : MonoBehaviour
{
    bool can = false;
    int i = 0;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!can)
        {
            StartCoroutine(waitingHere());
        }
        else if (can && (i < 26))
        { 
            transform.GetChild(i).GetComponent<Animator>().SetTrigger("Play");
            can = false;
            i++;
        }

        if (i == 26)
        {
            enabled = false;
        }

            
        
    }

    IEnumerator waitingHere()
    {
        can = false;
        yield return new WaitForSecondsRealtime(0.4f);
        can = true;
    }
}
