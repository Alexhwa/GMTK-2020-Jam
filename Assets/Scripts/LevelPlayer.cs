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

    public AudioClip playSong;

    public InterfaceManager interfaceManager;
    private int dialogueCounter;
    public DialogueData failDialogue;

    public int score = 0;


    private void Awake() {
        spawner.levelPlayer = this;    
    }

    private void Start() {
        levelTimer = -3;
        interfaceManager.ActivateDialogue(null);
    }

    private void Update() {
        if (!isActive) {
            return;
        }

        if (levelTimer == -3) {
            Managers.AudioManager.bgmAudio.pitch = 1;
            Managers.AudioManager.PlayLoop(Managers.AudioManager.bgmAudio, playSong, 3);
        }

        levelTimer = Mathf.Min(levelTimer + Time.deltaTime, levelTime);

        if (!started && levelTimer >= 0) {
            started = true;
        }
        if (!finished && levelTimer == levelTime) {
            finished = true;
            TimeOver();
            Managers.AudioManager.StopLoop(Managers.AudioManager.bgmAudio, 0);
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
            score += 2;
        else
            score -= 2;
    }

    private void PaperTrashed(BigPaper paper) {
        if (paper.isCounterfeit && !paper.isCompleted)
            score++;
    }


    private void TimeOver() {
        Utilities.Invoke(() => report.InitializeReport(this), 1.5f, this);

        OnTimeOver?.Invoke();
        if (CheckFailed())
        {
            interfaceManager.ActivateDialogue(failDialogue);
        }
        else
        {
            if (interfaceManager.levelDialogue.Length >= 2)
            {
                interfaceManager.onDialogueEnd.AddListener(() => StartCoroutine(PlayAnotherDialogue(1.4f)));

                interfaceManager.ActivateDialogue(null);
            }
        }
    }
    IEnumerator PlayAnotherDialogue(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (dialogueCounter < interfaceManager.levelDialogue.Length - 2)
        {
            interfaceManager.ActivateDialogue(null);
            dialogueCounter++;
        }
    }
    private bool CheckFailed()
    {
        return score < oneStarThreshold;
    }
}
