using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]

public class ButtonController : MonoBehaviour
{
    [Header("Configuration")] 
    public int buttonIndex = -1; 
    public GameObject button;
    [Header("Events")]
    public UnityEvent onPress;
    public UnityEvent onRelease;
    public ButtonEvent buttonEvent;

    private HandController controller;
    private HapticController haptic;
    private Interactable item;
    
    private Material mat;
    private bool isPressed, isActivated;
    private string stringBuffer;


    void Start()
    {
        mat = button.GetComponent<Renderer>().material;
        isPressed = false;
        isActivated = false;
    }

    private void Update()
    {
        if (isActivated)
        {
            stringBuffer = "";  // store action results, used by buttonEvent.Invoke()

            // actions depend on button index
            switch (buttonIndex)
            {
                case 1:
                    stringBuffer += item.AlterTransparency();
                    stringBuffer += item.AlterColor();
                    break;
                case 2:
                    stringBuffer += item.AlterGravity();
                    item.GetComponent<RotateController>().isActivated = false;
                    break;
                default:
                    // Default: do most of actions
                    stringBuffer += item.AlterTransparency();
                    stringBuffer += item.AlterColor();
                    stringBuffer += item.AlterScale();
                    stringBuffer += item.AlterRotate();
                    //SwitchGravity();
                    break;
            }

            buttonEvent.Invoke(controller, stringBuffer.Trim());
        }
    }


    public void ButtonTrigger()
    {
        Color buttonColor;

        /* used to (de)activate the button instance's action */
        if (!isActivated)
        {
            buttonColor = Color.green;
            isActivated = true;
            haptic.Pulse(0.5f, 100, 100, controller);
        }
        else
        {
            buttonColor = Color.red;
            isActivated = false;
            haptic.Pulse(0.5f, 100, 100, controller);

            // reset state
            stringBuffer = "";

            switch (buttonIndex)
            {
                case 2:
                    item.GetComponent<Rigidbody>().useGravity = false;
                    stringBuffer += "Gravity: " + item.GetComponent<Rigidbody>().useGravity + "\n ";
                    break;
                case -1:
                    item.GetComponent<RotateController>().isActivated = false;
                    stringBuffer += "Rotate: " + item.GetComponent<RotateController>().isActivated + "\n ";
                    break;
                default:
                    break;
            }

            buttonEvent.Invoke(controller, stringBuffer.Trim());
        }

        mat.color = buttonColor;    // To reflect button's current state
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("BUTTON OnTriggerEnter");
        controller = other.GetComponent<HandController>();
        haptic = controller.GetComponent<HapticController>();
        item = controller.heldItem;
        
        if (controller.ControllerThumbstick.state && item && !isPressed) // prevent continous triggering invoke function
        {
            button.transform.localPosition = new Vector3(0, 0.015f, 0);
            isPressed = true; 
            onPress.Invoke();   // can directly call ButtonTrigger(), but use invoke() for better practice
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("BUTTON OnTriggerExit");
        button.transform.localPosition = new Vector3(0, 0.03f, 0);
        isPressed = false;
        onRelease.Invoke();
    }
}
[Serializable] public class ButtonEvent : UnityEvent<HandController, string> { }