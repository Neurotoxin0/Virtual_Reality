using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;
using Hand = Valve.VR.InteractionSystem.Hand;
using System;

// on Player instance

public class PlayerController : LaserController
{
    [Header("Configuration")]
    public GameObject golfClub;
    public GameObject golfBallPrefab;
    public SteamVR_Action_Boolean showGolfClubButton;
    public SteamVR_Action_Boolean spawnGolfBallButton;
    public SteamVR_Action_Boolean triggerButton;

    [Header("Events")]
    public SpawnEvent onSpawnGolfBall;

    private Hand leftController, rightController;
    private int golfBallCount;  // how many left
    private bool showController, showHoverDetail;
    private bool spawnMode, showClub;

    void Start()
    {
        InitLaser();

        leftController = Player.instance.leftHand;
        rightController = Player.instance.rightHand;
        referenceObj = leftController.gameObject;  // use left hand to spawn golf ball; ref obj used by laser

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
        if (spawnGolfBallButton.stateDown) spawnMode = !spawnMode;
        

        if (spawnMode)
        {
            UpdateLaser();
            laser.enabled = true;

            if (hit.collider != null && hit.collider.gameObject.tag == "Ground" && triggerButton.stateDown)
            {
                //Debug.Log("Spawn golf ball");
                if (SpawnGolfBall()) spawnMode = false;
            }
        }
        else laser.enabled = false;
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

    private void ShowGolfClub()
    {
        GrabTypes grabType = rightController.GetGrabStarting();

        if (showClub)
        {
            rightController.AttachObject(golfClub, grabType);
            rightController.HoverLock(golfClub.GetComponent<Interactable>());
            golfClub.SetActive(true);
        }
        else
        {
            rightController.DetachObject(golfClub);
            rightController.HoverUnlock(golfClub.GetComponent<Interactable>());
            golfClub.SetActive(false);
        }
        
    }

    private bool SpawnGolfBall()
    {
        if (golfBallCount > 0)
        {
            GameObject golfBall = Instantiate(golfBallPrefab, hit.point, Quaternion.identity) as GameObject;
            golfBallCount--;
            onSpawnGolfBall.Invoke(golfBall.gameObject.name, golfBallCount);
            return true;
        }
        return false;
    }

    public void InvokeonSpawnGolfBall() { onSpawnGolfBall.Invoke("Manually", golfBallCount); }

}
[Serializable] public class SpawnEvent : UnityEvent<string, int> { }