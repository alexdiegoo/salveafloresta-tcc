using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public float timeDestroyShot = 3f; // Tempo para remover o tiro de cena
    
    private bool isPlayerHit = false; // Variável para controlar se o jogador foi atingido
   
    void Start()
    {
        Destroy(gameObject, timeDestroyShot);
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
            isPlayerHit = true; // Marca o jogador como atingido para evitar repetições

            collision.gameObject.GetComponent<PlayerLife>().LoseLife();

            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Zera a velocidade da bala
            Destroy(gameObject); // Destroi a bala
        }

        if(!(collision.gameObject.tag == "Player"))
        {
            Destroy(gameObject); // Destroi a bala
        }
        
    }
}