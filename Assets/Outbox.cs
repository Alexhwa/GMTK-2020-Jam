using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outbox : MonoBehaviour
{
    public GameObject submitParticles;
    public GameObject floatingText;

    public bool isActive { get; private set; }

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
        Instantiate(submitParticles, transform.position, Quaternion.identity);
        Instantiate(floatingText, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
    }


    private void OnTimeOver() {
        isActive = false;
    }
}
