using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(LineRenderer))]

public class HandController : MonoBehaviour
{
    public CustomInteractable heldItem { get; private set; }
    public SteamVR_Action_Boolean ControllerGrip; 
    public SteamVR_Action_Boolean ControllerTrigger; 
    public float laserRange = 10f;

    private LineRenderer laser;
    private RaycastHit hit;
    private CustomInteractable lastInteractable;

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
            OnTriggerEnter(hit.collider);
        }
        else    // hit nothing
        {
            laser.SetPosition(1, transform.position + transform.forward * laserRange);
            if (lastInteractable) OnTriggerExit();
        }
    }

    public void OnItemDetach(CustomInteractable item) { heldItem = null; }

    private void OnTriggerEnter(Collider other)
    {
        CustomInteractable interactable = other.GetComponent<CustomInteractable>();
        if (interactable)
        {
            if (interactable == lastInteractable) OnTriggerStay();
            else
            {
                if (lastInteractable != null)
                {
                    OnTriggerExit();
                }
                lastInteractable = interactable;
                lastInteractable.OnHoverEnter(this);
            }
        }
        else if (lastInteractable) OnTriggerExit();
    }

    private void OnTriggerStay()
    {
       lastInteractable.OnHoverStay(this);

        // Start Interacting 
        if (ControllerTrigger.stateDown)
        {
            bool state = lastInteractable.AttachToController(this);
            if (state) heldItem = lastInteractable;
        }
    }

    private void OnTriggerExit()
    {
        lastInteractable.OnHoverExit(this);
        lastInteractable = null;
    }
}
