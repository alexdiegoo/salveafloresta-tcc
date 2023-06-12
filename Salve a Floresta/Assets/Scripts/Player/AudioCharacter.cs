using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCharacter : MonoBehaviour
{
    [SerializeField] private AudioSource footstepsAudioSource = null;
    [SerializeField] private AudioSource jumpAudioSource = null;
    [SerializeField] private AudioSource skillSaciAudioSource = null;
    [SerializeField] private AudioSource skillCurupiraAudioSource = null;
    [SerializeField] private AudioSource skillIaraAudioSource = null;
    [SerializeField] private AudioSource hitAudioSource = null;
    [SerializeField] private AudioSource energyCrystalAudioSource = null;
    [SerializeField] private AudioSource hearthAudioSource = null;

    
    [Header("Audio Clips")]
    [SerializeField] AudioClip[] steps = null;
    
    [Header("Steps")]
    [SerializeField] float timer = 0.5f;

    private float stepsTimer;

    public void PlaySteps(float speedNormalized)
    {
        stepsTimer += Time.fixedDeltaTime * speedNormalized; 
        
        if (stepsTimer >= timer)
        {
            int index = Random.Range(0, steps.Length);
            footstepsAudioSource.PlayOneShot(steps[index]);

            stepsTimer = 0;
        }
    }

    public void PlayJump()
    {
        jumpAudioSource.Play();
    }

    public void PlaySkillSaci()
    {
        skillSaciAudioSource.Play();
    }

    public void PlaySkillCurupira()
    {
        skillCurupiraAudioSource.Play();
    }

    public void PlaySkillIara()
    {
        skillIaraAudioSource.Play();
    }

    public void PlayHit()
    {
        hitAudioSource.Play();
    }

    public void PlayCollectCrystal()
    {
        energyCrystalAudioSource.Play();
    }

    public void PlayCollectHearth()
    {
        hearthAudioSource.Play();
    }
}
