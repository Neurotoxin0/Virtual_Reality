using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using Valve.VR;
using Debug = UnityEngine.Debug;

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
    public TextMeshProUGUI status;

    void Start()
    {
        // To make sure we only collide with the actual handles and knobs, We'll use the collisionMatrix.
        // The controller's layers are set to "Controllers" and the Interactables' are set to "Interactables"
        // We can then restrict collisions in Edit->ProjectSettings->Physics 
        if (gameObject.layer != LayerMask.NameToLayer("Controllers")) Debug.LogError("Controllers should be in 'Controllers' Collision Layer");

        laser = GetComponent<LineRenderer>();

        GameObject canvas = gameObject.transform.Find("Canvas").gameObject;
        status = canvas.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>();
        
    }

    void Update()
    {
        if (ControllerGrip.state) // when pressed down and kept
        {
            Update_Laser();
            laser.enabled = true;
        }
        else laser.enabled = false;

        // Update Status Text
        UpdateCanvas(); 
    }


    private void UpdateCanvas()
    {
        var holdingItem = (heldItem) ? heldItem.gameObject.name : "Empty";
        var pointingItem = (lastInteractable) ? lastInteractable.gameObject.name : "Empty";
        status.text = "Held Item: " + holdingItem + "\nIs Pointing To: " + pointingItem;
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
            if (ControllerTrigger.stateDown) Detach();  // detach by pointing to nothing
        }
    }

    public void OnItemDetach(CustomInteractable item) 
    { 
        if (heldItem == item) heldItem = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        CustomInteractable interactable = other.GetComponent<CustomInteractable>();
        if (interactable)
        {
            if (interactable == lastInteractable) OnTriggerStay();
            else
            {
                OnTriggerExit();

                lastInteractable = interactable;
                interactable.OnHoverEnter(this);
            }
        }
        else
        {
            if (lastInteractable) OnTriggerExit();
            if (ControllerTrigger.stateDown) Detach();  // detach by pointing to spmething not interactable; i.e. table
        }
    }

    private void OnTriggerStay()
    {
        lastInteractable.OnHoverStay(this);
        if (ControllerTrigger.stateDown) Interact();
    }

    private void OnTriggerExit()
    {
        if (lastInteractable != null)
        {
            lastInteractable.OnHoverExit(this);
            lastInteractable = null;
        }
    }

    private void Interact()
    {
        if (lastInteractable != heldItem)
        {
            Detach();
            Attach();
        }
    }

    private void Attach()
    {
        bool state = lastInteractable.AttachToController(this);
        if (state) heldItem = lastInteractable;
    }

    private void Detach()
    {
        if (heldItem) heldItem.DetachFromController();
        //heldItem = null; -> OnItemDetach() will be invoked
    }
}
