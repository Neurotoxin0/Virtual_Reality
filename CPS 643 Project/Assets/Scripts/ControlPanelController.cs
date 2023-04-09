using UnityEngine;

// on Canvas.Control Panel

public class ControlPanelController : MonoBehaviour
{
    private GameObject playerCamera;

    void Start()
    {
        playerCamera = GameObject.Find("VRCamera");
        gameObject.SetActive(false);
    }

    void Update()
    {
        //Debug.Log(playerCamera.transform.forward);
        transform.position = new Vector3(playerCamera.transform.forward.x * 100, playerCamera.transform.forward.y * 100, 170);
        transform.rotation = playerCamera.transform.rotation;
    }

    public void ChangePanelActive(string debugInfo, bool state)
    {
        // if we need to make panel active, we need to make sure the player is facing the panel
        if (state)
        {

        }
        gameObject.SetActive(state);
    }
}