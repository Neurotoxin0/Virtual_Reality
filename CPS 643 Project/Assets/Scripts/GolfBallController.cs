using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]

// on Golf Ball

public class GolfBallController : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void Update()
    {
        // check if fall of the course
        if (transform.position.y < -2) Destroy(this, 2); 
    }
}
