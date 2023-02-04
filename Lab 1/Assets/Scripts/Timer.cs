using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI TimerText;

    private double timer_ss;
    private int timer_m, timer_s;
    private bool timer_end;

    void Start()
    {
        timer_m = 0;
        timer_ss = 0.0;
        timer_end = false;
    }

    void Update()
    {
        if (!timer_end) 
        {
            timer_ss += Time.deltaTime;
            SetTimerText();
        }
    }

    void SetTimerText() 
    { 
        if (timer_ss >= 60) 
        {
            timer_ss = 0;
            timer_m ++;
        }

        timer_s = (int)timer_ss;
        TimerText.text = "Timer " + timer_m + " : " + timer_s;
    }

    void SetTimerState() { timer_end = true; }
}
