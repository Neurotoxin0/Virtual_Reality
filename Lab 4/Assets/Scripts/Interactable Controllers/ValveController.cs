using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
            if (isSpring && Mathf.Abs(value) > 0.05)  // if is spring lever and released
            {
                // lerp issue: infinitely close to 0 resulting infinite invoke -> hard reset when reach a point
                value = Mathf.Lerp(value, 0, 0.1f * Time.deltaTime);
                transform.Rotate(new Vector3(0, 0, 1), -value);
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
                stringBuffer += item.AlterScale(convertedValue);
                break;
            default:
                // Default: do all actions !!!
                stringBuffer += item.AlterTransparency(convertedValue);
                stringBuffer += item.AlterColor(convertedValue);
                stringBuffer += item.AlterScale(convertedValue);
                stringBuffer += item.AlterRotate(convertedValue);
                break;
        }

        valveEvent.Invoke(controller, stringBuffer.Trim());
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("VALVE OnTriggerEnter");
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