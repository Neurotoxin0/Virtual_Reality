using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BalloonController : MonoBehaviour
{
    public int hitPoint = 1;

    private bool grown, dead, lock1;
    private Vector3 scale_ratio, constantForce_ratio, One, Ten, Zero;
    private Interactable interactable;
    private ConstantForce constantforce;
    private Rigidbody rigidbody;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        constantforce = GetComponent<ConstantForce>();
        rigidbody = GetComponent<Rigidbody>();

        // Init Shortcuts
        One = new Vector3(1f, 1f, 1f);
        Ten = new Vector3(0f, 10f, 0f);
        Zero = new Vector3(0f, 0f, 0f);
        
        // Init state
        grown = false;
        dead = false;
        lock1 = false;
        
        // Override scale
        scale_ratio = Zero;
        transform.localScale = scale_ratio;
        constantForce_ratio = Zero;
        constantforce.force = constantForce_ratio;

        // Override constant force & gravity settings
        constantforce.enabled = true;
        rigidbody.useGravity = true;

    }
    void Update()
    {
        if (hitPoint > 0)
        {
            // Grow when generated; Float when grown
            if (!grown)
            {
                // attach to hand
                GrabTypes grabType = Player.instance.rightHand.GetGrabStarting();
                Player.instance.rightHand.AttachObject(gameObject, grabType);
                Player.instance.rightHand.HoverLock(interactable);

                rigidbody.isKinematic = true;
                Scaling(0);
            }
            else
            {
                // detach
                if (!lock1) // prevent continous detach
                {
                    Player.instance.rightHand.DetachObject(gameObject);
                    Player.instance.rightHand.HoverUnlock(interactable);
                    lock1 = true;

                    GameObject.Find("Player").SendMessage("Unlock_Lock1");  // unlock so that player can generate another balloon
                }

                rigidbody.isKinematic = false;
            }
        }
        else
        {
            // Elimitation: deflate -> Destroy
            if (!dead) Scaling(1);
            else Destroy(gameObject, 5);
        }

        // height limit
        if (transform.position.y >= 10) Destroy(gameObject, 0);
    }


    private void Scaling(int stage)  
    {
        /*
        @param stage:
            0: scale from 0 to 1    -> grow
            1: scale from 1 to 0.25 -> deflate
        */
        if (stage == 0)
        {
            if (transform.localScale.y < 0.98 || constantforce.force.y < 9.98)
            {
                // gradually increase its size and floating force
                scale_ratio = Vector3.Lerp(scale_ratio, One, 0.025f);
                constantForce_ratio = Vector3.Lerp(constantForce_ratio, Ten, 0.25f);

                transform.localScale = scale_ratio;
                constantforce.force = constantForce_ratio;
            }
            else grown = true;
        }
        else if (stage == 1)
        {
            if (transform.localScale.y > 0.25 || constantforce.force.y > 0.1)
            {
                // gradually decrease its size and floating force
                scale_ratio = Vector3.Lerp(scale_ratio, Zero, 0.01f);
                constantForce_ratio = Vector3.Lerp(constantForce_ratio, Zero, 0.01f);

                transform.localScale = scale_ratio;
                constantforce.force = constantForce_ratio;
            }
            else dead = true;
        }
    }

    public void Damage(int damage) 
    { 
        hitPoint -= damage; 

        // Apply torque when hit, with random direction
        Vector3 random_vector = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));
        constantforce.torque = random_vector;
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
