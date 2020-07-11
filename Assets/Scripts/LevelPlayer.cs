using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelPlayer : MonoBehaviour
{
    public PaperSpawner spawner;
    public CanvasGroup raycastBlocker;

    public float levelTime;
    public float levelTimer { get; private set; }

    public bool started { get; private set; }

    public float TimeRemaining { get { return levelTimer >= 0 ? levelTime - levelTimer : levelTime; }}

    public static EmptyDelegate OnTimeOver;


    int score = 0;


    private void Awake() {
        spawner.levelPlayer = this;    
    }

    private void Start() {
        levelTimer = -3;
    }

    private void Update() {
        levelTimer = Mathf.Min(levelTimer + Time.deltaTime, levelTime);

        if (!started && levelTimer >= 0) {
            started = true;
            Managers.AudioManager.bgmAudio.Play();
        }
        if (levelTimer == levelTime) {
            TimeOver();
            Managers.AudioManager.bgmAudio.Stop();
        }
        Managers.AudioManager.bgmAudio.pitch = Mathf.Lerp(1, 1.8f, Mathf.Pow(1 - TimeRemaining / levelTime, 3));
     
        raycastBlocker.blocksRaycasts = !spawner.canSpawn;
    }

    private void OnEnable() {
        BigPaper.OnPaperSubmitted += PaperSubmitted;
    }

    private void OnDisable() {
        BigPaper.OnPaperSubmitted -= PaperSubmitted;
    }


    private void PaperSubmitted() {
        score++;
    }


    private void TimeOver() {
        OnTimeOver?.Invoke();
    }
}
