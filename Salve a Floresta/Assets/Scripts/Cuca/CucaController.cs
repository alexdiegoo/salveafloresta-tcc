using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CucaController : MonoBehaviour
{

    [Header("Dependecies")]
    public CameraController cameraController; // Referência para o script CameraController
    public GameObject player;
    public AnimationController animationController;
    public CucaLife cucaLifeController;
    public GameObject blockA; // Barreira no combate com o boss
    public GameObject blockB;


    public float activationDistance = 5f; // Definir uma distância de ativação da câmera da cuca
    private bool bossActive = false;


    [Header("Cuca Settings")]
    public float attackInterval = 8f; // Intervalo de tempo entre os ataques
    private float nextAttackTime = 0f; // Tempo para o próximo ataque
    private bool isAttacking  = false; // Flag para saber se a cuca está atacando


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
    public float followAttackProbability = 0f; // Probabilidade de ataque próximo
    public float magicBallAttackProbability = 0.4f; // Probabilidade de ataque com as esferas
    public float magicPortionAttackProbability = 0.6f; // Probabilidade de ataque porção magica


    [Header("Projectile Settings")]
    public GameObject projectilePrefab; // Prefab da esfera
    public float projectileSpeed = 10f; // Velocidade das esferas
    public Transform[] projectileSpawnPoints; // Pontos de origem das esferas


    [Header("MagicPortion Settings")]
    public GameObject magicPortionPrefab;
    public float portionSpeed = 5f;
    public Transform magicPortionPoint;

    [Header("MagicFollowPlayer Settings")]
    public GameObject magicFollowPrefab;
    public Transform magicFollowPosition;

    private bool firstFrame = false;

    void Start()
    {
        cucaLifeController.currentHealth = cucaLifeController.maxHealth;
        firstFrame = true;
    }

    void Update()
    {
        // Verificar a distância entre o jogador e a cuca
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= activationDistance && !bossActive)
        {
            cameraController.ActivateBossCamera();
            Debug.Log("Boss ativado");
            blockA.GetComponent<BoxCollider2D>().enabled = true;
            blockB.GetComponent<BoxCollider2D>().enabled = true;
            bossActive = true;
        }

        if (!cucaLifeController.isDead && bossActive)
        {
            // Verificar se a vida do boss é menor ou igual a 5
            if (cucaLifeController.currentHealth <= 5)
            {
                attackInterval = 5f;
                followAttackProbability = 0.4f;
                magicBallAttackProbability = 0.4f;
                magicPortionAttackProbability = 0.2f;
            }


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

                if (randomValue < followAttackProbability)
                {
                    // Ataque esfera que segue o jogador
                    MagicFollowPlayer();
                }
                else if (randomValue < followAttackProbability + magicBallAttackProbability)
                {
                    // Ataque com as esferas
                    isMoving = false; // Pausa a movimentação
                    isPausing = true;
                    pauseEndTime = Time.time + pauseDuration;
                    AttackMagicBall();
                }
                else
                {
                    // Ataque porção mágica
                    MagicPortion();
                }

                // Define o tempo para o próximo ataque
                nextAttackTime = Time.time + attackInterval;
            }
        }
    }

    private void MoveBoss()
    {
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

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    private void AttackMagicBall()
    {
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
        isAttacking = true;

        Vector3 direction = (magicPortionPoint.position - transform.position).normalized;

        GameObject magicPortion = Instantiate(magicPortionPrefab, magicPortionPoint.position, Quaternion.identity);
        Rigidbody2D magicPortionRb = magicPortion.GetComponent<Rigidbody2D>();

        magicPortionRb.velocity = direction * portionSpeed;

        isAttacking = false;
    }

    private void MagicFollowPlayer()
    {
        isAttacking = true;

        GameObject magic = Instantiate(magicFollowPrefab, magicFollowPosition.position, Quaternion.identity);

        isAttacking = false;
    }

}
