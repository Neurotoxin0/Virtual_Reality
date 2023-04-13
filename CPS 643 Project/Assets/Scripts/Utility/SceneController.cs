using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;

// on PlayerFramework

/*
 * SteamVR Source Code was edited
 * Search the keywork "Alter" for the changes
 * Involved files: TeleportPoint.cs
 */

public class SceneController : MonoBehaviour
{
    public Player player;
    
    private void Start()
    {
        SteamVR_Fade.View(Color.clear, 0.5f);
    }

    public IEnumerator MoveToScene(int index = -1)
    {
        Debug.Log("move to scene: " + index);

        // fade out
        SteamVR_Fade.View(Color.black, 0.5f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
        while (!asyncLoad.isDone)  yield return null;

        // fade in
        SteamVR_Fade.View(Color.clear, 0.5f);
    }
    public IEnumerator MoveToScene(string sceneName)
    {
        Debug.Log("move to scene: " + sceneName);

        // fade out
        SteamVR_Fade.View(Color.black, 0.5f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone) yield return null;

        // fade in
        SteamVR_Fade.View(Color.clear, 0.5f);
    }


    public void NextLevel()
    {
        // fade out
        SteamVR_Fade.View(Color.black, 0.5f);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        //TODO: kill old scene

        // fade in
        SteamVR_Fade.View(Color.clear, 0.5f);
    }

    public void ReloadCurrentScene()
    {
        // fade out
        SteamVR_Fade.View(Color.black, 0.5f); 
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        //TODO: kill old scene

        // fade in
        SteamVR_Fade.View(Color.clear, 0.5f);
    }
}
