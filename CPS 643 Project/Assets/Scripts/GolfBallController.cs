using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]

// on Golf Ball

public class GolfBallController : MonoBehaviour
{
    private GameObject camera, goalPoint;


    private void Awake()
    {
        camera = GameObject.Find("Golf Ball Camera");
        goalPoint = GameObject.Find("Goal Point");
    }
    void Start()
    {
        
    }

    void Update()
    {
        //Debug.Log("Golf y: " + transform.position.y);

        // check if fall of the course
        if (transform.position.y < -2) Destroy(gameObject);

        // adjust camera to make sure the ball is facing the goal point
        camera.transform.LookAt(goalPoint.transform);
    }

}
