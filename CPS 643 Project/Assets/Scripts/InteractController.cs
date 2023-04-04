using UnityEngine;
using Valve.VR.InteractionSystem;

// on interactable items: golf club, golf ball

public class InteractController : Interactable
{

    void Start()
    {
       
    }

    void Update()
    {
        
    }


    // Attach by laser pointer



    // Attach by physical contact
    private void HandHoverUpdate(Hand hand)
    {
        /*
        //Debug.Log("HoverUpdate");
        GrabTypes grabType = hand.GetGrabStarting();

        if (attachedToHand == null && grabType != GrabTypes.None)
        {
            hand.AttachObject(gameObject, grabType);
            hand.HoverLock(this);
        }
        else if (hand.IsGrabEnding(gameObject)) // release obj
        {
            hand.DetachObject(gameObject);
            hand.HoverUnlock(this);
        }
        */
    }

    


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("InteractController OnTriggerEnter: " + other.name);
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("InteractController OnTriggerStay: " + other.name);
    }
}