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

    public float TimeRemaining { get { return levelTimer >= 0 ? levelTime - levelTimer : levelTime; }}

    public static EmptyDelegate OnTimeOver;


    int score = 0;


    private void Start() {
        levelTimer = -3;
    }

    private void Update() {
        levelTimer = Mathf.Min(levelTimer + Time.deltaTime, levelTime);

        if (levelTimer == levelTime) {
            TimeOver();
        }
        
        if (levelTimer >= 0 && levelTimer < levelTime) {
            spawner.canSpawn = true;
            raycastBlocker.blocksRaycasts = false;
        }
        else {
            spawner.canSpawn = false;
            raycastBlocker.blocksRaycasts = true;
        }
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
