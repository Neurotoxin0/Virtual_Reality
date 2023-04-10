using UnityEngine;
using UnityEngine.SceneManagement;

// Reserved

public class SceneController : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void MoveToAnotherScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
