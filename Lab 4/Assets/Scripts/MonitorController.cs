using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonitorController : MonoBehaviour
{
    private GameObject cameraRig, leftController, rightController;
    private Interactable itemLeft, itemRight;
    private TextMeshProUGUI headerLeft, headerRight, textLeft, textRight;

    void Start()
    {
        headerLeft = transform.Find("HeaderLeft").gameObject.GetComponent<TextMeshProUGUI>();
        headerRight = transform.Find("HeaderRight").gameObject.GetComponent<TextMeshProUGUI>();
        textLeft = transform.Find("TextLeft").gameObject.GetComponent<TextMeshProUGUI>();
        textRight = transform.Find("TextRight").gameObject.GetComponent<TextMeshProUGUI>();

        cameraRig = GameObject.Find("[CameraRig]");
        leftController  = cameraRig.transform.Find("Controller (left)").gameObject;
        rightController = cameraRig.transform.Find("Controller (right)").gameObject;
    }

    
    private void UpdateItem()
    {
        itemLeft = leftController.GetComponent<HandController>().heldItem;
        itemRight = rightController.GetComponent<HandController>().heldItem;
        headerLeft.text = "Left: " + ((itemLeft) ? itemLeft.name : "Empty");
        headerRight.text = "Right: " + ((itemRight) ? itemRight.name : "Empty");
    }


    public void OnItemChange() 
    {
        //Debug.Log("MONITOR OnItemChange");
        UpdateItem(); 
    }

    public void OnValueChange(HandController controller, string str) 
    { 
        //Debug.Log("MONITOR OnValueChange: " + controller.name + " / " + str);

        if (controller.name == leftController.name) textLeft.text = str;
        else                                        textRight.text = str;
    }

}