using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

// on Player.SteamVRObjects.RightHand.VRInputModule

public class VRInputModule : BaseInputModule
{
    public Camera eventCamera;
    public SteamVR_Input_Sources targetSource;
    public SteamVR_Action_Boolean clickAction;

    private GameObject currentObject = null;
    private PointerEventData data = null;

    protected override void Awake()
    {
        base.Awake();   // call base class's Awake()
        data = new PointerEventData(eventSystem);
    }

    public override void Process()
    {
        // Reset data, set camera
        data.Reset();
        data.position = new Vector2(eventCamera.pixelWidth / 2, eventCamera.pixelHeight / 2);

        // Raycast
        eventSystem.RaycastAll(data, m_RaycastResultCache);
        data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        currentObject = data.pointerCurrentRaycast.gameObject;

        // Clear
        m_RaycastResultCache.Clear();

        // Hover
        HandlePointerExitAndEnter(data, currentObject);

        // Press
        if (clickAction.GetStateDown(targetSource))
        {
            ProcessPress(data);
        }

        // Release
        if (clickAction.GetStateUp(targetSource))
        {
            ProcessRelease(data);
        }
    }
    public PointerEventData GetData() { return data; }

    private void ProcessPress(PointerEventData eventData)
    {

    }

    private void ProcessRelease(PointerEventData eventData)
    {
        
    }
}
