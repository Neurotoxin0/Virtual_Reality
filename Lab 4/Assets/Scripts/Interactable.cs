using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(PositionLimiter))]

public class Interactable : MonoBehaviour
{
    public HandController attachedController { get; private set; }

    public Material hightlight;

    private Material[] mat;
    private RotateController rotateController;
    private Rigidbody rigidBody;
    private Color color;
    private bool selected;

    void Start()
    {
        // To make sure we only collide with the actual handles and knobs, We'll use the collisionMatrix.
        // The controller's layers are set to "Controllers" and the Interactables' are set to "Interactables"
        // We can then restrict collisions in Edit->ProjectSettings->Physics 
        if (gameObject.layer != LayerMask.NameToLayer("Interactables")) Debug.LogError("Interactables should be in 'Interactables' Collision Layer");

        mat = GetComponent<Renderer>().materials;
        rotateController = GetComponent<RotateController>();
        rigidBody = GetComponent<Rigidbody>();
        color = mat[0].color;
        selected = false;
    }


    public bool AttachToController(HandController controller)
    {
        if (controller.heldItem != null) return false;  // controller already holding item

        //Steal object from other controller.
        if (attachedController != null) DetachFromController();
        attachedController = controller;

        OnBeginInteraction();
        return true;
    }

    public void DetachFromController()
    {
        if (attachedController.heldItem == this)
        {
            attachedController.OnItemDetach(this);
            attachedController = null;
        }
        else Debug.LogError("Controller state (HeldItem) was incorrect. Tried to detach " + this + " while holding " + attachedController.heldItem);
        
        OnEndInteraction();
    }

    public string AlterTransparency()
    {
        float transparency = Mathf.Lerp(0, 1, Mathf.PingPong(Time.time, 1));
        color = new Color(color.r, color.g, color.b, transparency);
        GetComponent<Renderer>().materials[0].color = color;

        return "Transparency: " + transparency + "\n ";
    }
    public string AlterTransparency(float value)
    {
        color = new Color(color.r, color.g, color.b, value);
        GetComponent<Renderer>().materials[0].color = color;

        return "Transparency: " + value + "\n ";
    }

    public string AlterColor()
    {
        float transparency = color.a;
        color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
        color.a = transparency;
        GetComponent<Renderer>().materials[0].color = color;
        return "Color: " + color + "\n ";
    }
    public string AlterColor(float value)
    {
        float rgb = 255 * value / 100;
        color = new Color(rgb, rgb, rgb, color.a);
        GetComponent<Renderer>().materials[0].color = color;
        return "Color: " + color + "\n ";
    }

    public string AlterScale()
    {
        Vector3 scale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.PingPong(Time.time, 1));
        transform.localScale = scale;

        return "Scale: " + scale + "\n ";
    }
    public string AlterScale(float value)
    {
        float scaleRatio = value / 2;
        transform.localScale = Vector3.one * scaleRatio;

        return "Scale: " + scaleRatio + "\n ";
    }

    public string AlterRotate()
    {
        rotateController.isActivated = true;
        float rotateRatio = Mathf.Lerp(0, 2, Mathf.PingPong(Time.time, 1));
        rotateController.rotateRatio = rotateRatio;

        return "Rotate: " + rotateController.isActivated + "\n " +
               "Rotate Ratio: " + rotateRatio + "\n ";
    }
    public string AlterRotate(float value)
    {
        rotateController.isActivated = true;
        float rotateRatio = value * 2;
        rotateController.rotateRatio = rotateRatio;

        return "Rotate: " + rotateController.isActivated + "\n " +
               "Rotate Ratio: " + rotateRatio + "\n ";
    }

    public string AlterGravity()
    {
        GetComponent<Rigidbody>().useGravity = true;
        rotateController.isActivated = false;

        return "Gravity: " + rigidBody.useGravity + "\n " +
               "Rotate: " + rotateController.isActivated + "\n ";
    }



    protected virtual void OnBeginInteraction()
    {
        //Debug.Log("Begin Interaction");
        selected = true;
    }

    protected virtual void OnEndInteraction()
    {
        //Debug.Log("End Interaction");
        selected = false;
        mat[1] = null;   // remove hightlight
        GetComponent<Renderer>().materials = mat;
    }

    public virtual void OnHoverEnter(HandController controller) 
    {
        //Debug.Log("Hover Enter");
        mat[1] = hightlight;   // hightlight
        GetComponent<Renderer>().materials = mat;
    }

    public virtual void OnHoverExit(HandController controller)
    {
        //Debug.Log("Hover Exit");
        if (!selected) mat[1] = null;   // remove hightlight
        GetComponent<Renderer>().materials = mat;
    }

    public virtual void OnHoverStay(HandController controller)
    {
        //Debug.Log("Hover Stay");
    }
}
