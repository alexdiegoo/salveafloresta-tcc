using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPortionController : MonoBehaviour
{
    public float timeDestroy = 2f;

    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

    void Update()
    {
        
    }
}
