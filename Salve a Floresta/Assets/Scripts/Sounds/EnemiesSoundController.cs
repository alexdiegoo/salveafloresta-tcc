using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource shotAudioSource = null;

    public void PlayShot()
    {
        shotAudioSource.Play();
    }
}
