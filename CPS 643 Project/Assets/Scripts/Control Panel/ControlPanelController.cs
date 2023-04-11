using UnityEngine;
using Valve.VR;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(Canvas))]

// on Canvas.Control Panel

public class ControlPanelController : MonoBehaviour
{
    [Header("Configuration")] 
    public SteamVR_Action_Boolean showControlPanelButton;

    [Header("Events")]
    public ShowPanelEvent onShowPanel;

    private GameObject playerCamera;
    private Canvas canvas;
    private bool showPanel;

    void Start()
    {
        playerCamera = GameObject.Find("VRCamera");
        canvas = gameObject.GetComponent<Canvas>();
        showPanel = false;
    }

    void Update()
    {
        if (showControlPanelButton.stateDown)
        {
            showPanel = !showPanel;
            onShowPanel.Invoke("", showPanel);
        }

        if (showPanel)
        {
            canvas.enabled = true;  // display canvas only instead of disabling the whole game object -> to keep the script running

            // adjust panel.transform to make sure the player is facing the panel
            //Debug.Log(playerCamera.transform.forward);
            transform.position = new Vector3(playerCamera.transform.forward.x * 100, playerCamera.transform.forward.y * 100, 200);
            transform.rotation = playerCamera.transform.rotation;
        }
        else canvas.enabled = false; 
    }

}
[Serializable] public class ShowPanelEvent : UnityEvent<string, bool> { }