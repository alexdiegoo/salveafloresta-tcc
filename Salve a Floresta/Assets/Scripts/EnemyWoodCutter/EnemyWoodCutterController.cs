using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWoodCutterController : MonoBehaviour
{
    [Header("Dependencies")]
    public EnemyPatrol enemyPatrol;
    public AnimationController animationController;
    public EnemyFollow enemyFollow;
    public Rigidbody2D rb;
    public GameObject obstacleRayObject;
    public LayerMask layerMask;


    public float jumpForce = 6f;
    public float obstacleRayDistance = 2f;
    

    void Start()
    {
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
            return;
        }
    }

    private bool CheckObstacle()
    {
        RaycastHit2D hit;
        if (transform.rotation.y == -1)
        {
            hit = Physics2D.Raycast(obstacleRayObject.transform.position, Vector2.left, obstacleRayDistance, layerMask);
            Debug.DrawRay(obstacleRayObject.transform.position, Vector2.left * obstacleRayDistance, Color.red);

        }
        else
        {
            hit = Physics2D.Raycast(obstacleRayObject.transform.position, Vector2.right, obstacleRayDistance, layerMask);
            Debug.DrawRay(obstacleRayObject.transform.position, Vector2.right * obstacleRayDistance, Color.red);
        }

        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

}
