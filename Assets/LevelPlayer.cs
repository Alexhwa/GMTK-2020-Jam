using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelPlayer : MonoBehaviour
{
    public PaperSpawner spawner;

    public float levelTime;
    private float levelTimer;

    public float TimeRemaining { get { return levelTimer >= 0 ? levelTime - levelTimer : levelTime; }}


    int score = 0;


    private void Start() {
        levelTimer = -3;
    }

    private void Update() {
        levelTimer = Mathf.Min(levelTimer + Time.deltaTime, levelTime);
        
        if (levelTimer >= 0 && levelTimer < levelTime) {
            spawner.canSpawn = true;
        }
        else {
            spawner.canSpawn = false;
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
}
