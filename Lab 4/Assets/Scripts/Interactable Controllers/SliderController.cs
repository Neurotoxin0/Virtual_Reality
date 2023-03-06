using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]

public class SliderController : MonoBehaviour
{
    [Header("Configuration")]
    public int buttonIndex = -1;
    [Header("Events")]
    public UnityEvent onPress;
    public UnityEvent onRelease;
    public SliderEvent sliderEvent;

    private HandController controller;
    private HapticController haptic;
    private Interactable item;
    private Material mat;
    private bool isPressed, isActivated;
    private string stringBuffer;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        isPressed = false;
        isActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            stringBuffer = "";  // store action results, used by buttonEvent.Invoke()

            // actions depend on button index
            switch (buttonIndex)
            {
                default:
                    // Default: do most of actions
                    stringBuffer += item.AlterTransparency();
                    stringBuffer += item.AlterColor();
                    stringBuffer += item.AlterScale();
                    stringBuffer += item.AlterRotate();
                    //SwitchGravity();
                    break;
            }

            sliderEvent.Invoke(controller, stringBuffer.Trim());
        }
    }


    public void SliderTrigger()
    {
        Color buttonColor;

        /* used to (de)activate the button instance's action */
        if (!isActivated)
        {
            buttonColor = Color.green;
            transform.localPosition = new Vector3(0.025f, 0.03f, 0);
            isActivated = true;
            haptic.Pulse(0.5f, 100, 100, controller);
        }
        else
        {
            buttonColor = Color.red;
            transform.localPosition = new Vector3(-0.025f, 0.03f, 0);
            isActivated = false;
            haptic.Pulse(0.5f, 100, 100, controller);

            // reset state
            stringBuffer = "";

            switch (buttonIndex)
            {
                case -1:
                    item.GetComponent<RotateController>().isActivated = false;
                    stringBuffer += "Rotate: " + item.GetComponent<RotateController>().isActivated + "\n ";
                    break;
                default:
                    break;
            }

            sliderEvent.Invoke(controller, stringBuffer.Trim());
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
            isPressed = true;
            onPress.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("BUTTON OnTriggerExit");
        isPressed = false;
        onRelease.Invoke();
    }
}
[Serializable] public class SliderEvent : UnityEvent<HandController, string> { }