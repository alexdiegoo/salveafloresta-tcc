using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] ManagerInput managerInput;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] PlayerAnimationController playerAnimationController;

    [Header("Physics")]
    [SerializeField] float speed;
    [SerializeField] bool isFacingRight = true;


    void Start()
    {
        playerAnimationController.PlayAnimation("Idle");
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
}
