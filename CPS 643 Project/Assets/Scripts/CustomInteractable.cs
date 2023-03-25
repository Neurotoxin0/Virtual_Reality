using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]

public class CustomInteractable : MonoBehaviour
{
    private Interactable interactable;

    void Start()
    {
        interactable = GetComponent<Interactable>();
    }


    void Update()
    {
        
    }

    // Attach
    private void HandHoverUpdate(Hand hand)
    {
        //Debug.Log("HoverUpdate");
        GrabTypes grabType = hand.GetGrabStarting();

        if (interactable.attachedToHand == null && grabType != GrabTypes.None)
        {
            hand.AttachObject(gameObject, grabType);
            hand.HoverLock(interactable);
        }
        else if (hand.IsGrabEnding(gameObject)) // release obj
        {
            hand.DetachObject(gameObject);
            hand.HoverUnlock(interactable);
        }
    }
}
