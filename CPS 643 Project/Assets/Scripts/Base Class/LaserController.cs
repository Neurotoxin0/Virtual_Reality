using UnityEngine;

// Base class for raycast laser; used by PlayerController and UIPointerController

[RequireComponent(typeof(LineRenderer))]

public class LaserController : MonoBehaviour
{
    [Header("Laser Configuration")]
    public float laserRange = 10f;

    protected private LineRenderer laser;
    protected private RaycastHit hit;
        
    protected virtual void InitLaser() 
    {
        laser = GetComponent<LineRenderer>();
    }

    protected virtual void UpdateLaser(GameObject referenceObj)
    {
        laser.SetPosition(0, referenceObj.transform.position);

        if (Physics.Raycast(referenceObj.transform.position, referenceObj.transform.forward, out hit, laserRange))    // if hit something
        {
            laser.SetPosition(1, hit.point);
        }
        else    // hit nothing
        {
            laser.SetPosition(1, referenceObj.transform.position + referenceObj.transform.forward * laserRange); 
        }
    }
}
