using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//[RequireComponent(typeof(Collider))]

// under Canvas.Control Panel.Panel

public class SliderController : MonoBehaviour
{
    [Header("Configuration")]
    public bool invokeSetDiffuculty = false;

    [Header("Events")]
    public FloatEvent setDiffuculty;
    
    private int type;   // 1: button; 2: toggle

    private void Start()
    {
        // get type
        if (GetComponent<Slider>() != null) type = 1;
        else if (GetComponent<Toggle>() != null) type = 2;
        else Debug.LogError("SliderController: no Slider component found!");
    }
    public void OnSliderValueChanged(float value)
    {
        Debug.Log("Slider value changed to " + value);

        if (invokeSetDiffuculty) setDiffuculty.Invoke(value);
    }
}
[Serializable] public class FloatEvent : UnityEvent<float> { }