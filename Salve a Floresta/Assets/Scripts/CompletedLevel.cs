using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompletedLevel : MonoBehaviour
{
    public void winGame()
    {
        if(SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("CompletedLevel"))
        {

            Debug.Log("Win Game");
            PlayerPrefs.SetInt("CompletedLevel", SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.Save();
        }
    }
}
