using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
[RequireComponent(typeof(ConstantForce))]

// on Golf Ball

public class GolfBallController : MonoBehaviour
{
    public bool cheatMode { get; private set; }
    private GameObject camera, goalPoint;
    private ConstantForce constantForce;

    void Start()
    {
        camera = GameObject.Find("Golf Ball Camera");
        goalPoint = GameObject.Find("Goal Point");
        constantForce = GetComponent<ConstantForce>();

        cheatMode = true;   // TODO
    }

    void Update()
    {
        //Debug.Log("Golf y: " + transform.position.y);

        // check if fall of the course
        if (transform.position.y < -2) Destroy(gameObject);

        // adjust camera to make sure the ball is facing the goal point
        camera.transform.LookAt(goalPoint.transform);
    }

    public void SetCheatMode(bool state) { cheatMode = state; Debug.Log("Golf Ball Cheat: " + cheatMode); }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("GolfBallController OnCollisionEnter" + collision.collider.name);
        if (collision.gameObject.name == "Golfclub" && cheatMode)
        {
            Debug.Log("Adjust golf ball");
            
            // if cheat mode: apply force to "help" the golf ball go towards the goal point :)))
            Vector3 TheForce = camera.transform.position + camera.transform.forward;
            constantForce.force = -TheForce * 0.1f;
            constantForce.enabled = true;
            Invoke("ResetConstantForce", 1.5f);    // too obvious ? no way
        }
    }

    private void ResetConstantForce()
    {
        constantForce.force = new Vector3 (0, 0, 0);
        constantForce.enabled = false;
    }
}
