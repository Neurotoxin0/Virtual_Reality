using System;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]

// on Goal Point.Cup

public class GoalPointController : MonoBehaviour
{
    [Header("Configuration")]
    public GameObject nextLevelTeleportPoint;
    [Header("Events")]
    public GoalEvent onGoal;

    public void Start()
    {
        //Debug.Log(nextLevelTeleportPoint.GetComponent<TeleportPoint>().locked);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("HoleController Trigger by " + other.name);

        if (other.gameObject.tag == "Golfball")
        {
            //Debug.Log("Goal");
            Destroy(other.gameObject);
            onGoal.Invoke("Level Passed", 5f, Color.green);
            nextLevelTeleportPoint.GetComponent<TeleportPoint>().locked = false;
        }
    }
}
[Serializable] public class GoalEvent : UnityEvent<string, float, Color> { }