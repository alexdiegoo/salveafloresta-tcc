using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    [Header("Physics")]
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] bool isFacingRight = true;
    [SerializeField] float fallGravity;
    [SerializeField] float jumpGravity;

    [Header("Knockback Settings")]
    public float kbForce;
    public float KbCount;
    public float KbTime;
    public bool isKnockRight = false;

    [Header("SpecialAttack Settings")] 
    public float fireForce = 20f;
    public float specialCount = 5f;
    [SerializeField] private float dashAtual; // Duração do dash atual
    [SerializeField] private bool canDash; // Saber se ja pode executar o dash novamente
    [SerializeField] private bool isDashing; // Para sabe se esta executando dash
    [SerializeField] private float dashSpeed; // Velocidade do dash
    [SerializeField] private float dashingTime = 0.5f; // Quantidade de tempo do dash
    

    private float nextSpecialTime;



    private float gravityScale;
    private GameController gameController;

    private float inputForce;
    private float forceSum;

    void Start()
    {
        AnimationController.PlayAnimation("Idle");

        managerInput.OnButtonEvent += ManagerInput_OnButtonEvent;
        gravityScale = rigidBody2D.gravityScale;
        gameController = GameController.gameController;
        

        canDash = true;
        dashAtual = dashingTime;
        nextSpecialTime = Time.time + specialCount;
    }

    private void ManagerInput_OnButtonEvent()
    {
        if(detection.ground != null && Input.GetKeyDown(KeyCode.Space))
        {
            JumpPlayer();
        }
        else if(Input.GetKeyDown(KeyCode.X))
        {
            if(gameController.energyCrystals >= 2 /*&& Time.time > nextSpecialTime*/)
            {
                /*
                SpecialFire();
                gameController.SetEnergyCrystals(-2);
                nextSpecialTime = Time.time + specialCount;
                */                
            }
        }
    }


    void Update()
    {
        CheckDirection();
        if (detection.ground != null)
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
        else
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

            collision.GetComponent<AnimationController>().PlayAnimation("Death");
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            collision.gameObject.GetComponent<EnemyPatrol>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            Destroy(collision.gameObject, 1f);
        }

        if(collision.gameObject.tag == "EnergyCrystal")
        {
            gameController.SetEnergyCrystals(1);
            Destroy(collision.gameObject);
        }
    }

    public void SpecialFire()
    {
        GameObject fire = Instantiate(firePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D fireRb = fire.GetComponent<Rigidbody2D>();
        fireRb.velocity = firePoint.right * fireForce;
        Debug.Log("Lançou");
    }
}