using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    private int previousSceneIndex;

    private void Start()
    {
        previousSceneIndex = PlayerPrefs.GetInt("PreviousSceneIndex", 0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(previousSceneIndex);
    }
}
