using UnityEngine;
using UnityEngine.SceneManagement;

// on PlayerFramework

public class DifficultyController : MonoBehaviour
{
    private const int MaxDifficulty = 3;

    void Start()
    {
        SetDifficulty(SceneManager.GetActiveScene().buildIndex, 1);
    }
    
    public void SetDifficulty(int sentByLevel, int difficulty)
    {
        if (difficulty <= 0 || difficulty > 5 || SceneManager.GetActiveScene().buildIndex != sentByLevel)
        {
            Debug.Log("Invalid Difficulty: " + difficulty);
            return;
        }

        //Debug.Log("SetDifficulty: " + difficulty);
        DifficultyAction(MaxDifficulty, false);
        DifficultyAction(difficulty, true);
    }

    private void DifficultyAction(int level, bool newState)
    {
        for (int i = 1; i <= level; i++)
        {
            GameObject[] gameobjects = GameObject.FindGameObjectsWithTag("Difficulty " + i);

            foreach (GameObject gameobject in gameobjects)
            {
                //Debug.Log("Difficulty " + i + ": set " + gameobject.name + " to " + newState);
                
                //gameobject.SetActive(newState); -> cannot just disable the gameobject -> cannot be searched later -> will not be able to enable it again
                gameobject.GetComponent<MeshRenderer>().enabled = newState;
                gameobject.GetComponent<Collider>().enabled = newState;
            }
        }
    }
}
