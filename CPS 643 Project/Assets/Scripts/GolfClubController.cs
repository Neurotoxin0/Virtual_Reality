using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]

public class GolfClubController : MonoBehaviour
{
    [Header("Events")]
    public StrikeEvent onStrike;
    
    private int strikeNum;
    private bool lock1;

    void Start()
    {
        strikeNum = 0;
    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("GolfClubController OnCollisionEnter" + collision.collider.name);
        if (collision.gameObject.tag == "Golf Ball" && !lock1)
        {
            lock1 = true;   // prevent multiple adding to strikeNum
            strikeNum++;
            onStrike.Invoke(collision.gameObject.name, strikeNum);
        }
    }

    private void OnCollisionExit(Collision collision) { lock1 = false; }


    public void InvokeonStrikeEvent() { onStrike.Invoke("Manually", strikeNum); }
}
[Serializable] public class StrikeEvent : UnityEvent<string, int> { }