using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outbox : MonoBehaviour
{
    public GameObject goodSubmitParticles;
    public GameObject goodFloatingText;
    public GameObject badSubmitParticles;
    public GameObject badFloatingText;
    public AudioClip submitAudio;
    public AudioClip failAudio;

    public bool isActive { get; private set; }
    public bool isOnMenu;

    private void Start() {
        isActive = true;
    }

    private void OnEnable() {
        LevelPlayer.OnTimeOver += OnTimeOver;
    }
    private void OnDisable() {
        LevelPlayer.OnTimeOver -= OnTimeOver;
    }

    public void PaperSubmitted(BigPaper paper) {
        if (!paper.isCounterfeit) {
            if (!isOnMenu) {
                Instantiate(goodSubmitParticles, transform.position, Quaternion.identity);
                Instantiate(goodFloatingText, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            }

            Managers.AudioManager.PlayOneShot(submitAudio);
        }
        else {
            if (!isOnMenu) {
                Instantiate(badSubmitParticles, transform.position, Quaternion.identity);
                Instantiate(badFloatingText, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            }
            
            Managers.AudioManager.PlayOneShot(failAudio);
        }
    }


    private void OnTimeOver() {
        isActive = false;
    }
}
