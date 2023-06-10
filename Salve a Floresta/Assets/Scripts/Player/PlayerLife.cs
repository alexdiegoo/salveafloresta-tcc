using System.Collections;
using System.Collections.Generic;
using DragonBones;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private GameController gameController;
    public bool alive = true;
    public float immunityDuration = 5f; // Duração da imunidade em segundos
    public bool isImmune = false; // Verifica se o jogador está imune

    private int currentSceneIndex;

    private Material blinkMaterial;
    void Start()
    {
        gameController = GameController.gameController;
        blinkMaterial = GetComponentInChildren<MeshRenderer>().material;
    }

    void Update()
    {
        
    }

    public void LoseLife()
    {
        if(alive && !isImmune)
        {
            if (gameController.lives > 1 )
            {
                StartCoroutine(ApplyImmunity());
                gameController.SetLives(-1);
            }
            else if (gameController.lives == 1)
            {
                alive = false;
                gameController.SetLives(-1);


                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                gameObject.GetComponent<Player>().enabled = false;
                gameObject.GetComponent<AnimationController>().PlayAnimation("Death");
                
                currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                PlayerPrefs.SetInt("PreviousSceneIndex", currentSceneIndex);

                Invoke("LoadGameOverScene", 0.5f);
            }
        }
        

        Debug.Log(gameController.lives);
    }

    private IEnumerator ApplyImmunity()
    {
        isImmune = true;

        // Aqui você pode adicionar efeitos visuais ou lógica de imunidade,
        // como alterar a cor do jogador ou mostrar um escudo protetor.
        blinkMaterial.SetColor("_BlinkColor", Color.red);
        yield return new WaitForSeconds(immunityDuration/10f);
        blinkMaterial.SetColor("_BlinkColor", Color.white);
        yield return new WaitForSeconds(immunityDuration/10f);
        blinkMaterial.SetColor("_BlinkColor", Color.red);
        yield return new WaitForSeconds(immunityDuration/10f);
        blinkMaterial.SetColor("_BlinkColor", Color.white);
        yield return new WaitForSeconds(immunityDuration/10f);
        blinkMaterial.SetColor("_BlinkColor", Color.red);
        yield return new WaitForSeconds(immunityDuration/10f);
        blinkMaterial.SetColor("_BlinkColor", Color.white);
        yield return new WaitForSeconds(immunityDuration/10f);
        blinkMaterial.SetColor("_BlinkColor", Color.red);
        yield return new WaitForSeconds(immunityDuration/10f);
        blinkMaterial.SetColor("_BlinkColor", Color.white);
        yield return new WaitForSeconds(immunityDuration/10f);
        blinkMaterial.SetColor("_BlinkColor", Color.red);
        yield return new WaitForSeconds(immunityDuration/10f);
        blinkMaterial.SetColor("_BlinkColor", Color.white);
        yield return new WaitForSeconds(immunityDuration/10f);
        //yield return new WaitForSeconds(immunityDuration);

        
        isImmune = false;

        // Aqui você pode reverter os efeitos visuais ou lógica de imunidade.

        yield return null;
    }

    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOverMenu");
    }
}
