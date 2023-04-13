using System;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;
using static Valve.VR.InteractionSystem.Hand;

[RequireComponent(typeof(LineRenderer))] // should have inherited from Laser Controller; but just in case

// on PlayerFramework.Player

public class PlayerController : LaserController
{
    [Header("Configuration")]
    public GameObject golfClubInstance;
    public GameObject golfBallPrefab;
    public GameObject golfBallPointerPrefab;
    public SteamVR_Action_Boolean showGolfClubButton;
    public SteamVR_Action_Boolean spawnGolfBallButton;
    public SteamVR_Action_Boolean leftTriggerButton;    // spawn golf ball

    [Header("Events")]
    public SpawnEvent onSpawnGolfBall;  // related: Screen.ScreenController.UpdateGolfBallCnt; Utility.HapticCpntroller.ShortPulse;

    private Hand leftController, rightController;
    private GameObject golfBallPointer;
    private ScreenController screenController;
    private int golfBallCount;  // how many left
    private bool showController, showHoverDetail;
    private bool spawnMode, showClub;

    void Start()
    {
        InitLaser();    // from BaseClass: LaserController

        leftController = Player.instance.leftHand;
        rightController = Player.instance.rightHand;
        screenController = GameObject.Find("Screen").GetComponent<ScreenController>();

        // use left hand to spawn golf ball and right hand to interact with UI
        golfBallPointer = Instantiate(golfBallPointerPrefab, leftController.gameObject.transform);

        golfBallCount = 5;
        showController = true;
        showHoverDetail = true;
        spawnMode = false;  // spawn golf ball
        showClub = true;
    }

    
    void Update()
    {
        ShowController();
        ShowHoverDetail();
        ShowGolfClub();

        // change status
        if (showGolfClubButton.stateDown) showClub = !showClub;
        if (spawnGolfBallButton.stateDown)    // only can spawn new golf ball when there is no golf ball in the scene
        {
            if (GameObject.FindWithTag("Golfball") == null) spawnMode = !spawnMode;
            else screenController.ShowMsg("There is already a golf ball in the scene", 2, Color.red);
        }

        // spawn golf ball
        if (spawnMode)
        {
            //Debug.Log("Spawn Mode");
            
            UpdateLaser(leftController.gameObject);
            laser.enabled = true;

            if (hit.collider != null && hit.collider.gameObject.tag == "Ground")
            {
                // show golf ball pointer
                Vector3 pos = new Vector3(hit.point.x, (float)(hit.point.y + 0.05), hit.point.z);   // offset a bit to make sure the golf ball is on the ground
                golfBallPointer.transform.position = pos;
                golfBallPointer.SetActive(true);

                if (leftTriggerButton.stateDown)
                {
                    //Debug.Log("Spawn golf ball");
                    bool state = SpawnGolfBall();
                    if (state) spawnMode = false;   // turn off spawn mode when finish spawning
                } 
            }
            else golfBallPointer.SetActive(false);
        }
        else
        {
            laser.enabled = false;
            golfBallPointer.SetActive(false);
        }
    }


    private void ShowController()
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

    private void ShowHoverDetail()
    {
        foreach (var hand in Player.instance.hands)
        {
            if (showHoverDetail)
            {
                hand.showDebugText = true;
            }
            else
            {
                hand.showDebugText = false;
            }
        }
        
    }

    private AttachmentFlags defaultAttachmentFlags = AttachmentFlags.ParentToHand | AttachmentFlags.DetachOthers | AttachmentFlags.DetachFromOtherHand | AttachmentFlags.TurnOnKinematic | AttachmentFlags.SnapOnAttach;
    private void ShowGolfClub()
    {
        GrabTypes grabType = rightController.GetGrabStarting();

        if (showClub)
        {                                                                                    // apply hover point offset
            rightController.AttachObject(golfClubInstance, grabType, defaultAttachmentFlags, golfClubInstance.transform.GetChild(0).transform);
            rightController.HoverLock(golfClubInstance.GetComponent<Interactable>());
            golfClubInstance.SetActive(true);
        }
        else
        {
            rightController.DetachObject(golfClubInstance);
            rightController.HoverUnlock(golfClubInstance.GetComponent<Interactable>());
            golfClubInstance.SetActive(false);
        }
        
    }

    private bool SpawnGolfBall()
    {
        if (golfBallCount > 0)
        {
            Vector3 offset = new Vector3(0, 0.05f, 0);
            GameObject golfBall = Instantiate(golfBallPrefab, hit.point + offset, Quaternion.identity) as GameObject;
            golfBallCount--;
            onSpawnGolfBall.Invoke(Player.instance.leftHand, golfBallCount);
            return true;
        }
        return false;
    }

    public void InvokeonSpawnGolfBall() { onSpawnGolfBall.Invoke(null, golfBallCount); }

}
[Serializable] public class SpawnEvent : UnityEvent<Hand, int> { }