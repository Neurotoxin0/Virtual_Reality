using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

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
    private Color itemColor;
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
                    LerpTransparency();
                    LerpColor();
                    break;
                case 2:
                    SwitchGravity();
                    item.GetComponent<RotateController>().isActivated = false;
                    break;
                default:
                    // Default: do most of actions
                    LerpTransparency();
                    LerpColor();
                    LerpScale();
                    AlterRotate();
                    //SwitchGravity();
                    break;
            }

            buttonEvent.Invoke(controller, stringBuffer.Trim());
        }
    }

    private void LerpTransparency()
    {
        float transparency = Mathf.Lerp(0, 1, Mathf.PingPong(Time.time, 1));
        itemColor = new Color(itemColor.r, itemColor.g, itemColor.b, transparency);
        item.GetComponent<Renderer>().material.color = itemColor;

        stringBuffer += "Transparency: " + transparency + "\n ";
    }
    
    private void LerpColor()
    {
        float transparency = itemColor.a;
        itemColor = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
        itemColor.a = transparency;
        item.GetComponent<Renderer>().material.color = itemColor;
        stringBuffer += "Color: " + itemColor + "\n ";
    }

    private void LerpScale()
    {
        Vector3 scale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.PingPong(Time.time, 1));
        item.transform.localScale = scale;

        stringBuffer += "Scale: " + scale + "\n "; 
    }

    private void AlterRotate()
    {
        RotateController rotateController = item.GetComponent<RotateController>();
        rotateController.isActivated = true;
        float rotateRatio = Mathf.Lerp(0, 2, Mathf.PingPong(Time.time, 1));
        rotateController.rotateRatio = rotateRatio;

        stringBuffer += "Rotate: " + rotateController.isActivated + "\n " +
                        "Rotate Ratio: " + rotateRatio + "\n ";
    }

    private void SwitchGravity()
    {
        item.GetComponent<Rigidbody>().useGravity = true;
        item.GetComponent<RotateController>().isActivated = false;

        stringBuffer += "Gravity: " + item.GetComponent<Rigidbody>().useGravity + "\n " +
                        "Rotate: " + item.GetComponent<RotateController>().isActivated + "\n ";
    }

    public void ButtonTrigger()
    {
        Color buttonColor;

        /* used to (de)activate the button instance's action */
        if (!isActivated)
        {
            itemColor = item.GetComponent<Renderer>().material.color;

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
                default:
                    item.GetComponent<RotateController>().isActivated = false;
                    stringBuffer += "Gravity: " + item.GetComponent<Rigidbody>().useGravity + "\n " +
                                    "Rotate: " + item.GetComponent<RotateController>().isActivated + "\n "; 
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