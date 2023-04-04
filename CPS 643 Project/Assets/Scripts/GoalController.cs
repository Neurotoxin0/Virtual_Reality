using System;
using UnityEngine;
using UnityEngine.Events;

// on Goal_Point.Cup

public class GoalController : MonoBehaviour
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

        if (other.gameObject.tag == "Golf Ball")
        {
            //Debug.Log("Goal");
            Destroy(other.gameObject);
            onGoal.Invoke(other.gameObject.name, "Level Passed");
        }
    }
}
[Serializable] public class GoalEvent : UnityEvent<string, string> { }