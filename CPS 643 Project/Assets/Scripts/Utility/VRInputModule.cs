using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

[RequireComponent(typeof(EventSystem))]

// on PlayerFramework.Player.VRInputModule

public class VRInputModule : BaseInputModule
{
    [Header("Configuration")]
    public Camera eventCamera;
    public SteamVR_Action_Boolean UIInteractButton;

    public PointerEventData data { get; private set; }
    private GameObject currentObject;
    
    protected override void Awake()
    {
        base.Awake();   // call base class's Awake()
        data = new PointerEventData(eventSystem);
    }

    public override void Process()
    {
        /* 
         * Raycast using the camera bind to the pointer -> on right hand in this case
         *  -> Player.SteamVRObjects.RightHand.UI Pointer.camera
         */
        
        // Reset data, set position
        data.Reset();
        data.position = new Vector2(eventCamera.pixelWidth / 2, eventCamera.pixelHeight / 2);   // center of the camera

        // Raycast
        eventSystem.RaycastAll(data, m_RaycastResultCache); // m_RaycastResultCache is from BaseInputModule
        data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        currentObject = data.pointerCurrentRaycast.gameObject;
        m_RaycastResultCache.Clear();   // clear cache

        // Hover
        HandlePointerExitAndEnter(data, currentObject);

        // Press & Release
        if (UIInteractButton.stateDown)  ProcessPress(data);
        if (UIInteractButton.stateUp)    ProcessRelease(data);
    }

    private void ProcessPress(PointerEventData data)
    {
        data.pointerPressRaycast = data.pointerCurrentRaycast;

        GameObject onPress = ExecuteEvents.ExecuteHierarchy(currentObject, data, ExecuteEvents.pointerDownHandler);
        if (onPress == null) onPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject); // if no pointerDownHandler, try pointerClickHandler

        // set data
        data.pressPosition = data.position;
        data.pointerPress = onPress;
        data.rawPointerPress = currentObject;
    }

    private void ProcessRelease(PointerEventData data)
    {
        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        GameObject onRelease = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject);

        if (data.pointerPress == onRelease) ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler); // double check

        // clear selected game object
        eventSystem.SetSelectedGameObject(null);

        // reset data
        data.pressPosition = Vector2.zero;
        data.pointerPress = null;
        data.rawPointerPress = null;
    }
}
