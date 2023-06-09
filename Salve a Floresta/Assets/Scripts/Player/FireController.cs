using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public float timeDestroyFire = 3f;
    void Start()
    {
        Destroy(gameObject, timeDestroyFire);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("EnergyCrystal"))
        {
            return;
        }
        
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (col.gameObject.GetComponent<EnemyHunterController>())
            {
                col.gameObject.GetComponent<EnemyHunterController>().enabled = false;
            }
            else if (col.gameObject.GetComponent<EnemyWoodCutterController>())
            {
                col.gameObject.GetComponent<EnemyWoodCutterController>().enabled = false;
            }
            else if (col.gameObject.GetComponent<EnemyGoldMinerController>())
            {
                col.gameObject.GetComponent<EnemyGoldMinerController>().enabled = false;
            }
            
            col.gameObject.GetComponent<AnimationController>().PlayAnimation("Death");
            col.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            col.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            col.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Destroy(gameObject);
            Destroy(col.gameObject, 0.8f);
        }
        else
        {
            Destroy((gameObject));
        }
    }
}
