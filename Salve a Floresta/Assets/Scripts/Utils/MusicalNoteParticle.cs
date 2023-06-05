using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalNoteParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       GetComponent<ParticleSystem>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
