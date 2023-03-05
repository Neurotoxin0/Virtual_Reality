using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(LineRenderer), typeof(Rigidbody), typeof(Collider))]

public class HandController : MonoBehaviour
{
    public Interactable heldItem { get; private set; }

    public SteamVR_Action_Boolean ControllerGrip; 
    public SteamVR_Action_Boolean ControllerTrigger; 
    public SteamVR_Action_Boolean ControllerThumbstick;
    public float laserRange = 10f;

    private LineRenderer laser;
    private RaycastHit hit;
    private Interactable lastInteractable;
    private TextMeshProUGUI status;

    void Start()
    {
        // To make sure we only collide with the actual handles and knobs, We'll use the collisionMatrix.
        // The controller's layers are set to "Controllers" and the Interactables' are set to "Interactables"
        // We can then restrict collisions in Edit->ProjectSettings->Physics 
        if (gameObject.layer != LayerMask.NameToLayer("Controllers")) Debug.LogError("Controllers should be in 'Controllers' Collision Layer");

        GameObject canvas = gameObject.transform.Find("Canvas").gameObject;
        status = canvas.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>();
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

        // Update Status Text
        UpdateCanvas(); 
    }


    private void UpdateCanvas()
    {
        string holdingItem = (heldItem) ? heldItem.gameObject.name : "Empty";
        string pointingItem = (lastInteractable) ? lastInteractable.gameObject.name : "Empty";
        status.text = "Held Item: " + holdingItem + "\nIs Pointing To: " + pointingItem;
    }

    private void Update_Laser()
    {
        laser.SetPosition(0, transform.position);

        if (Physics.Raycast(transform.position, transform.forward, out hit, laserRange)) // if hit something
        {
            laser.SetPosition(1, hit.point);

            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable)
            {
                if (lastInteractable == null)
                {
                    lastInteractable = interactable;
                    OnLaserEnter();
                }
                else if (interactable == lastInteractable) OnLaserStay();
                else // pointing to different obj
                {
                    OnLaserExit();
                    lastInteractable = interactable;
                    OnLaserEnter();
                }

            }
            else
            {
                if (lastInteractable) OnLaserExit();
                if (ControllerTrigger.stateDown) Detach();  // detach by pointing to something not interactable; i.e. table, monitor
            }
        }
        else    // hit nothing
        {
            laser.SetPosition(1, transform.position + transform.forward * laserRange);

            if (lastInteractable) OnLaserExit();
            if (ControllerTrigger.stateDown) Detach();  // detach by pointing to nothing
        }
    }

    private void Interact()
    {
        if (lastInteractable != heldItem)   // already held item -> detach old obj + attach new obj
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
        //heldItem = null; -> OnItemDetach() will be invoked and heldItem will be reset
    }


    public void OnItemDetach(Interactable item) 
    { 
        if (heldItem == item) heldItem = null;
    }

    private void OnLaserEnter()
    {
        //Debug.Log("HAND OnLaserEnter");
        lastInteractable.OnHoverEnter(this);
    }

    private void OnLaserStay()
    {
        //Debug.Log("HAND OnLaserStay"); 
        lastInteractable.OnHoverStay(this);
        if (ControllerTrigger.stateDown) Interact();
    }

    private void OnLaserExit()
    {
        //Debug.Log("HAND OnLaserExit"); fr

        if (lastInteractable != null)
        {
            lastInteractable.OnHoverExit(this);
            lastInteractable = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("HAND OnTriggerEnter");
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("HAND OnTriggerStay");
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("HAND OnTriggerExit");
    }
}
