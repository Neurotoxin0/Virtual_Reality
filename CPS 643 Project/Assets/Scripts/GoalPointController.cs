using System;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]

// on Goal Point.Cup

public class GoalPointController : MonoBehaviour
{
    [Header("Events")]
    public GoalEvent onGoal;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("HoleController Trigger by " + other.name);

        if (other.gameObject.tag == "Golfball")
        {
            //Debug.Log("Goal");
            Destroy(other.gameObject);
            onGoal.Invoke("Level Passed", 5f);
            GameObject.Find("Level 2").GetComponent<TeleportPoint>().locked = false;
        }
    }
}
[Serializable] public class GoalEvent : UnityEvent<string, float> { }