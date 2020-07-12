using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    public AudioSource bgmAudio;
    public AudioSource sfxAudio;
    public AudioSource ambienceAudio;

    public void PlayOneShot(AudioClip clip) {
        sfxAudio.PlayOneShot(clip);
    }


    public void PlayLoop(AudioSource audioSource, AudioClip clip, float fadeTime) {
        audioSource.DOKill();

        audioSource.clip = clip;
        audioSource.volume = 0;
        
        audioSource.Play();
        audioSource.DOFade(1, fadeTime);
    }
    public void StopLoop(AudioSource audioSource, float fadeTime) {
        audioSource.DOKill();
        audioSource.DOFade(0, fadeTime);
    }
}
