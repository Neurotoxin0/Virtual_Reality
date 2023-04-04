using TMPro;
using UnityEngine;

// on Scrreen.Panel

public class CanvasController : MonoBehaviour
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

        GameObject.Find("Golf Club").gameObject.GetComponent<GolfClubController>().InvokeonStrikeEvent();
        GameObject.Find("Player").gameObject.GetComponent<PlayerController>().InvokeonSpawnGolfBall();
    }

    void Update()
    {
        
    }


    public void UpdateStrikeCnt(string debug_text, int cnt)
    {
        //Debug.Log(debug_text + cnt);
        strikeCnt.GetComponent<TextMeshProUGUI>().text = "Number of Strike: " + cnt.ToString();
    }

    public void UpdateGolfBallCnt(string debug_text, int cnt)
    {
        //Debug.Log(debug_text + cnt); 
        golfBallCnt.GetComponent<TextMeshProUGUI>().text = "Golf Ball Left: " + cnt.ToString();
    }

    public void UpdateTimer(int mm, int ss)
    { 
        string time = mm.ToString("00") + ":" + ss.ToString("00");
        timer.GetComponent<TextMeshProUGUI>().text = "Timer: " + time;
    }

    public void ShowMsg(string debug_text, string msg)
    {
        message.GetComponent<TextMeshProUGUI>().text = msg;
        message.SetActive(true);
    }
}
