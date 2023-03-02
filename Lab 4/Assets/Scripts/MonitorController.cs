using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonitorController : MonoBehaviour
{
    private TextMeshProUGUI statusLeft, statusRight;

    void Start()
    {
        statusLeft = transform.Find("TextLeft").gameObject.GetComponent<TextMeshProUGUI>();
        statusRight = transform.Find("TextRight").gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        UpdateCanvas();
    }

    private void UpdateCanvas()
    {
        statusLeft.text = "111";
        statusRight.text = "222";
    }
}
