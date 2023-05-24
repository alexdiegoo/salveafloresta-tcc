using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] pratrolPoints;
    public float moveSpeed;
    public int patrolDestination;
    public bool isFacingRight = true;
    public AnimationController animationController;

    public Transform player;
    public float playerDetectionDistance = 10f; // Distância para detectar o jogador

    void Update()
    {
        if (StopPatrol())
        {
            // Realize ações no script controlador do inimigo
            // (por exemplo, pare de patrulhar e siga o jogador)

            // Verifica a direção do jogador
            Vector3 directionToPlayer = player.position - transform.position;
            float playerDirection = Mathf.Sign(directionToPlayer.x);

            if(playerDirection == 1 && isFacingRight == false)
            {
                Flip();
            }
            else if(playerDirection == -1 && isFacingRight == true)
            {
                Flip();
            }
            
            return;
        }

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

    public bool StopPatrol()
    {
        // Verificar se o jogador está dentro da distância de detecção
        if (Vector2.Distance(transform.position, player.position) <= playerDetectionDistance)
        {
            return true;
        }

        return false;
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
        animationController.PlayAnimation("Walk");
    }

}
