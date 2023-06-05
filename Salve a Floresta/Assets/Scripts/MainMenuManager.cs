using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string nextScene;
    public void Play()
    {
        SceneManager.LoadScene(nextScene);
    }
    
    public void Quit()
    {
        Debug.Log("Saiu do jogo");
        Application.Quit();
    }
}
