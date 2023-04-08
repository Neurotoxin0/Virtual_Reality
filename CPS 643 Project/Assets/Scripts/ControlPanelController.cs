using System.Collections;
using UnityEngine;

public class ControlPanelController : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangePanelActive(string debugInfo, bool state)
    {
        gameObject.SetActive(state);
    }
}