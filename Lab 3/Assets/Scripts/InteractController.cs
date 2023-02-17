using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InteractController : MonoBehaviour
{
    public float BreakForce = 10f;
    public SteamVR_Action_Boolean ControllerTrigger;
    public SteamVR_Action_Boolean ControllerGrip;

    private bool valid_interactable_item, is_grabbing;
    private GameObject interactable_item;
    private FixedJoint hover_point;
    private SteamVR_Behaviour_Pose behavior;

    void Start()
    {
        behavior = GetComponent<SteamVR_Behaviour_Pose>();
        hover_point = GetComponent<FixedJoint>();
        valid_interactable_item = false;
        is_grabbing = false;
    }

    void Update()
    {
        Generate_Joint();
        Grab_Item();
    }


    private void Generate_Joint()
    {
        /* Check if fixed joint still exist -> will be destroyed after OnJointBreak() */
        if (!hover_point)
        {
            gameObject.AddComponent<FixedJoint>();
            hover_point = GetComponent<FixedJoint>();
        }
    }

    private void Grab_Item()
    {
        if (valid_interactable_item)
        {
            if (ControllerTrigger.state)
            {
                is_grabbing = true;
                hover_point.connectedBody = interactable_item.GetComponent<Rigidbody>();
                hover_point.breakForce = BreakForce;

                // Interactable item state control
                if (interactable_item.GetComponent<LightController>() != null)  // is a flashlight
                { 
                    if (ControllerGrip.stateDown)
                    {
                        //Debug.Log("Send Light Message");
                        GameObject.Find("HandLight").SendMessage("Switch_Power_State");
                        //Debug.Log("Send Light Message OVER");
                    }
                }
            }
            if (ControllerTrigger.stateUp && is_grabbing)
            {
                hover_point.connectedBody = null;

                Vector3 angular = behavior.GetAngularVelocity();
                float angular_magnitude = angular.magnitude;
                Vector3 velocity = behavior.GetVelocity();
                float velocity_magnitude = velocity.magnitude;
                //Debug.Log("angular: " + angular + ", mag: " + angular_magnitude);
                //Debug.Log("vel: " + velocity + ", mag: " + velocity_magnitude);

                //interactable_item.GetComponent<Rigidbody>().AddForce(-angular.normalized * velocity_magnitude, ForceMode.Impulse);
                interactable_item.GetComponent<Rigidbody>().velocity = -angular.normalized * velocity_magnitude;

                is_grabbing = false;
            }
        }
    }

    //private void OnTriggerEnter(Collider other) { Debug.Log("enter"); }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Interactable")
        {
            interactable_item = other.gameObject; 
            valid_interactable_item = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactable_item = null;
        valid_interactable_item = false;
    }

    //private void OnJointBreak(float breakForce) Debug.Log(breakForce);
}
