using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlataform : MonoBehaviour
{
    public float speed; // Velocidade da plataforma
    public int startingPoint;
    public Transform[] points;

    private int i; // Ponto inicial

    void Start()
    {
        transform.position= points[startingPoint].position;
    }

    void Update()
    {
        if(Vector2.Distance(transform.position, points[i].position) < 0.2f)
        {
            i++;
            if(i == points.Length)
            {
                i = 0; // Reseta para o primeiro ponto
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }
}
