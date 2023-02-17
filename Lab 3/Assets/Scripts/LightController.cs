using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private bool state;
    private Material mat;

    void Start()
    {
        state = false;
        mat = GetComponent<Renderer>().material;
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
