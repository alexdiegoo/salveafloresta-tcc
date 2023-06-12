using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CucaAudio : MonoBehaviour
{
    [SerializeField] private AudioSource attack1_1AudioSource = null;
    [SerializeField] private AudioSource attack2AudioSource = null;
    [SerializeField] private AudioSource attack3AudioSource = null;
    
    public void PlayAttack1()
    {
        attack1_1AudioSource.Play();
    }

    public void PlayAttack2()
    {
        attack2AudioSource.Play();
    }

    public void PlayAttack3()
    {
        attack3AudioSource.Play();
    }
}
