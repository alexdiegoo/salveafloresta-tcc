using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] ManagerInput managerInput;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] AnimationController AnimationController;
    [SerializeField] Detection detection;
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private GameObject musicalParticlePrefab;

    [Header("Physics")]
    [SerializeField] public float speed;
    [SerializeField] float jumpForce;
    [SerializeField] bool isFacingRight = true;
    [SerializeField] float fallGravity;
    [SerializeField] float jumpGravity;
    private float gravityScale;
    private float inputForce;
    private float forceSum;

    [Header("Audio")] [SerializeField] private AudioCharacter audioPlayer = null;

    [Header("Dialogue Settings")]
    public bool inDialogue = false;
    


    [Header("Knockback Settings")]
    public float kbForce;
    public float KbCount;
    public float KbTime;
    public bool isKnockRight = false;

    [Header("SpecialAttack Settings")] 
    public float specialCount = 2f;
    private float nextSpecialTime;
    private bool isUsingSpecialAttack = false; // Variável para controlar se um ataque especial está sendo usado
    private bool isFirstSpecialAttack = true;
    
    // Poder do curupira
    public float fireForce = 20f;
    
    // Poder do saci
    public float dashDuration = 3f; // Duração do dash em segundos
    public float dashForce = 30f; // Força do dash
    public float dashCooldown = 5f; // Tempo de espera antes de poder usar o dash novamente
    private float dashEndTime; // Tempo de término do dash atual
    private bool isDashing; // Indica se o jogador está atualmente em um dash
    private float nextDashTime; // Tempo para poder usar o dash novamente
    
    // Poder da Iara
    public float paralyzeDistance;
    public float paralyzeDuration = 1f; // Duração da paralisação em segundos
    private float paralyzeEndTime = -1f; // Tempo de término da paralisação (-1 significa sem paralisação)
    private List<GameObject> paralyzedEnemies;


    private GameController gameController;
    private bool firstFrame = false;
    
    void Start()
    {
        AnimationController.PlayAnimation("Idle");

        managerInput.OnButtonEvent += ManagerInput_OnButtonEvent;
        gravityScale = rigidBody2D.gravityScale;
        gameController = GameController.gameController;
        
        nextSpecialTime = Time.time + specialCount;
        nextDashTime = nextSpecialTime; // Definir o nextDashTime como o tempo atual no início

        paralyzedEnemies = new List<GameObject>();

        firstFrame = true;
    }

    private void ManagerInput_OnButtonEvent()
    {
        if(detection.ground != null && Input.GetKeyDown(KeyCode.Space))
        {
            if(inDialogue) return;
            JumpPlayer();
            audioPlayer.PlayJump();
        }
        else if(Input.GetKeyDown(KeyCode.X) && !isUsingSpecialAttack)
        {
            
           if(isFirstSpecialAttack && gameController.energyCrystals >= 2 || (gameController.energyCrystals >= 2 && Time.time > nextSpecialTime))
           {
               isFirstSpecialAttack = false;
                isUsingSpecialAttack = true;
                SpecialFire();
                gameController.SetEnergyCrystals(-2);
                nextSpecialTime = Time.time + specialCount;    
                StartCoroutine(ResetSpecialAttack());
           }
        }
        else if (Input.GetKeyDown(KeyCode.Z) && !isUsingSpecialAttack)
        {
            if (isFirstSpecialAttack && gameController.energyCrystals >= 2 ||  (gameController.energyCrystals >= 2 && Time.time > nextDashTime))
            {
                isFirstSpecialAttack = false;
                isUsingSpecialAttack = true;
                StartDash();
                gameController.SetEnergyCrystals(-2);
                nextDashTime = Time.time + dashCooldown;
                StartCoroutine(ResetSpecialAttack());
            }
        }
        else if (Input.GetKeyDown(KeyCode.C) && !isUsingSpecialAttack)
        {
            if (isFirstSpecialAttack && gameController.energyCrystals >= 2 || (gameController.energyCrystals >= 2 && Time.time > nextSpecialTime))
            {
                isFirstSpecialAttack = false;
                isUsingSpecialAttack = true;
                ParalyzeEnemies();
                paralyzeEndTime = Time.time + paralyzeDuration;
                nextSpecialTime = Time.time + specialCount;
                Debug.Log("Time:");
                Debug.Log(Time.time);
                Debug.Log("Tempo final de paralização");
                Debug.Log(paralyzeEndTime);
                StartCoroutine(ResetSpecialAttack());
            }
        }
    }


    void Update()
    {

        if(firstFrame)
        {
            gameController.ResetPlayerValues();
            firstFrame = false;
        }

        CheckDirection();
        if (detection.ground != null && isDashing == false)
        {
            if (Mathf.Abs(managerInput.GetInputX()) > 0f)
            {
                AnimationController.PlayAnimation("Run"); // Toca a animação "Run" quando o jogador está se movendo no chão
            }
            else
            {
                AnimationController.PlayAnimation("Idle"); // Toca a animação "Idle" quando o jogador não está se movendo no chão
            }
        }
        else if(isDashing == false)
        {
            if (Mathf.Abs(managerInput.GetInputX()) > 0f)
            {
                AnimationController.PlayAnimation("Jump"); // Toca a animação "Jump" quando o jogador está se movendo no ar
            }
            else
            {
                AnimationController.PlayAnimation("Jump"); // Mantém a animação "Jump" quando o jogador não está se movendo no ar
            }
        }
    }

    
    void FixedUpdate()
    {
        KnockLogic();
        JumpBetterPlayer();
        
        if (isDashing && Time.time < dashEndTime)
        {
            Dash();
        }
        else
        {
            isDashing = false;
            tr.emitting = false;
        }
        
        
        if (paralyzeEndTime > 0 && Time.time > paralyzeEndTime)
        {
            // Encerrar a paralisação dos inimigos
            UnparalyzeEnemies();
            paralyzeEndTime = -1f;
        }

        if (detection.ground != null)
        {
            //Audio
            audioPlayer.PlaySteps(Mathf.Abs(inputForce));
        }
        
    }

    private void GetInputForce()
    {
        inputForce = managerInput.GetInputX() * speed * 0.3f;
    }

    void KnockLogic()
    {
       /* if(KbCount < 0)
        {
            Move();
        }
        else
        {
            if(isKnockRight == true)
            {
                rigidBody2D.velocity = new Vector2(-kbForce, rigidBody2D.velocity.y + 1f);
            }
            else
            {
                rigidBody2D.velocity = new Vector2(kbForce, rigidBody2D.velocity.y + 1f);
            }
        }

        KbCount -= Time.deltaTime;*/

       GetInputForce();

       if (KbCount > 0)
       {
           if(isKnockRight) 
               forceSum -= kbForce;
           else 
               forceSum += kbForce;
           
           KbCount -= Time.deltaTime;
       }
       
       Move();

    }

    private void Move()
    {
        forceSum += inputForce;
        forceSum = Mathf.Clamp(forceSum, -speed, speed);
        forceSum = Mathf.LerpUnclamped(forceSum, 0, Time.deltaTime * speed/2);
        if (Mathf.Abs(forceSum) <= 1.2f) forceSum = 0;
        rigidBody2D.velocity = new Vector2(forceSum, rigidBody2D.velocity.y);
    }

    private void CheckDirection()
    {
        if(isFacingRight && managerInput.GetInputX() < 0f)
        {
            Flip();
        }
        else if(!isFacingRight && managerInput.GetInputX() > 0f)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    private void JumpPlayer()
    {
        rigidBody2D.velocity = Vector2.up * jumpForce;
    }

    private void JumpBetterPlayer()
    {
        if(rigidBody2D.velocity.y < 0) {
            rigidBody2D.gravityScale = fallGravity;
        }
        else if(rigidBody2D.velocity.y > 0 && !managerInput.IsDown())
        {
            rigidBody2D.gravityScale = jumpGravity;
        }
        else 
        {
            rigidBody2D.gravityScale = gravityScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            rigidBody2D.velocity = Vector2.zero;
            rigidBody2D.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
            audioPlayer.PlayHit();

            if(collision.gameObject.GetComponent<EnemyHunterController>())
            {
                collision.gameObject.GetComponent<EnemyHunterController>().enabled = false;
            }
            else if(collision.gameObject.GetComponent<EnemyWoodCutterController>())
            {
                collision.gameObject.GetComponent<EnemyWoodCutterController>().enabled = false;
            }
            else if(collision.gameObject.GetComponent<EnemyGoldMinerController>())
            {
                collision.gameObject.GetComponent<EnemyGoldMinerController>().enabled = false;
            }

            collision.gameObject.GetComponent<AnimationController>().PlayAnimation("Death");
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            collision.gameObject.GetComponent<EnemyPatrol>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            Destroy(collision.gameObject, 1f);
        }

        if(collision.gameObject.CompareTag("Cuca"))
        {
            rigidBody2D.velocity = Vector2.zero;
            rigidBody2D.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
            audioPlayer.PlayHit();

            collision.gameObject.GetComponent<CucaLife>().TakeDamage(1);
        }

        if(collision.gameObject.tag == "EnergyCrystal")
        {
            audioPlayer.PlayCollectCrystal();
            if(gameController.energyCrystals < gameController.maxEnergyCrystals)
            {
                gameController.SetEnergyCrystals(1);
            }
          
            Destroy(collision.gameObject, 0.1f);
        }

        if(collision.gameObject.CompareTag("Hearth"))
        {
            if(gameController.lives < 3)
            {
                gameController.SetLives(1);
            }

            Destroy(collision.gameObject, 0.1f);
        }

        if (collision.CompareTag("DeathZone"))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            
            PlayerPrefs.SetInt("PreviousSceneIndex", currentSceneIndex);
            PlayerPrefs.Save();
            SceneManager.LoadScene("GameOverMenu");
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Impede que o jogador empurre o inimigo
            collision.rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        if(collision.gameObject.CompareTag("Cuca"))
        {
            collision.rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        if (collision.gameObject.CompareTag("Enemy") && isDashing)
        {
            if(collision.gameObject.GetComponent<EnemyHunterController>())
            {
                collision.gameObject.GetComponent<EnemyHunterController>().enabled = false;
            }
            else if(collision.gameObject.GetComponent<EnemyWoodCutterController>())
            {
                collision.gameObject.GetComponent<EnemyWoodCutterController>().enabled = false;
            }
            else if(collision.gameObject.GetComponent<EnemyGoldMinerController>())
            {
                collision.gameObject.GetComponent<EnemyGoldMinerController>().enabled = false;
            }

            collision.gameObject.GetComponent<AnimationController>().PlayAnimation("Death");
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            collision.gameObject.GetComponent<EnemyPatrol>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            Destroy(collision.gameObject, 1f);
        }
    }

    public void SpecialFire()
    {
        audioPlayer.PlaySkillCurupira();
        GameObject fire = Instantiate(firePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D fireRb = fire.GetComponent<Rigidbody2D>();
        fireRb.velocity = firePoint.right * fireForce;
    }
    
    private void StartDash()
    {
        isDashing = true;
        dashEndTime = Time.time + dashDuration;
        rigidBody2D.velocity = Vector2.zero; // Zerar a velocidade atual para evitar interferências no dash
        AnimationController.PlayAnimation("Dash");
        tr.emitting = true;
        audioPlayer.PlaySkillSaci();
        StartCoroutine(ApplyImmunity());
    }

    private void Dash()
    {
        float dashDirection = isFacingRight ? 1f : -1f;
        rigidBody2D.velocity = new Vector2(dashDirection * dashForce, rigidBody2D.velocity.y);
    }
    
    private IEnumerator ApplyImmunity()
    {
        GetComponent<PlayerLife>().isImmune = true;
        
        yield return new WaitForSeconds(dashDuration);
        
        GetComponent<PlayerLife>().isImmune = false;

        yield return null;
    }
    
    private IEnumerator ResetSpecialAttack()
    {
        yield return new WaitForSeconds(specialCount); // Aguardar o tempo de duração do ataque especial
        
        isUsingSpecialAttack = false; // Permitir o uso de outro ataque especial
        
        yield return null;
    }
    
    private void ParalyzeEnemies()
    {
        GameObject particle = Instantiate(musicalParticlePrefab, transform.position, transform.rotation);
        audioPlayer.PlaySkillIara();
        Destroy(particle, paralyzeDuration);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, paralyzeDistance, LayerMask.GetMask("Enemy"));

        foreach (Collider2D enemyCollider in enemies)
        {
            if (enemyCollider.gameObject.CompareTag("Enemy"))
            {
                if (enemyCollider.gameObject.GetComponent<EnemyHunterController>() != null)
                {
                    enemyCollider.gameObject.GetComponent<EnemyHunterController>().enabled = false;
                    enemyCollider.gameObject.GetComponent<EnemyPatrol>().enabled = false;
                }
                if (enemyCollider.gameObject.GetComponent<EnemyWoodCutterController>() != null)
                {
                    enemyCollider.gameObject.GetComponent<EnemyWoodCutterController>().enabled = false;
                    enemyCollider.gameObject.GetComponent<EnemyPatrol>().enabled = false;
                    enemyCollider.gameObject.GetComponent<EnemyFollow>().enabled = false;

                }
                if (enemyCollider.gameObject.GetComponent<EnemyGoldMinerController>() != null)
                {
                    enemyCollider.gameObject.GetComponent<EnemyGoldMinerController>().enabled = false;
                    enemyCollider.gameObject.GetComponent<EnemyPatrol>().enabled = false;
                    enemyCollider.gameObject.GetComponent<EnemyFollow>().enabled = false;

                }

                GameObject enemy = enemyCollider.gameObject;
                paralyzedEnemies.Add(enemy);
                enemyCollider.gameObject.GetComponent<AnimationController>().PlayAnimation("Idle");
               
            }
        }
    }
    
    private void UnparalyzeEnemies()
    {
        foreach (GameObject enemy in paralyzedEnemies)
        {
            if (enemy != null && enemy.CompareTag("Enemy"))
            {
                if (enemy.GetComponent<EnemyHunterController>() != null)
                {
                    enemy.GetComponent<EnemyHunterController>().enabled = true;
                    enemy.GetComponent<EnemyPatrol>().enabled = true;
                }
                else if (enemy.GetComponent<EnemyWoodCutterController>() != null)
                {
                    enemy.GetComponent<EnemyWoodCutterController>().enabled = true;
                    enemy.GetComponent<EnemyPatrol>().enabled = true;
                    enemy.GetComponent<EnemyFollow>().enabled = true;
                }
                else if (enemy.GetComponent<EnemyGoldMinerController>() != null)
                {
                    enemy.GetComponent<EnemyGoldMinerController>().enabled = true;
                    enemy.GetComponent<EnemyPatrol>().enabled = true;
                    enemy.GetComponent<EnemyFollow>().enabled = true;
                }
            }
            
        }
        paralyzedEnemies.Clear();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, paralyzeDistance);
    }
}