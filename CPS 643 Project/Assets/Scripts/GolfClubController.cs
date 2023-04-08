using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]

public class GolfClubController : TimeOut
{
    [Header("Events")]
    public StrikeEvent onStrike;
    
    private int strikeNum;

    void Start()
    {
        strikeNum = 0;
    }

    void Update()
    {
        if (timeoutEnabled && timeout) ResetTimeOut();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("GolfClubController OnCollisionEnter" + collision.collider.name);
        if (collision.gameObject.tag == "Golfball" && !timeoutEnabled)
        {
            // prevent multiple adding to strikeNum
            Debug.Log("timeoutenabled: " + timeoutEnabled);
            SetTimeOut(5);

            strikeNum++;
            onStrike.Invoke(collision.gameObject.name, strikeNum);
        }
    }

    private void OnCollisionExit(Collision collision) { timeoutEnabled = false; }


    public void InvokeonStrikeEvent() { onStrike.Invoke("Manually", strikeNum); }
}
[Serializable] public class StrikeEvent : UnityEvent<string, int> { }