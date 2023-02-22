using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private Material mat; 
    private bool state;

    void Start()
    {
        mat = GetComponent<Renderer>().material; 
        state = false;
    }

    void Switch_Power_State()
    {
        state = (state) ? false : true;

        if (state)
        {
            //Debug.Log("State ON");
            mat.EnableKeyword("_EMISSION");
        }
        else
        {
            //Debug.Log("State OFF"); 
            mat.DisableKeyword("_EMISSION");
        }
    }
}
