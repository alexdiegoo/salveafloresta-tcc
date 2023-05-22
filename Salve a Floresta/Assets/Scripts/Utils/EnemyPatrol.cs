using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] pratrolPoints;
    public float moveSpeed;
    public int patrolDestination;
    [SerializeField] bool isFacingRight = true;
    [SerializeField] EnemyHunterAnimationController enemyHunterAnimationController;

    void Update()
    {
        if(patrolDestination == 0 && Vector2.Distance(transform.position, pratrolPoints[0].position) < .5f)
        {
            patrolDestination = 1;
        }

        if(patrolDestination == 1 && Vector2.Distance(transform.position, pratrolPoints[1].position) < .5f)
        {
            patrolDestination = 0;
        }

        MoveToPoint();
        CheckDirection();
    }

    private void CheckDirection()
    {
        if(isFacingRight && patrolDestination == 0)
        {
            Flip();
        }
        else if(!isFacingRight && patrolDestination == 1)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    private void MoveToPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, pratrolPoints[patrolDestination].position, moveSpeed * Time.deltaTime);
        enemyHunterAnimationController.PlayAnimation("Walk");
    }

}
