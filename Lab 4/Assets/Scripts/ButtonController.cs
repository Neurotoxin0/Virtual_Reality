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

    private Interactable item;
    private Color itemColor;
    private Material mat;
    private bool isPressed, isActivated;
    private bool isRotating, isGravity;
 
    void Start()
    {
        mat = button.GetComponent<Renderer>().material;
        isPressed = false;
        isActivated = false;
        isRotating = false;
        isGravity = false;
    }

    private void Update()
    {
        if (isActivated)
        {
            // actions depend on button index
            switch (buttonIndex)
            {
                case 1:
                    LerpColor();
                    LerpTransparency();
                    break;
                case 2:
                    SwitchGravity();
                    item.GetComponent<RotateController>().isActivated = false;
                    break;
                default:
                    // Default: do most of actions
                    LerpColor();
                    LerpTransparency();
                    LerpScale();
                    AlterRotate();
                    //SwitchGravity();
                    break;
            }
        }
    }

    private void LerpColor()
    {
        itemColor = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
        item.GetComponent<Renderer>().material.color = itemColor;
    }

    private void LerpTransparency()
    {
        float transparency = Mathf.Lerp(0, 1, Mathf.PingPong(Time.time, 1));
        itemColor = new Color(itemColor.r, itemColor.g, itemColor.b, transparency);
        item.GetComponent<Renderer>().material.color = itemColor;
    }

    private void LerpScale()
    {
        Vector3 scale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.PingPong(Time.time, 1));
        item.transform.localScale = scale;
    }

    private void AlterRotate()
    {
        RotateController rotateController = item.GetComponent<RotateController>();
        rotateController.isActivated = true;
        float rotateRatio = Mathf.Lerp(0, 2, Mathf.PingPong(Time.time, 1));
        rotateController.rotateRatio = rotateRatio;
    }

    private void SwitchGravity()
    {
        item.GetComponent<Rigidbody>().useGravity = true;
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

        }
        else
        {
            buttonColor = Color.red;
            isActivated = false;

            // reset state
            switch (buttonIndex)
            {
                case 2:
                    item.GetComponent<Rigidbody>().useGravity = false;
                    break;
                default:
                    item.GetComponent<RotateController>().isActivated = false;
                    break;
            }
        }

        mat.color = buttonColor;    // To reflect button's current state
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("BUTTON OnTriggerEnter");
        HandController controller = other.GetComponent<HandController>(); 
        item = controller.heldItem;
        
        if (controller.ControllerThumbstick.state && item && !isPressed) // prevent continous triggering invoke function
        {
            button.transform.localPosition = new Vector3(0, 0.015f, 0);
            isPressed = true; 
            onPress.Invoke();   // can directly call ButtonTrigger(), but use invoke() for practice
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
