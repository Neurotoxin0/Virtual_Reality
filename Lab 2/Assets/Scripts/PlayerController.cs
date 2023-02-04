using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Hand = Valve.VR.InteractionSystem.Hand;

public class PlayerController : MonoBehaviour
{
    public int damage = 1;
    public float weaponRange = 50f;
    public float hitForce = 350f;
    public SteamVR_Action_Boolean rightControllerA;
    public SteamVR_Action_Boolean rightControllerB;
    public SteamVR_Action_Boolean rightControllerTrigger;
    public GameObject balloonPrefab;

    private GameObject SteamVRObjects, left_controller, right_controller;
    private LineRenderer laser;
    private WaitForSeconds shotDuration;
    private float nextFire, fireRate;
    private bool showController = true;
    private RaycastHit hit;

    void Start()
    {
        SteamVRObjects = GameObject.Find("SteamVRObjects").gameObject;
        left_controller = SteamVRObjects.transform.Find("LeftHand").gameObject;
        right_controller = SteamVRObjects.transform.Find("RightHand").gameObject;
        laser = GetComponent<LineRenderer>();
        
        fireRate = 0.25f;
        shotDuration = new WaitForSeconds(0.05f);
    }

    void Update()
    {
        Init_Controller();

        // Generate balloon
        if (rightControllerA.stateDown) Generate_Balloon();
        
        // Shoot
        if (rightControllerTrigger.stateDown && Time.time > nextFire) StartCoroutine(Shoot());

        // Debug
        //if (rightControllerB.stateDown) Test();
    }


    private void Init_Controller()
    {
        // if show controller
        foreach (var hand in Player.instance.hands)
        {
            if (showController)
            {
                hand.ShowController();
                hand.SetSkeletonRangeOfMotion(EVRSkeletalMotionRange.WithController);
            }
            else
            {
                hand.HideController();
                hand.SetSkeletonRangeOfMotion(EVRSkeletalMotionRange.WithoutController);
            }
        }
    }

    private void Test(GameObject obj)
    {
        GrabTypes grabType = Player.instance.rightHand.GetGrabStarting();
        Player.instance.rightHand.AttachObject(obj, grabType);
        Debug.Log("attached");
    }
    
    private void Generate_Balloon()
    {
        GameObject balloon = Instantiate(balloonPrefab, right_controller.transform.position, Quaternion.identity) as GameObject;
    }
    
    private IEnumerator Shoot()
    {
        laser.SetPosition(0, right_controller.transform.position);

        if (Physics.Raycast(right_controller.transform.position, right_controller.transform.forward, out hit, weaponRange)) // if hit something
        {
            laser.SetPosition(1, hit.point);

            // if hit a balloon
            BalloonController balloon = hit.collider.GetComponent<BalloonController>(); 
            if (balloon != null) balloon.Damage(damage); // -> balloon.Damage: hitPoint --, apply torque

            // apply extra physical force
            if (hit.rigidbody != null) hit.rigidbody.AddForce(-hit.normal * hitForce);
        }
        else    // hit nothing
        {
            laser.SetPosition(1, right_controller.transform.forward * weaponRange);;
        }
        
        nextFire = Time.time + fireRate;
        laser.enabled = true;
        yield return shotDuration;  // yield return x -> wait for x frame
        laser.enabled = false;

        // TODO: Remove any instantiated objects touched by the laser
    }
}
