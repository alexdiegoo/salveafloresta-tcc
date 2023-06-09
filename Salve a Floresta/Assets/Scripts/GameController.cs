using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController gameController;

    public int lives;
    public int energyCrystals = 0;
    public int maxEnergyCrystals = 10;

    public Image[] heartSprites;
    public Image energyBarSprite;


    public GameObject dialoguePanel;
    public Text dialogueText;

    public Text nameNpcText;
    public Image imageNpc;

    private bool firstFrame = false;
    
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

    private void Start()
    {
        lives = 3;
        RefreshScreen();
        
        firstFrame = true;
    }

    private void Update()
    {
        if(firstFrame)
        {
            if(dialoguePanel != null)
            {
                dialoguePanel.SetActive(false);
            }

            firstFrame = false;
        }
    }

    public void SetLives(int life)
    {
        lives += life;
        if(lives >= 0)
        {
            RefreshScreen();
        }
    }

    public void SetEnergyCrystals(int energyCrystal)
    {   
        energyCrystals += energyCrystal;
        RefreshScreen();
    }

    public void RefreshScreen()
    {
        for (int i = 0; i < heartSprites.Length; i++)
        {
            if (i < lives)
            {
                // Define o fillAmount como 1 para corações cheios
                heartSprites[i].fillAmount = 1f;
            }
            else
            {
                // Define o fillAmount como 0 para corações vazios
                heartSprites[i].fillAmount = 0f;
            }
        }
        
        float fillAmount = (float)energyCrystals / maxEnergyCrystals;
        energyBarSprite.fillAmount = fillAmount;
    }

    public void ResetPlayerValues()
    {
        gameController.SetLives(3 - gameController.lives);
        gameController.SetEnergyCrystals(-gameController.energyCrystals);
        gameController.RefreshScreen();
    }
}
