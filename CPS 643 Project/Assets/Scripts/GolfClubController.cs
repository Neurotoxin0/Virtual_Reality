using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]

public class GolfClubController : MonoBehaviour
{
    [Header("Events")]
    public StrikeEvent onStrike;
    
    private int strikeNum;
    private bool isStriked;

    void Start()
    {
        strikeNum = 0;
        isStriked = false;
    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("GolfClubController OnCollisionEnter" + collision.collider.name);
        if (collision.gameObject.tag == "Golfball" && !isStriked)  // prevent multiple adding to strikeNum in a short time
        {
            strikeNum++;
            isStriked = true;
            onStrike.Invoke(collision.gameObject.name, strikeNum);
        }
    }

    private void OnCollisionExit(Collision collision) 
    {
        Invoke("UnlockStrike", 2f);
    }

    public void InvokeonStrikeEvent() { onStrike.Invoke("Manually", strikeNum); }

    private void UnlockStrike() { isStriked = false; }
}
[Serializable] public class StrikeEvent : UnityEvent<string, int> { }