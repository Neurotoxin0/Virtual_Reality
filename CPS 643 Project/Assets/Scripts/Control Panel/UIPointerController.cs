using UnityEngine;
using UnityEngine.EventSystems;

// on Plater.SteamVRObjects.RightHand.UI Pointer

public class UIPointerController : LaserController
{
    public float defaultLength = 5.0f;
    public GameObject dot;
    public VRInputModule vrInputModule;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        UpdateRenderLine();
    }

    void UpdateRenderLine()
    {
        // if hit, use hit point or use default length
        PointerEventData data = vrInputModule.data;
        float len = data.pointerCurrentRaycast.distance == 0 ? defaultLength : data.pointerCurrentRaycast.distance;

        RaycastHit hit = CreateRaycast(len);

        Vector3 endPos = hit.collider != null ? hit.point : transform.position + (transform.forward * len);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPos);
        lineRenderer.enabled = true;
        dot.transform.position = endPos;    // set dot position
    }

    private RaycastHit CreateRaycast(float distance)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        Physics.Raycast(ray, out hit, distance);

        return hit;
    }
}
