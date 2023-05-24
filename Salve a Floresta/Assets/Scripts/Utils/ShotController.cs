using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    [SerializeField] float timeDestroyShot = 3f; // Tempo para remover o tiro de cena
   
    void Start()
    {
        Destroy(gameObject, timeDestroyShot);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}