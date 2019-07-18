using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager {
    private static LevelManager instance;
    public static LevelManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new LevelManager();
            }
            return instance;
        }
    }

    /// <summary>
    /// Use it when the player die.
    /// The player will be respwned in checkpoint.
    /// </summary>
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Use it when the restart button is pressed.
    /// The player will be respawned at the start of current level.
    /// </summary>
    public void RestartLevel()
    {
        CheckPoint.CleanCheckPoint();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Use it when the level finished.
    /// The player will go to another level.
    /// </summary>
    /// <param name="levelName"></param>
    public void GoToLevel(string levelName)
    {
        CheckPoint.CleanCheckPoint();
        SceneManager.LoadScene(levelName);
    }
}
