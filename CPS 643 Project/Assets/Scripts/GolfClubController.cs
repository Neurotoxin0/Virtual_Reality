using System;
using Valve.VR.InteractionSystem;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]

// on Golf Club

public class GolfClubController : MonoBehaviour
{
    [Header("Events")]
    public StrikeEvent onStrike;    // related: Screen.ScreenController.UpdateStrikeCnt; Utility.HapticCpntroller.ShortPulse;

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
            onStrike.Invoke(Player.instance.rightHand, strikeNum);

            // add extra force to golfball
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 1000f);     //TODO: test
        }
    }

    private void OnCollisionExit(Collision collision) 
    {
        Invoke("UnlockStrike", 2f);
    }

    public void InvokeonStrikeEvent() { onStrike.Invoke(null, strikeNum); }

    private void UnlockStrike() { isStriked = false; }
}
[Serializable] public class StrikeEvent : UnityEvent<Hand, int> { }