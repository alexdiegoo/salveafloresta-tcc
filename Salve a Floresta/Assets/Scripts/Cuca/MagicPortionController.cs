using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPortionController : MonoBehaviour
{
    private Player playerController;
    private bool isPlayerHit = false; // Variável para controlar se o jogador foi atingido
    public AudioSource attack1_1AudioSource = null;

    public float timeDestroy = 2f;

    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

     private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPlayerHit)
        {
            // Verifica se o jogador já foi atingido para evitar repetições
            return;
        } 
            
        if(collision.gameObject.tag == "Player")
        {    
            attack1_1AudioSource.Play();

            isPlayerHit = true; // Marca o jogador como atingido para evitar repetições

            collision.gameObject.GetComponent<PlayerLife>().LoseLife();

            playerController = collision.gameObject.GetComponent<Player>();
            playerController.KbCount = playerController.KbTime;

           if(playerController.transform.position.x <= transform.position.x)
           {
                playerController.isKnockRight = true;
           }
           else if (playerController.transform.position.x > transform.position.x)
           {
                playerController.isKnockRight = false;
           }

            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Zera a velocidade da bala
            Destroy(gameObject, 0.1f); // Destroi a bala
        }

        if(!(collision.gameObject.tag == "Player"))
        {
            Destroy(gameObject); // Destroi a bala
        }
    }
}
