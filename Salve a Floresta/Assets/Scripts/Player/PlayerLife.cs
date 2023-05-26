using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private GameController gameController;
    public bool alive = true;
    void Start()
    {
        gameController = GameController.gameController;
    }

    void Update()
    {
        
    }

    public void LoseLife()
    {
        if(alive && gameController.lives > 1)
        {
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

            Debug.Log("Game Over!");
            //Invoke("LoadScene", 1f);
        }

        Debug.Log(gameController.lives);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Recarregar cena");
    }
}
