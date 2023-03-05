using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[RequireComponent(typeof(Rigidbody), typeof(Collider), typeof(PositionLimiter))]

public class Interactable : MonoBehaviour
{
    public HandController attachedController { get; private set; }

    private Material mat;

    void Start()
    {
        // To make sure we only collide with the actual handles and knobs, We'll use the collisionMatrix.
        // The controller's layers are set to "Controllers" and the Interactables' are set to "Interactables"
        // We can then restrict collisions in Edit->ProjectSettings->Physics 
        if (gameObject.layer != LayerMask.NameToLayer("Interactables")) Debug.LogError("Interactables should be in 'Interactables' Collision Layer");

        mat = GetComponent<Renderer>().material;
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


    protected virtual void OnBeginInteraction()
    {
        //Debug.Log("Begin Interaction");
        mat.color = Color.yellow;   // hightlight
    }

    protected virtual void OnEndInteraction()
    {
        //Debug.Log("End Interaction");
        Color currentColor = GetComponent<Renderer>().material.color; 
        if (currentColor == Color.yellow) mat.color = Color.white; // if no change to color was made -> return to default color
    }

    public virtual void OnHoverEnter(HandController controller) 
    {
        //Debug.Log("Hover Enter");
    }

    public virtual void OnHoverExit(HandController controller)
    {
        //Debug.Log("Hover Exit");
    }

    public virtual void OnHoverStay(HandController controller)
    {
        //Debug.Log("Hover Stay");
    }
}
