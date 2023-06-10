using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompletedLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            WinGame();
        }
    }

    public void WinGame()
    {
        if(SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("CompletedLevel"))
        {

            PlayerPrefs.SetInt("CompletedLevel", SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.Save();
        }

        Debug.Log("Win Game");
        SceneManager.LoadScene("WinScreen");
    }
}
