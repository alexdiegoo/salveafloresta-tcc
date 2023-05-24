using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShotController : MonoBehaviour
{
    [SerializeField] float timeDestroyShot = 3f; // Tempo para remover o tiro de cena
    
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
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            collision.gameObject.GetComponent<Player>().enabled = false;
            
            isPlayerHit = true; // Marca o jogador como atingido para evitar repetições
            Invoke("LoadScene", 1f);

            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Zera a velocidade da bala
        }

        if(!(collision.gameObject.tag == "Player"))
        {
            Destroy(gameObject); // Destroi a bala
        }
        
    }

    void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Recarregar cena");
        Destroy(gameObject); // Destroi a bala
    }
}