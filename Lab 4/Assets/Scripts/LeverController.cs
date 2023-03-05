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
    [Range(1, 5)] public float debug = 1;
    [Header("Events")] 
    public UnityEvent onPress;
    public UnityEvent onRelease;
    public UnityEvent onValueChanged;

    private Interactable item;
    private Color itemColor;
    private HandController controller;
    private bool isPressed, isActivated;
    private float boundary, currentValue, rerordValue, originalYCoods;

    void Start()
    {
        isActivated = false;
        boundary = 0.5f;    // depends on the length of base
        currentValue = 0;
        rerordValue = currentValue;
    }

    void Update()
    {
        // check value change
        currentValue = transform.localPosition.x; // range: -0.5 <-> 0.5

        if (currentValue != rerordValue)
        {
            rerordValue = currentValue;
            onValueChanged.Invoke();
        }

        if (isActivated) LeverAction();
        else
        {
            if (isSpring && currentValue != 0)  // if is spring lever and released
            {
                // lerp issue: infinitely close to 0 -> faster lerp + hard reset when reach a point
                if (Mathf.Abs(currentValue) <= 0.01)        currentValue = Mathf.Lerp(currentValue, 0, Time.deltaTime);
                else if (Mathf.Abs(currentValue) <= 0.005)  currentValue = 0;   
                else                                        currentValue = Mathf.Lerp(currentValue, 0, 0.75f * Time.deltaTime);

                transform.localPosition = new Vector3(currentValue, transform.localPosition.y, transform.localPosition.z);
                LeverAction();
            }
        }
    }

    private void LeverAction()
    {
        float value = currentValue + 0.5f;  // change value range from -0.5 <-> 0.5 to: 0 <-> 1
        //Debug.Log("currentValue: " + currentValue + " value: " + value);

        switch (leverIndex)
        {
            case 1:
                ChangeScale(value);
                break;
            case 2:
                ChangeRotateSpeed(value);
                break;
            default:
                // Default: do all actions !!!
                ChangeColor(value);
                ChangeTransparency(value);
                ChangeScale(value);
                ChangeRotateSpeed(value);
                break;
        }
    }

    private void ChangeColor(float value)
    {
        float rgb = 255 * value / 100;
        itemColor = new Color(rgb, rgb, rgb, itemColor.a);
        item.GetComponent<Renderer>().material.color = itemColor;
    }

    private void ChangeTransparency(float value)
    {
        itemColor = new Color(itemColor.r, itemColor.g, itemColor.b, value);
        item.GetComponent<Renderer>().material.color = itemColor;
    }

    private void ChangeScale(float value)
    {
        float scaleRatio = value / 2;
        item.transform.localScale = Vector3.one * scaleRatio;
    }

    private void ChangeRotateSpeed(float value)
    {
        RotateController rotateController = item.GetComponent<RotateController>();
        rotateController.rotateRatio = value * 2;
        rotateController.isActivated = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("LEVER OnTriggerEnter");
        controller = other.GetComponent<HandController>();
        item = controller.heldItem;

        if (item & !isPressed)
        {
            itemColor = item.GetComponent<Renderer>().material.color;
            originalYCoods = item.transform.position.y;
            isPressed = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("LEVER OnTriggerStay: " + other.name);

        if (controller.ControllerThumbstick.state && isPressed)
        {
            isActivated = true;
                
            float offset = transform.InverseTransformPoint(other.transform.position).x;     // offset between controller & lever
            float targetX = Mathf.Lerp(transform.localPosition.x, transform.localPosition.x+offset, 0.75f * Time.deltaTime);
            targetX = Mathf.Clamp(targetX, -boundary, boundary);    // boundaries limitation

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
[Serializable] public class FloatEvent : UnityEvent<float> { }