using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

// on PlayerFramework; Reserved

public class SceneController : MonoBehaviour
{
    public void MoveToScene(int index)
    {
        Debug.Log("move to scene: " + name);

        // fade out
        SteamVR_Fade.View(Color.black, 0.5f);

        SceneManager.LoadScene(index);

        //TODO: kill old scene

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
