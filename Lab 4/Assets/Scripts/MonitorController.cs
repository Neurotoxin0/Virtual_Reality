using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonitorController : MonoBehaviour
{
    private GameObject cameraRig, leftController, rightController;
    private Interactable itemLeft, itemRight;
    private TextMeshProUGUI statusLeft, statusRight;

    void Start()
    {
        statusLeft = transform.Find("TextLeft").gameObject.GetComponent<TextMeshProUGUI>();
        statusRight = transform.Find("TextRight").gameObject.GetComponent<TextMeshProUGUI>();
        cameraRig = GameObject.Find("[CameraRig]");
        leftController  = cameraRig.transform.Find("Controller (left)").gameObject;
        rightController = cameraRig.transform.Find("Controller (right)").gameObject;
    }

    void Update()
    {
        UpdateItem(); 
        UpdateCanvas();
    }

    
    private void UpdateItem()
    {
        itemLeft = leftController.GetComponent<HandController>().heldItem;
        itemRight = rightController.GetComponent<HandController>().heldItem;
    }
    
    private void UpdateCanvas()
    {
        statusLeft.text  = "Left: \n"  + Status(itemLeft);
        statusRight.text = "Right: \n" + Status(itemRight);
    }

    private string Status(Interactable item)
    {
        if (item == null) return "Empty";

        string color = item.GetComponent<Renderer>().material.color.ToString();
        return color;
    }
}
