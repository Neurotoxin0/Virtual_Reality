using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// under Canvas.Control Panel.Panel

public class ButtonController : MonoBehaviour
{
    [Header("Configuration")]
    public bool invokeResetScene = false;
    public bool invokeCheatMode = false;

    [Header("Events")]
    public BoolEvent resetScene; 
    public BoolEvent cheatMode;

    private int type;   // 1: button; 2: toggle
    private bool buttonState;

    void Start()
    {
        // get type
        if (GetComponent<Button>() != null) type = 1;
        else if (GetComponent<Toggle>() != null) type = 2;
        else Debug.LogError("ButtonController: no Button or Toggle component found!");

        buttonState = false;
    }

    public void OnClick()
    {
        Debug.Log("Button clicked!");

        if (type == 1) buttonState = !buttonState;
        else if (type == 2) buttonState = GetComponent<Toggle>().isOn;

        if (invokeResetScene)   resetScene.Invoke(buttonState);
        if (invokeCheatMode)    cheatMode.Invoke(buttonState);
    }
}
[Serializable] public class BoolEvent : UnityEvent<bool> { }