using UnityEngine;

// on PlayerFramework

public class DifficultyController : MonoBehaviour
{
    private const int MaxDifficulty = 3;
    
    public void SetDifficulty(int difficulty)
    {
        if (difficulty < 0 || difficulty > 3)
        {
            Debug.Log("Invalid Difficulty: " + difficulty);
            return;
        }
        else if (difficulty == 0) DifficultyAction(MaxDifficulty, false);   // Reset difficulty
        else DifficultyAction(difficulty, true);
    }

    private void DifficultyAction(int level, bool newState)
    {
        for (int i = 1; i <= level; i++)
        {
            GameObject[] gameobjects = GameObject.FindGameObjectsWithTag("Difficulty" + i);

            foreach (GameObject gameobject in gameobjects)
            {
                gameobject.SetActive(newState);
            }
        }
    }
}
