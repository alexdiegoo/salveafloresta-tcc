using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string nextScene;

    public Image black;
    public Animator anim;

    public void Play()
    {
        StartCoroutine(Fading());
    }
    
    public void Quit()
    {
        Debug.Log("Saiu do jogo");
        Application.Quit();
    }

    IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a==1);
        SceneManager.LoadScene(nextScene);
    }
}
