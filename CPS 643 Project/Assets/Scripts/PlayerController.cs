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
    public GameObject golfClubInstance;
    public GameObject golfBallPrefab;
    public GameObject golfBallPointerPrefab;
    public SteamVR_Action_Boolean showGolfClubButton;
    public SteamVR_Action_Boolean spawnGolfBallButton;
    public SteamVR_Action_Boolean showControlPanelButton;
    public SteamVR_Action_Boolean leftTriggerButton;
    public SteamVR_Action_Boolean rightTriggerButton;

    [Header("Events")]
    public SpawnEvent onSpawnGolfBall;
    public PanelEvent onInteractWithUI;

    private Hand leftController, rightController;
    private GameObject golfBallPointer;
    private int golfBallCount;  // how many left
    private bool showController, showHoverDetail;
    private bool spawnMode, showClub, showPanel;

    void Start()
    {
        InitLaser();

        leftController = Player.instance.leftHand;
        rightController = Player.instance.rightHand;

        // use left hand to spawn golf ball and right hand to interact with UI
        golfBallPointer = Instantiate(golfBallPointerPrefab, leftController.gameObject.transform);

        golfBallCount = 5;
        showController = true;
        showHoverDetail = true;
        spawnMode = false;  // spawn golf ball
        showClub = true;
        showPanel = false;
    }

    
    void Update()
    {
        ShowController();
        ShowHoverDetail();
        ShowGolfClub();

        // change status
        if (showGolfClubButton.stateDown) showClub = !showClub; 
        if (spawnGolfBallButton.stateDown) spawnMode = !spawnMode;
        if (showControlPanelButton.stateDown) showPanel = !showPanel;

        // spawn golf ball
        if (spawnMode)
        {
            //Debug.Log("Spawn Mode");
            
            UpdateLaser(leftController.gameObject);
            laser.enabled = true;

            if (hit.collider != null && hit.collider.gameObject.tag == "Ground")
            {
                Vector3 pos = new Vector3(hit.point.x, (float)(hit.point.y + 0.5), hit.point.z);
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
            //laser.enabled = false;
            golfBallPointer.SetActive(false);
        }

        // open the control panel
        if (showPanel)
        {
            //Debug.Log("Show Panel");
            
            onInteractWithUI.Invoke("", true);
            UpdateLaser(rightController.gameObject);
            laser.enabled = true;

            if (hit.collider != null && hit.collider.gameObject.name == "Control Panel")
            {
                Debug.Log("Hit Panel");
            }
        }
        else
        {
            onInteractWithUI.Invoke("", false);
            //laser.enabled = false;
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

    private void ShowGolfClub()
    {
        GrabTypes grabType = rightController.GetGrabStarting();

        if (showClub)
        {
            rightController.AttachObject(golfClubInstance, grabType);
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
[Serializable] public class PanelEvent : UnityEvent<string, bool> { }