using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManagement : MonoBehaviour
{
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == PlayerPrefs.GetInt("CompletedLevel"))
        {
            PlayerPrefs.SetInt("curupiraSkill", 1);
            PlayerPrefs.SetInt("iaraSkill", 1);
            PlayerPrefs.SetInt("saciSkill", 1);
            PlayerPrefs.Save();
        }
    }
}
