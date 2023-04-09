using UnityEngine;

public class UIPointerController : LaserController
{
    public float defaultLength = 5.0f;
    public GameObject dot;
    public VRInputModule vrInputModule;
    private LineRenderer lineRenderer = null;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Start()
    {
        
    }


    void Update()
    {
        UpdateRenderLine();
    }

    void UpdateRenderLine()
    {
        float len = defaultLength;

        RaycastHit hit = CreateRaycast(len);

        Vector3 endPos = transform.position + (transform.forward * len);

        if (hit.collider != null)
        {
            endPos = hit.point;
        }
        lineRenderer.enabled = true;
        dot.transform.position = endPos;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPos);
    }

    private RaycastHit CreateRaycast(float distance)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        Physics.Raycast(ray, out hit, distance);

        return hit;
    }
}
