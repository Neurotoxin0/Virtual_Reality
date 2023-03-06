using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverController : MonoBehaviour
{
    [Header("Configuration")] 
    public int leverIndex = -1;
    public bool isSpring = false;
    //[Range(1, 5)] public float debug = 1;
    [Header("Events")] 
    public UnityEvent onPress;
    public UnityEvent onRelease;
    public LeverEvent leverEvent;

    private Interactable item;
    private HandController controller;
    private HapticController haptic;
    private bool isPressed, isActivated;
    private float boundary, value;
    private string stringBuffer;

    void Start()
    {
        isActivated = false;
        boundary = 0.5f;    // depends on the length of base
        value = transform.localPosition.x;  // default: 0
    }

    void Update()
    {
        // check value change
        value = transform.localPosition.x; // range: -0.5 <-> 0.5

        if (isActivated) LeverAction();
        else
        {
            if (isSpring && value != 0)  // if is spring lever and released
            {
                // lerp issue: infinitely close to 0 resulting infinite invoke -> hard reset when reach a point
                if (Mathf.Abs(value) <= 0.01)       value = 0;
                else                                value = Mathf.Lerp(value, 0, 0.75f * Time.deltaTime);

                transform.localPosition = new Vector3(value, transform.localPosition.y, transform.localPosition.z);
                LeverAction();
            }
        }
    }

    private void LeverAction()
    {
        float convertedValue = value + 0.5f;  // change value range from -0.5 <-> 0.5 to: 0 <-> 1
        //Debug.Log("convertedValue: " + convertedValue);

        stringBuffer = "";  // store action results, used by leverEvent.Invoke()

        switch (leverIndex)
        {
            case 1:
                stringBuffer += item.AlterColor(convertedValue);
                break;
            case 2:
                stringBuffer += item.AlterRotate(convertedValue);
                break;
            default:
                // Default: do all actions !!!
                stringBuffer += item.AlterTransparency(convertedValue);
                stringBuffer += item.AlterColor(convertedValue);
                stringBuffer += item.AlterScale(convertedValue);
                stringBuffer += item.AlterRotate(convertedValue);
                break;
        }

        leverEvent.Invoke(controller, stringBuffer.Trim());
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("LEVER OnTriggerEnter");
        controller = other.GetComponent<HandController>();
        haptic = controller.GetComponent<HapticController>();
        item = controller.heldItem;

        if (item & !isPressed)
        {
            isPressed = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("LEVER OnTriggerStay: " + other.name);

        if (controller.ControllerThumbstick.state && isPressed)
        {
            isActivated = true;

            float offset = transform.InverseTransformPoint(other.transform.position).x;     // position offset between controller & lever
            float targetX = Mathf.Lerp(transform.localPosition.x, transform.localPosition.x+offset, 0.75f * Time.deltaTime);
            targetX = Mathf.Clamp(targetX, -boundary, boundary);    // boundaries limitation
            if (Mathf.Abs(targetX) == boundary) haptic.Pulse(0.5f, 100, 100, controller);

            Vector3 targetPosition = new Vector3(targetX, transform.localPosition.y, transform.localPosition.z);    // only x-axis adjustment
            //Debug.Log("offset: " + offset + " current pos: " + transform.localPosition + " targetPosition: " + targetPosition);
            transform.localPosition = targetPosition;
            onPress.Invoke();
        }
        else isActivated = false;
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("LEVER OnTriggerExit");\
        isActivated = false;
        isPressed = false; 
        onRelease.Invoke();
    }
}
[Serializable] public class LeverEvent : UnityEvent<HandController, string> { }