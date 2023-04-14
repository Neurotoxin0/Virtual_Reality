using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
[RequireComponent(typeof(ConstantForce))]

// on Golf Ball

public class GolfBallController : MonoBehaviour
{
    private GameObject camera, goalPoint;
    private ConstantForce constantForce;
    private bool cheat, isAdjusted;

    void Start()
    {
        camera = GameObject.Find("Golf Ball Camera");
        goalPoint = GameObject.Find("Goal Point");
        constantForce = GetComponent<ConstantForce>();

        cheat = false;
        isAdjusted = false;
    }

    void Update()
    {
        // check if fall of the course
        if (transform.position.y < -2) Destroy(gameObject);

        // adjust camera to make sure the ball is facing the goal point
        camera.transform.LookAt(goalPoint.transform);
    }

    public void SetCheatMode(bool state) 
    {
        cheat = state; 
        //Debug.Log("in value: " + state + " Golf Ball Cheat: " + cheat);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("GolfBallController OnCollisionEnter" + collision.collider.name);
        
        if (collision.gameObject.name == "Golfclub")
        {
            //Debug.Log("cheat: " + cheat + ", isadjusted: " + isAdjusted);
            if (cheat && !isAdjusted)
            {
                isAdjusted = true;
                //Debug.Log("Adjust golf ball");

                // if cheat mode: apply force to "help" the golf ball go towards the goal point :)))
                Vector3 TheForce = camera.transform.position + camera.transform.forward;
                constantForce.force = -TheForce.normalized * 10f;
                //Debug.Log(-TheForce.normalized * 10f);
                constantForce.enabled = true;
                Invoke("ResetConstantForce", 1.5f);    // too obvious ? no way
            }

            // Apply force to golf ball to simulate the effect of hitting the golf ball
            GameObject aimpioint = collision.gameObject.transform.GetChild(1).gameObject;
            Vector3 vec = aimpioint.transform.position + aimpioint.transform.forward;
            //Debug.Log(vec + ", " + vec.normalized);
            gameObject.GetComponent<Rigidbody>().AddForce(vec.normalized * 2f);     //TODO: test

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        Invoke("UnlockStrike", 2f);
    }
    private void UnlockStrike() { isAdjusted = false; }

    private void ResetConstantForce()
    {
        constantForce.force = new Vector3 (0, 0, 0);
        constantForce.enabled = false;
    }
}
