using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    public int buttonIndex = -1;

    private GameObject cameraRig, leftController, rightController;
    private Interactable itemLeft, itemRight;
    private Material mat;
    private bool isPressed, activated;
    Color buttonColor, itemColor;


    void Start()
    {
        mat = button.GetComponent<Renderer>().material;
        cameraRig = GameObject.Find("[CameraRig]");
        leftController = cameraRig.transform.Find("Controller (left)").gameObject;
        rightController = cameraRig.transform.Find("Controller (right)").gameObject; 
        isPressed = false;
        activated = false;
    }


    private void UpdateItem()
    {
        itemLeft = leftController.GetComponent<HandController>().heldItem;
        itemRight = rightController.GetComponent<HandController>().heldItem;
    }


    public void Button1Triggered()
    {
        UpdateItem();

        if (!activated)
        {
            activated = true;
            buttonColor = Color.green;
            itemColor = Color.blue;
        }
        else
        {
            activated = false; 
            buttonColor = Color.red;
            itemColor = Color.white;
        }

        mat.color = buttonColor;
        if (itemLeft) itemLeft.GetComponent<Renderer>().material.color = itemColor;
        if (itemRight) itemRight.GetComponent<Renderer>().material.color = itemColor;


    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("BUTTON OnTriggerEnter");
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(0, 0.015f, 0);
            onPress.Invoke();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("BUTTON OnTriggerExit");
        //Debug.Log("BUTTON Presser: " + presser);
        //Debug.Log("BUTTON Other: " + other);

        button.transform.localPosition = new Vector3(0, 0.03f, 0);
        onRelease.Invoke();
        isPressed = false;
    }
}
