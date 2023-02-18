using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TeleportController : MonoBehaviour
{
    public float laserRange = 10f;
    public SteamVR_Action_Boolean ControllerPadNorth;
    public SteamVR_Action_Boolean ControllerPadButton;
    public GameObject pointer;

    private LineRenderer laser;
    private RaycastHit hit;
    private bool valid_teleport_position;
    private GameObject CameraRig;
    private WaitForSeconds teleport_duration;

    void Start()
    {
        laser = GetComponent<LineRenderer>();
        valid_teleport_position = false;
        teleport_duration = new WaitForSeconds(0.5f);
        CameraRig = GameObject.Find("[CameraRig]").gameObject;
    }

    void Update()
    {
        // Show laser
        if (ControllerPadNorth.state) // when pressed down and kept
        {
            Update_Laser();
            laser.enabled = true;
        }
        else
        {
            Set_Teleport_Conditions(false);
            laser.enabled = false;
        }

        // Teleport
        if (ControllerPadButton.stateDown && valid_teleport_position) StartCoroutine(Teleport());
    }


    private void Update_Laser()
    {
        laser.SetPosition(0, transform.position);
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, laserRange)) // if hit something
        {
            laser.SetPosition(1, hit.point);

            // if is pointing ground
            if (hit.collider.tag == "Ground" && hit.point.y > 0) // second condition prevents few extreme conditions
            {
                Set_Teleport_Conditions(true);
                return;
            }
        }
        else    // hit nothing
        {
            laser.SetPosition(1, transform.position + transform.forward * laserRange);
        }

        Set_Teleport_Conditions(false); // hit nothing or hitpoint cannot teleport to
    }

    private void Set_Teleport_Conditions(bool state)
    {
        /* Determine if now can teleport */
        if (state)
        {
            pointer.transform.position = hit.point;
            pointer.SetActive(true);
            valid_teleport_position = true;
        }
        else
        {
            pointer.SetActive(false);
            valid_teleport_position = false;
        }
    }

    private IEnumerator Teleport()  // use IEnumerator instead of invoke function -> prefer to have all actions in one function
    {
        SteamVR_Fade.View(Color.black, 0.5f);
        yield return teleport_duration;
        CameraRig.transform.position = hit.point;
        SteamVR_Fade.View(Color.clear, 0.5f);  
    }
}
