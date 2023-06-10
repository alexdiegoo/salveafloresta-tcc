using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    private int previousSceneIndex;
    public Image black;
    public Animator anim;

    private void Start()
    {
        previousSceneIndex = PlayerPrefs.GetInt("PreviousSceneIndex", 0);
    }

    public void RestartGame()
    {
        StartCoroutine(Fading());
    }

    IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a==1);
        SceneManager.LoadScene(previousSceneIndex);
    }
}
