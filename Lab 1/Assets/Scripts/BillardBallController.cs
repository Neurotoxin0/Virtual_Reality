using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillardBallController : MonoBehaviour
{
    private bool alive;

    void Start()
    {
        alive = true;
    }

    void Update()
    {
        if (transform.position.y <= 0 && alive)
        {
            GameObject.Find("Player").SendMessage("SetBilliardCount"); 
            alive = false;
            Destroy(gameObject, 1); // delay 1 sec
        }
    }
}
