using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Button[] buttons;

    void Update()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            if(i + 5 > PlayerPrefs.GetInt("CompletedLevel"))
            {
                buttons[i].interactable = false;
            }
        }
    }
}
