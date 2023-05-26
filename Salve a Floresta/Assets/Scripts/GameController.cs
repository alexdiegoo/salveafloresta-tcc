using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController gameController;

    public int lives = 3;

    void Awake()
    {
        if(gameController == null)
        {
            gameController = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(gameController != this)
        {
            Destroy(gameObject);
        }

        RefreshScreen();
    }

    public void SetLives(int life)
    {
        lives += life;
        if(lives >= 0)
        {
            RefreshScreen();
        }
    }

    public void RefreshScreen()
    {
    }
}
