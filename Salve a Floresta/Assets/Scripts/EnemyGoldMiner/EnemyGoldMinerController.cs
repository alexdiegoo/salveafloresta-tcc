using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoldMinerController : MonoBehaviour
{
    [Header("Dependencies")]
    public EnemyPatrol enemyPatrol;
    public AnimationController animationController;
    public EnemyFollow enemyFollow;
    public Rigidbody2D rb;
    public GameObject obstacleRayObject;
    public LayerMask obstacleLayerMask;
    public GameObject player;

    [Header("Enemy Settings")]
    public float minimumDistanceHit = 2f; // Distância mínima desejada entre o jogador e o inimigo
    public float jumpForce = 6f;
    public float obstacleRayDistance = 2f;

    private Player playerController;
    void Start()
    {
         playerController = player.GetComponent<Player>();
    }

    void Update()
    {
         if(enemyPatrol.StopPatrol())
        {    
            enemyFollow.FollowPlayer();
            animationController.PlayAnimation("Run");
        }

        if (CheckObstacle())
        {
            Jump();
        }

        if (CheckPlayerDistance())
        {
            HitPlayer();
        }
    }

    private bool CheckObstacle()
    {
        RaycastHit2D hit;
        if (transform.rotation.y == -1)
        {
            hit = Physics2D.Raycast(obstacleRayObject.transform.position, Vector2.left, obstacleRayDistance, obstacleLayerMask);
            Debug.DrawRay(obstacleRayObject.transform.position, Vector2.left * obstacleRayDistance, Color.red);

        }
        else
        {
            hit = Physics2D.Raycast(obstacleRayObject.transform.position, Vector2.right, obstacleRayDistance, obstacleLayerMask);
            Debug.DrawRay(obstacleRayObject.transform.position, Vector2.right * obstacleRayDistance, Color.red);
        }

        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    private bool CheckPlayerDistance()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= minimumDistanceHit)
        {
           playerController.KbCount = playerController.KbTime;

           if(playerController.transform.position.x <= transform.position.x)
           {
                playerController.isKnockRight = true;
           }
           else if (playerController.transform.position.x > transform.position.x)
           {
                playerController.isKnockRight = false;
           }

           return true;
        }

        return false;
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void HitPlayer()
    {
        player.GetComponent<PlayerLife>().LoseLife();
    }
}
