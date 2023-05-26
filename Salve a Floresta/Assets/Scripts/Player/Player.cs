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

    [Header("Physics")]
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] bool isFacingRight = true;
    [SerializeField] float fallGravity;
    [SerializeField] float jumpGravity;
    
    private float gravityScale;
    private GameController gameController;

    void Start()
    {
        AnimationController.PlayAnimation("Idle");

        managerInput.OnButtonEvent += ManagerInput_OnButtonEvent;
        gravityScale = rigidBody2D.gravityScale;
        gameController = GameController.gameController;
    }

    private void ManagerInput_OnButtonEvent()
    {
        if(detection.ground != null && Input.GetKeyDown(KeyCode.Space))
        {
            JumpPlayer();
        }
        else if(Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Apertou X");
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
        Move();
        JumpBetterPlayer();
    }

    private void Move()
    {
        rigidBody2D.velocity = new Vector2(managerInput.GetInputX() * speed, rigidBody2D.velocity.y);
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

            collision.gameObject.GetComponent<EnemyHunterController>().enabled = false;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            collision.gameObject.GetComponent<EnemyPatrol>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            collision.GetComponent<AnimationController>().PlayAnimation("Death");
            Destroy(collision.gameObject, 1f);
        }

        if(collision.gameObject.tag == "EnergyCrystal")
        {
            gameController.SetEnergyCrystals(1);
            Destroy(collision.gameObject);
        }
    }
}