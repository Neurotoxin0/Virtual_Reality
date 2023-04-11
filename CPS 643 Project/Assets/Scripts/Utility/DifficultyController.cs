using UnityEngine;
using UnityEngine.SceneManagement;

// on PlayerFramework

public class DifficultyController : MonoBehaviour
{
    private int level, difficulty;

    void Start()
    {
        level = SceneManager.GetActiveScene().buildIndex;
        difficulty = 0;
    }

    void Update()
    {
        switch (level)
        {
            case 1: Level1Difficulty(); break;
            case 2: Level2Difficulty(); break;
            default: break;     
        }
    }

    public void SetDifficulty(int value)
    {
        difficulty = value;
        Debug.Log("Difficulty set to " + difficulty);
    }

    private void Level1Difficulty()
    {

    }

    private void Level2Difficulty()
    {

    }


}
