using System;
using UnityEngine;
using UnityEngine.Events;

// on Goal_Point.Cup

public class GoalPointController : MonoBehaviour
{
    [Header("Events")]
    public GoalEvent onGoal;


    void Start()
    {

    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("HoleController Trigger by " + other.name);

        if (other.gameObject.tag == "Golfball")
        {
            //Debug.Log("Goal");
            Destroy(other.gameObject);
            onGoal.Invoke("Level Passed", 5f);
        }
    }
}
[Serializable] public class GoalEvent : UnityEvent<string, float> { }