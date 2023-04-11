using UnityEngine;
using UnityEngine.Events;
using System;

// on  Canvas.Screen

public class Timer : MonoBehaviour
{
    [Header("Events")]
    public TimeEvent onTimeChange;
    public UnityEvent onTimerEnd;

    private double tmp_s;
    private int timer_m, timer_s;
    private bool active, countdown, pause;

    void Start()
    {
        active = false;
        pause = false;

        Init();
    }

    void FixedUpdate()
    {
        if (active && !pause) 
        {
            tmp_s = countdown ? tmp_s - Time.deltaTime : tmp_s + Time.deltaTime;
            SetTimerText();
        }
    }


    public void Init()
    {
        countdown = false;
        timer_m = 0;
        tmp_s = 0;
        active = true;
    }
    public void Init(int time)  // countdown
    {
        countdown = true; 
        timer_m = time / 60;
        tmp_s = time % 60;
        active = true;
    }


    private void SetTimerText() 
    { 
        if (countdown)
        {
            if (tmp_s <= 0)
            {
                if (timer_m <= 0) // end
                {
                    timer_m = 0;
                    tmp_s = 0;
                    active = false;
                    onTimerEnd.Invoke();
                    return;
                }
                else
                {
                    tmp_s = 60;
                    timer_m--;
                }
            }
            
        }
        else
        {
            if (tmp_s >= 60)
            {
                tmp_s = 0;
                timer_m++;
            }
        }
        
        timer_s = (int)tmp_s;
        //Debug.Log(timer_m + " : " + timer_s);
        onTimeChange.Invoke(timer_m, timer_s);
    }

    public void Pause() { pause = !pause; }
}
[Serializable] public class TimeEvent : UnityEvent<int, int> { }