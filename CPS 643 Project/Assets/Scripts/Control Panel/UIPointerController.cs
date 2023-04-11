using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(Camera))] // should have inherited from Laser Controller; but just in case

// on Plater.SteamVRObjects.RightHand.UI Pointer

public class UIPointerController : LaserController
{
    [Header("Configuration")] 
    public GameObject UIPointer;
    //public VRInputModule vrInputModule;

    private bool showPanel;


    private void Start()
    {
        InitLaser();
        showPanel = false;
    }

    void Update()
    {
        if (showPanel)
        {
            UpdateLaser(this.gameObject);
            laser.enabled = true;

            // distance to panel button; provided by EventSystem; 0 if not hit
            //PointerEventData data = vrInputModule.data;
            //float distance = data.pointerCurrentRaycast.distance;

            if (hit.collider != null)
            {
                // show UI pointer
                UIPointer.transform.position = hit.point;
                UIPointer.SetActive(true);
            }
            else UIPointer.SetActive(false);
        }
        else
        {
            laser.enabled = false;
            UIPointer.SetActive(false);
        }
    }

    public void ShowUIPointer(string debug, bool state) // get called by ControlPanelController.ShowPanelEvent->onShowPanel
    {
        showPanel = state;
    }
}
