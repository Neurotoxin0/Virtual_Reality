using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandController : MonoBehaviour
{
    public SteamVR_Action_Boolean ControllerGrip; 
    public SteamVR_Action_Boolean ControllerTrigger; 
    public Interactable heldItem;
    public float laserRange = 10f;

    private LineRenderer laser;
    private RaycastHit hit;

    void Start()
    {
        // To make sure we only collide with the actual handles and knobs, We'll use the collisionMatrix.
        // The controller's layers are set to "Controllers" and the Interactables' are set to "Interactables"
        // We can then restrict collisions in Edit->ProjectSettings->Physics 
        if (gameObject.layer != LayerMask.NameToLayer("Controllers")) Debug.LogError("Controllers should be in 'Controllers' Collision Layer");

        laser = GetComponent<LineRenderer>();

    }

    void Update()
    {
        if (ControllerGrip.state) // when pressed down and kept
        {
            Update_Laser();
            laser.enabled = true;
        }
        else laser.enabled = false; 
        
        if (ControllerTrigger.stateUp && heldItem) heldItem.DetachFromController();
    }

    private void Update_Laser()
    {
        laser.SetPosition(0, transform.position);

        if (Physics.Raycast(transform.position, transform.forward, out hit, laserRange)) // if hit something
        {
            laser.SetPosition(1, hit.point);

            // if is pointing interactable obj
            if (hit.collider.tag == "Interactable")
            {
                // TODO
            }
        }
        else    // hit nothing
        {
            laser.SetPosition(1, transform.position + transform.forward * laserRange);
        }
    }

    public void OnItemDetach(Interactable item)
    {
        heldItem = null;
    }

    //Send HoverEnter message to Interactable
    private void OnTriggerEnter(Collider other)
    {
        // The collider we collided with may not be the root object of the interactable.
        // We assume that the rigidbody is.
        if (other.attachedRigidbody == null) return;
        
        var interactable = other.attachedRigidbody.GetComponent<Interactable>();
        if (interactable == null) return;

        if (heldItem == interactable) return;

        interactable.OnHoverEnter(this);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody == null) return;

        var interactable = other.attachedRigidbody.GetComponent<Interactable>();
        if (interactable == null) return;

        if (heldItem == interactable) return;

        // Start Interacting 
        if (ControllerTrigger.stateDown)
        {
            if (interactable.AttachToController(this)) heldItem = interactable;
        }
        else interactable.OnHoverStay(this);
    }

    //Send HoverExit message to Interactable
    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody == null) return;

        var interactable = other.attachedRigidbody.GetComponent<Interactable>();
        if (interactable == null) return;

        if (heldItem == interactable) return;

        interactable.OnHoverStay(this);
    }
}
