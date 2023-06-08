using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource musicBackgroundAudioSource = null;

    public void PlayBackgroundMusic()
    {
        musicBackgroundAudioSource.Play();
    }
}
