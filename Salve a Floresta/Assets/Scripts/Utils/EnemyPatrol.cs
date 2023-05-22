using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] pratrolPoints;
    public float moveSpeed;
    public int patrolDestination;

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
    }

    private void MoveToPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, pratrolPoints[patrolDestination].position, moveSpeed * Time.deltaTime);
    }
}
