using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CucaController : MonoBehaviour
{

    [Header("Dependecies")]
    public CameraController cameraController; // Referência para o script CameraController
    public GameObject player;
    public AnimationController animationController;


    public float activationDistance = 5f; // Definir uma distância de ativação da câmera da cuca
    private bool bossActive = false;


    [Header("Cuca Settings")]
    public int maxHealth = 10; // Pontos de vida máximos da cuca
    private int currentHealth; // Pontos de vida atuais da cuca
    private bool isDead = false; // Flag para verificar se a cuca foi derrotado
    public float attackInterval = 5f; // Intervalo de tempo entre os ataques
    private float nextAttackTime = 0f; // Tempo para o próximo ataque

    [Header("Moviment Settings")]
    public float speed = 5f; // Velocidade de movimento da cuca
    public float distance = 10f; // Distância total que a cuca vai percorrer
    public Transform target; // Transform do jogador que o chefe vai seguir
    private bool isFacingRight = false; // Flag para verificar a direção atual da cuca
    private bool isMoving = true; // Flag para controlar o estado de movimentação
    private bool isPausing = false; // Flag para controlar o estado de pausa
    public float pauseDuration = 2f; // Duração da pausa em segundos
    private float pauseEndTime = 0f; // Tempo de término da pausa

    [Header("Probability Attack")]
    public float meleeAttackProbability = 0.4f; // Probabilidade de ataque próximo
    public float magicBallAttackProbability = 0.4f; // Probabilidade de ataque com as esferas
    public float magicPortionAttackProbability = 0.2f; // Probabilidade de ataque porção magica

    [Header("MeleeAtack Settings")]
     // Configurações do ataque corpo a corpo
    public float attackRange = 1f;
    public float attackCooldown = 2f;
    private bool canAttack = true;
    private bool isAttacking  = false;

    

    [Header("Projectile Settings")]
    public GameObject projectilePrefab; // Prefab da esfera
    public float projectileSpeed = 10f; // Velocidade das esferas
    public Transform[] projectileSpawnPoints; // Pontos de origem das esferas


    [Header("MagicPortion Settings")]
    public GameObject magicPortionPrefab;
    public float portionSpeed = 5f;
    public Transform magicPortionPoint;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
         // Verificar a distância entre o jogador e o chefe
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= activationDistance && !bossActive)
        {
            cameraController.ActivateBossCamera();
            Debug.Log("Boss ativado");
            bossActive = true;
        }

        if (!isDead && bossActive)
        {
            if (!isAttacking)
            {
                if (isMoving)
                {
                    MoveBoss();
                }
                else if (isPausing)
                {
                    if (Time.time >= pauseEndTime)
                    {
                        isPausing = false;
                        isMoving = true;
                    }
                }
            }

            if (!isAttacking && Time.time >= nextAttackTime)
            {
                // Seleciona aleatoriamente qual ataque será executado com base nas probabilidades
                float randomValue = Random.value;

                if (randomValue < meleeAttackProbability)
                {
                    // Ataque próximo
                    //MeleeAttack();
                }
                else if (randomValue < meleeAttackProbability + magicBallAttackProbability)
                {
                    // Ataque com as esferas
                    isMoving = false; // Pausa a movimentação
                    isPausing = true;
                    pauseEndTime = Time.time + pauseDuration;
                    AttackMagicBall();
                }
                else
                {
                    // Ataque mágico
                    MagicPortion();
                }

                // Define o tempo para o próximo ataque
                nextAttackTime = Time.time + attackInterval;
            }
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

    /*private void MeleeAttack()
    {
        canAttack = false;
        isAttacking  = true;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == player)
            {
                // Reduz a vida do jogador
                PlayerLife playerController = player.GetComponent<PlayerLife>();
                if (playerController != null)
                {
                    playerController.LoseLife();
                }
                break;
            }
        }
       
        // Executa animação de ataque

        isAttacking  = false;

        // Aguarda o tempo de cooldown antes de permitir outro ataque
        Invoke(nameof(ResetAttackCooldown), attackCooldown);
    }

    private void ResetAttackCooldown()
    {
        canAttack = true;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }*/

    private void AttackMagicBall()
    {
        canAttack = false;
        isAttacking = true;

        animationController.PlayAnimation("Attack");

        // Disparar 3 esferas em direções diferentes
        for (int i = 0; i < projectileSpawnPoints.Length; i++)
        {
            Vector3 direction = (projectileSpawnPoints[i].position - transform.position).normalized;

            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoints[i].position, Quaternion.identity);
            Rigidbody2D projectileRigidbody = projectile.GetComponent<Rigidbody2D>();

            projectileRigidbody.velocity = direction * projectileSpeed;
        }

        isAttacking = false;
    }

    private void MagicPortion()
    {
        animationController.PlayAnimation("AttackBomb");
        canAttack = false;
        isAttacking = true;

        Vector3 direction = (magicPortionPoint.position - transform.position).normalized;

        GameObject magicPortion = Instantiate(magicPortionPrefab, magicPortionPoint.position, Quaternion.identity);
        Rigidbody2D magicPortionRb = magicPortion.GetComponent<Rigidbody2D>();

        magicPortionRb.velocity = direction * portionSpeed;

        isAttacking = false;
    }

}
