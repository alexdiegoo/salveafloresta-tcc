using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [Header("Pause Menu")]
    public GameObject pauseMenuPanel;
    private Player player = null;
    
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

        gameController.RefreshScreen();
    }

    private void Start()
    {
        lives = 3;
        energyCrystals = 0;
        gameController.RefreshScreen();
        
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

            if(pauseMenuPanel != null)
            {
                pauseMenuPanel.SetActive(false);
            }

            player = GameObject.FindObjectOfType<Player>();
            ResetPlayerValues();
            firstFrame = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuPanel.activeSelf)
            {
                pauseMenuPanel.SetActive(false);
                Time.timeScale = 1;
                if (player != null)
                {
                    player.enabled = true;
                }
            }
            else
            {
                pauseMenuPanel.SetActive(true);
                Time.timeScale = 0;
                if (player != null)
                {
                    player.enabled = false;
                }
            }
        }
    }

    public void SetLives(int life)
    {
        lives += life;
        if(lives >= 0)
        {
            gameController.RefreshScreen();
        }
    }

    public void SetEnergyCrystals(int energyCrystal)
    {   
        energyCrystals += energyCrystal;
        gameController.RefreshScreen();
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

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
        if (player != null)
        {
            player.enabled = true;
        }
    }

    public void QuitGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelMenu");
    }
}
