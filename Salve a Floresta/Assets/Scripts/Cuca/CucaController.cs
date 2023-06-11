using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CucaController : MonoBehaviour
{

    [Header("Dependecies")]
    public CameraController cameraController; // Referência para o script CameraController
    public GameObject player;
    public AnimationController animationController;


    public float activationDistance = 5f; // Definir uma distância de ativação da câmera do chefe
    private bool bossActive = false;


    [Header("Cuca Settings")]
    public int maxHealth = 10; // Pontos de vida máximos do chefe
    private int currentHealth; // Pontos de vida atuais do chefe
    private bool isDead = false; // Flag para verificar se o chefe foi derrotado

    [Header("Moviment Settings")]
    public float speed = 5f; // Velocidade de movimento do chefe
    public float distance = 10f; // Distância total que o chefe vai percorrer
    public Transform target; // Transform do jogador que o chefe vai seguir

    private bool isFacingRight = false; // Flag para verificar a direção atual do chefe


    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
         // Verificar a distância entre o jogador e o chefe
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= activationDistance)
        {
            cameraController.ActivateBossCamera();
            Debug.Log("Boss ativado");
            bossActive = true;
        }

        if (!isDead && bossActive)
        {
            MoveBoss();
            //BossAttack();
        }
    }

    // Função chamada quando o chefe é atingido
    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        // Verificar se o chefe foi derrotado
        if (currentHealth <= 0)
        {
            isDead = true;
            BossDefeated();
        }
    }

    // Função chamada quando o chefe é derrotado
    private void BossDefeated()
    {
        // Executar ações quando o chefe é derrotado, como tocar uma animação, desativar colisores, etc.

        // Exemplo: Destruir o objeto do chefe após 2 segundos
        Destroy(gameObject, 2f);
    }

    // Função para controlar os movimentos do chefe
    private void MoveBoss()
    {
        // Implemente o movimento do chefe de acordo com a lógica do seu jogo
        // Por exemplo, você pode usar transform.Translate para mover o chefe
        // ou controlar a posição do chefe usando uma curva ou um padrão predefinido
        // Lembre-se de levar em consideração a velocidade, a direção e a duração dos movimentos

        // Exemplo: Movimento horizontal simples

        // Verifica a direção do jogador
        Vector3 directionToPlayer = target.position - transform.position;
        float playerDirection = Mathf.Sign(directionToPlayer.x);

        Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if(playerDirection == 1 && isFacingRight == false)
        {
            Flip();
        }
        else if(playerDirection == -1 && isFacingRight == true)
        {
            Flip();
        }
        
        animationController.PlayAnimation("Walk");
    }

    // Função para controlar os ataques do chefe
    /*
    private void BossAttack()
    {
        // Implemente a lógica dos ataques do chefe
        // Você pode instanciar objetos de ataque, ativar animações de ataque,
        // controlar temporizadores para a frequência dos ataques, etc.

        // Exemplo: Instanciar um objeto de ataque (projétil) a cada 2 segundos
        if (Time.time % 2 == 0)
        {
            Instantiate(attackPrefab, transform.position, Quaternion.identity);
        }
    }
    */

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }
    
}
