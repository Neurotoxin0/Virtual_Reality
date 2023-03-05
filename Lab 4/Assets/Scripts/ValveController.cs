using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]

public class ValveController : MonoBehaviour
{
    [Header("Configuration")]
    public int valveIndex = -1;
    public bool isSpring = false;
    [Range(1, 5)] public float debug = 1;
    [Header("Events")]
    public UnityEvent onPress;
    public UnityEvent onRelease;
    public ValveEvent valveEvent;

    private Interactable item;
    private Color itemColor;
    private HandController controller;
    private HapticController haptic;
    private bool isPressed, isActivated;
    private float boundary, value;
    private string stringBuffer;

    void Start()
    {
        isActivated = false;
        boundary = 0.5f;    // depends on the length of base
        value = transform.localRotation.z; // default: 0
    }

    void Update()
    {
        // check value change
        value = transform.localRotation.z; // range: 1 <-> 0 <-> -1 (REM reverse range before use!!!)

        if (isActivated) ValveAction();
        else
        {
            if (isSpring && value != 0)  // if is spring lever and released
            {
                // lerp issue: infinitely close to 0 resulting infinite invoke -> hard reset when reach a point
                if (Mathf.Abs(value) <= 0.01) value = 0;
                else value = Mathf.Lerp(value, 0, 0.75f * Time.deltaTime);

                transform.localEulerAngles = new Vector3(0, 0, -(value) * 360);
                //transform.localPosition = new Vector3(value, transform.localPosition.y, transform.localPosition.z);
                ValveAction();
            }
        }
    }

    private void ValveAction()
    {
        float convertedValue = (-value+1)/2;  // change value range from 1 <-> 0 <-> -1 to: 0 <-> 0.5 <-> 1
        //Debug.Log("convertedValue: " + convertedValue);

        stringBuffer = "";  // store action results, used by leverEvent.Invoke()

        switch (valveIndex)
        {
            case 1:
                ChangeScale(convertedValue);
                break;
            case 2:
                ChangeRotateSpeed(convertedValue);
                break;
            default:
                // Default: do all actions !!!
                ChangeTransparency(convertedValue);
                ChangeColor(convertedValue);
                ChangeScale(convertedValue);
                ChangeRotateSpeed(convertedValue);
                break;
        }

        valveEvent.Invoke(controller, stringBuffer.Trim());
    }
    private void ChangeTransparency(float value)
    {
        itemColor = new Color(itemColor.r, itemColor.g, itemColor.b, value);
        item.GetComponent<Renderer>().material.color = itemColor;

        stringBuffer += "Transparency: " + value + "\n ";
    }

    private void ChangeColor(float value)
    {
        float rgb = 255 * value / 100;
        itemColor = new Color(rgb, rgb, rgb, itemColor.a);
        item.GetComponent<Renderer>().material.color = itemColor;

        stringBuffer += "Color: " + itemColor + "\n ";
    }

    private void ChangeScale(float value)
    {
        float scaleRatio = value / 2;
        item.transform.localScale = Vector3.one * scaleRatio;

        stringBuffer += "Scale: " + scaleRatio + "\n ";
    }

    private void ChangeRotateSpeed(float value)
    {
        RotateController rotateController = item.GetComponent<RotateController>();
        rotateController.isActivated = true;
        float rotateRatio = value * 2;
        rotateController.rotateRatio = rotateRatio;

        stringBuffer += "Rotate: " + rotateController.isActivated + "\n " +
                        "Rotate Ratio: " + rotateRatio + "\n ";
    }

    private float offset;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("VALVE OnTriggerEnter");
        controller = other.GetComponent<HandController>();
        haptic = controller.GetComponent<HapticController>();
        item = controller.heldItem;

        if (item & !isPressed)
        {
            itemColor = item.GetComponent<Renderer>().material.color;
            
            isPressed = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("VALVE OnTriggerStay: " + other.name);

        if (controller.ControllerThumbstick.state && isPressed)
        {
            isActivated = true;

            Quaternion offset = Quaternion.Inverse(other.transform.rotation);   // angle offset between controller & lever
            float targetZ = offset.z;
            targetZ = Mathf.Clamp(targetZ, -boundary, boundary);    // boundaries limitation
            if (Mathf.Abs(targetZ) == boundary) haptic.Pulse(0.5f, 100, 100, controller);

            transform.localEulerAngles = new Vector3(0, 0, -targetZ *360);  // to adjust range to -1 <-> 1

            onPress.Invoke();
        }
        else isActivated = false;
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("VALVE OnTriggerExit");\
        isActivated = false;
        isPressed = false;
        onRelease.Invoke();
    }
}
[Serializable] public class ValveEvent : UnityEvent<HandController, string> { }