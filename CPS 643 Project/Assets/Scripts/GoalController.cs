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

        if (other.gameObject.tag == "Golfball")
        {
            //Debug.Log("Goal");
            Destroy(other.gameObject);
            onGoal.Invoke("Level Passed", -1);
        }
    }
}
[Serializable] public class GoalEvent : UnityEvent<string, int> { }