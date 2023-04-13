using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
[RequireComponent(typeof(ConstantForce))]

// on Golf Ball

public class GolfBallController : MonoBehaviour
{
    private GameObject camera, goalPoint;
    private ConstantForce constantForce;
    private bool cheatMode, isStriked;

    void Start()
    {
        camera = GameObject.Find("Golf Ball Camera");
        goalPoint = GameObject.Find("Goal Point");
        constantForce = GetComponent<ConstantForce>();

        cheatMode = false;
        isStriked = false;
    }

    void Update()
    {
        //Debug.Log("Golf y: " + transform.position.y);

        // check if fall of the course
        if (transform.position.y < -2) Destroy(gameObject);

        // adjust camera to make sure the ball is facing the goal point
        camera.transform.LookAt(goalPoint.transform);

        Debug.Log("Cheat Mode222: " + cheatMode);
    }

    public void SetCheatMode(bool state) 
    { 
        cheatMode = state;
        Debug.Log("Golf Ball Cheat: " + cheatMode);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("GolfBallController OnCollisionEnter" + collision.collider.name);
        if (collision.gameObject.name == "Golfclub" && !isStriked)
        {
            Debug.Log("Adjust golf ball");
            isStriked = true;

            if (cheatMode) Debug.Log("Cheat Mode111: " + cheatMode);

            // if cheat mode: apply force to "help" the golf ball go towards the goal point :)))
            Vector3 TheForce = camera.transform.position + camera.transform.forward;
            constantForce.force = -TheForce * 0.05f;
            Debug.Log(-TheForce * 0.1f);
            constantForce.enabled = true;
            Invoke("ResetConstantForce", 1.5f);    // too obvious ? no way
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        Invoke("UnlockStrike", 2f);
    }
    private void UnlockStrike() { isStriked = false; }

    private void ResetConstantForce()
    {
        constantForce.force = new Vector3 (0, 0, 0);
        constantForce.enabled = false;
    }
}
