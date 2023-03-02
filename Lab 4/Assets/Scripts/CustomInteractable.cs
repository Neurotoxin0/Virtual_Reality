using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]

public class CustomInteractable : MonoBehaviour
{
    public HandController attachedController { get; private set; }

    void Start()
    {
        // To make sure we only collide with the actual handles and knobs, We'll use the collisionMatrix.
        // The controller's layers are set to "Controllers" and the Interactables' are set to "Interactables"
        // We can then restrict collisions in Edit->ProjectSettings->Physics 
        if (gameObject.layer != LayerMask.NameToLayer("Interactables")) Debug.LogError("Interactables should be in 'Interactables' Collision Layer");

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
        if (attachedController.heldItem == this) attachedController.OnItemDetach(this);
        else Debug.LogError("Controller state (HeldItem) was incorrect. Tried to detach " + this + " while holding " + attachedController.heldItem);
        attachedController = null;

        OnEndInteraction();
    }

    //Called when the user begins interacting with this object. 
    protected virtual void OnBeginInteraction()
    {
        Debug.Log("Begin Interaction");
    }

    //Called  when the user stops interacting with this object. 
    protected virtual void OnEndInteraction()
    {
        Debug.Log("End Interaction");
    }

    //No need for OnInteractionStay. You can issue updates for your object in Update() and check if attachedController is null;

    //Called by WandController when an unattached controller overlaps this object's collider.
    public virtual void OnHoverEnter(HandController controller) 
    {
        Debug.Log("Hover Enter");
    }

    //Called by WandController when an unattached controller overlaps this object's collider.
    public virtual void OnHoverExit(HandController controller)
    {
        Debug.Log("Hover Exit");
    }

    //Called by WandController on each frame an unattached controller overlaps this object's collider.
    public virtual void OnHoverStay(HandController controller)
    {
        Debug.Log("Hover Stay");
    }
}
