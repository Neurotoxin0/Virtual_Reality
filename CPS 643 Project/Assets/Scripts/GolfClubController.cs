using System;
using Valve.VR.InteractionSystem;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]

// on Golf Club

public class GolfClubController : LaserController   // debug purpose: to find where GolfClub.Aim point is pointing to
{
    [Header("Events")]
    public StrikeEvent onStrike;    // related: Screen.ScreenController.UpdateStrikeCnt; Utility.HapticCpntroller.ShortPulse;

    //private GameObject refObj;   // aim point as the ref
    private int strikeNum;
    private bool isStriked;

    void Start()
    {
        //refObj = gameObject.transform.GetChild(1).gameObject;
        //InitLaser();
        //laser.enabled = true;
        
        strikeNum = 0;
        isStriked = false;
    }

    void Update()
    {  
        //UpdateLaser(refObj);   
        //laser.SetPosition(1, refObj.transform.position + refObj.transform.forward);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("GolfClubController OnCollisionEnter" + collision.collider.name);
        if (collision.gameObject.tag == "Golfball" && !isStriked)  // prevent multiple adding to strikeNum in a short time
        {
            strikeNum++;
            isStriked = true;
            onStrike.Invoke(Player.instance.rightHand, strikeNum);
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