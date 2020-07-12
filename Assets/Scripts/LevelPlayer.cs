using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelPlayer : MonoBehaviour
{
    public PaperSpawner spawner;
    public CanvasGroup raycastBlocker;
    public EmployeeReport report;

    public float levelTime;
    public float levelTimer { get; private set; }
    public float TimeRemaining { get { return levelTimer >= 0 ? levelTime - levelTimer : levelTime; }}

    public int oneStarThreshold;
    public int twoStarThreshold;
    public int threeStarThreshold;

    public bool isActive { get; set; }
    public bool started { get; private set; }
    public bool finished { get; private set; }

    public bool shouldAudioPitch = true;

    public static EmptyDelegate OnTimeOver;

    public InterfaceManager interfaceManager;

    public int score = 0;


    private void Awake() {
        spawner.levelPlayer = this;    
    }

    private void Start() {
        levelTimer = -3;
        interfaceManager.onDialogueEnd.AddListener(() => ActivateHelpBox());
    }

    private void Update() {
        if (!isActive) {
            return;
        }

        levelTimer = Mathf.Min(levelTimer + Time.deltaTime, levelTime);

        if (!started && levelTimer >= 0) {
            started = true;
            Managers.AudioManager.bgmAudio.pitch = 1;
            Managers.AudioManager.bgmAudio.Play();
        }
        if (!finished && levelTimer == levelTime) {
            finished = true;
            TimeOver();
            Managers.AudioManager.bgmAudio.Stop();
            Managers.AudioManager.bgmAudio.pitch = 1;
        }

        if (started && !finished && shouldAudioPitch) {
            Managers.AudioManager.bgmAudio.pitch = Mathf.Lerp(1, 1.8f, Mathf.Pow(1 - TimeRemaining / levelTime, 3));
        }

        raycastBlocker.blocksRaycasts = !spawner.canSpawn;
    }

    private void OnEnable() {
        BigPaper.OnPaperSubmitted += PaperSubmitted;
        BigPaper.OnPaperTrashed += PaperTrashed;
    }

    private void OnDisable() {
        BigPaper.OnPaperSubmitted -= PaperSubmitted;
        BigPaper.OnPaperTrashed -= PaperTrashed;
    }


    private void PaperSubmitted(BigPaper paper) {
        if (!paper.isCounterfeit)
            score++;
        else
            score--;
    }

    private void PaperTrashed(BigPaper paper) {
        if (paper.isCounterfeit && !paper.isCompleted)
            score++;
    }


    private void TimeOver() {
        Utilities.Invoke(() => report.InitializeReport(this), 1.5f, this);

        OnTimeOver?.Invoke();
    }
    private void ActivateHelpBox()
    {
        print("Dialogue Ended. Activating help box.");
    }
}
