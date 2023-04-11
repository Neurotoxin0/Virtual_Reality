using System;
using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

// Reserved

public class SceneController : MonoBehaviour
{
    void Start()
    {
        
    }

    private void Update()
    {
        
    }


    public void MoveToScene(string name)
    {
        Debug.Log("move to scene: " + name);

        // fade out
        SteamVR_Fade.View(Color.black, 0.5f);

        if (name == "") // reload current scene
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else // load another scene
        {
            SceneManager.LoadScene(name);
        }

        

        // fade in
        SteamVR_Fade.View(Color.clear, 0.5f);
    }
}
