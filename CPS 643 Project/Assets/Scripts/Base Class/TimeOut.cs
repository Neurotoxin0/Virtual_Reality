using System.Collections;
using UnityEngine;

// Base class for timeout function

public class TimeOut: MonoBehaviour
{
    private protected bool timeoutEnabled = false, timeout = false;
    private protected WaitForSeconds duration;

    protected virtual void SetTimeOut(float time)
    {
        duration = new WaitForSeconds(time);
        StartCoroutine(CountdownEvent());
    }

    protected virtual void ResetTimeOut() 
    { 
        Debug.Log("Reset Timeout");
        timeoutEnabled = false; 
        timeout = false; 
    }


    protected virtual IEnumerator CountdownEvent()
    {
        Debug.Log("Timeout ON");
        timeoutEnabled = true;
        yield return duration;
        timeout = true;
        Debug.Log("Timeout OFF");
    }
}
