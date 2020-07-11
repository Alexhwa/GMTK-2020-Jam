using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource bgmAudio;
    public AudioSource sfxAudio;

    public void PlayOneShot(AudioClip clip) {
        sfxAudio.PlayOneShot(clip);
    }
}
