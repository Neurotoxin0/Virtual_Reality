using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//[RequireComponent(typeof(Collider))]

// under Canvas.Control Panel.Panel

public class SliderController : MonoBehaviour
{
    [Header("Usage")]
    public bool invokeSetDiffuculty = false;

    [Header("Events")]
    public IntEvent setDiffuculty;
    
    private int type;   // 1: button; 2: toggle; reserved

    private void Start()
    {
        // get type
        if (GetComponent<Slider>() != null) type = 1;
        else if (GetComponent<Toggle>() != null) type = 2;
        else Debug.LogError("SliderController: no Slider component found!");
    }
    public void OnSliderValueChanged(float value)
    {
        Debug.Log("Slider value changed to " + (int)value);

        if (invokeSetDiffuculty) setDiffuculty.Invoke(SceneManager.GetActiveScene().buildIndex, (int)value);
    }
}
[Serializable] public class IntEvent : UnityEvent<int, int> { }