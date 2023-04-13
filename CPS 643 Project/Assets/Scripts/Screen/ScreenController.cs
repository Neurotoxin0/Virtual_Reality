using TMPro;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Canvas))]

// on Canvas.Screen

/*
 * Steam VR Source Cod was edited
 * Search for keyword "Alter" for the changes
 * Involved files:TeleportPoint.cs
 */

public class ScreenController : MonoBehaviour
{
    private GameObject strikeCnt, golfBallCnt, timer, message;

    void Start()
    {
        strikeCnt = GameObject.Find("Strike Count Text").gameObject;
        golfBallCnt = GameObject.Find("Golf Ball Count Text").gameObject;
        timer = GameObject.Find("Timer Text").gameObject;
        message = GameObject.Find("Message Text").gameObject;
        //Debug.Log("CanvasController Start: " + strikeCnt + ", " + golfBallCnt + ", " + timer + ", " + message);
        message.SetActive(false);

        GameObject.Find("Golfclub").gameObject.GetComponent<GolfClubController>().InvokeonStrikeEvent();
        GameObject.Find("Player").gameObject.GetComponent<PlayerController>().InvokeonSpawnGolfBall();
    }

    void Update()
    {

    }


    public void UpdateStrikeCnt(Hand _, int cnt)
    {
        //Debug.Log(debug_text + cnt);
        strikeCnt.GetComponent<TextMeshProUGUI>().text = "Number of Strike: " + cnt.ToString();
    }

    public void UpdateGolfBallCnt(Hand _, int cnt)
    {
        //Debug.Log(debug_text + cnt); 
        golfBallCnt.GetComponent<TextMeshProUGUI>().text = "Golf Ball Left: " + cnt.ToString();
    }

    public void UpdateTimer(int mm, int ss)
    { 
        string time = mm.ToString("00") + ":" + ss.ToString("00");
        timer.GetComponent<TextMeshProUGUI>().text = "Timer: " + time;
    }

    public void ShowMsg(string msg, float time, Color color)
    {
        //Debug.Log(msg + time);
        TextMeshProUGUI text = message.GetComponent<TextMeshProUGUI>();
        text.text = msg;
        text.color = color;
        message.SetActive(true); 
        if (time > 0) Invoke("HideMsg", time); // Hide the Message after specified time second
    }

    private void HideMsg() { message.SetActive(false); }
}
