using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] ManagerInput managerInput;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] PlayerAnimationController playerAnimationController;
    [SerializeField] Detection detection;

    [Header("Physics")]
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] bool isFacingRight = true;
    [SerializeField] float fallGravity;
    [SerializeField] float jumpGravity;
    
    private float gravityScale;


    void Start()
    {
        playerAnimationController.PlayAnimation("Idle");

        managerInput.OnButtonEvent += ManagerInput_OnButtonEvent;
        gravityScale = rigidBody2D.gravityScale;
    }

    private void ManagerInput_OnButtonEvent()
    {
        if(detection.ground != null)
        {
            JumpPlayer();
        }
    }


    void Update()
    {
        CheckDirection();
        if (Mathf.Abs(managerInput.GetInputX()) > 0f)
        {
            playerAnimationController.PlayAnimation("Run"); // Toca a animação "Run" quando o jogador está se movendo
        }
        else
        {
            playerAnimationController.PlayAnimation("Idle"); // Toca a animação "Idle" quando o jogador não está se movendo
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
}
