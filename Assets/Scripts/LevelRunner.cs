using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelRunner : MonoBehaviour
{
    public List<Dispenser> dispensers;
    public bool isTimePaused;
    public bool isInCutscene;

    public float dispenseBaseRate;
    public float dispenseSpeedMultiplier;
    private float dispenseTimer;

    public float paperBaseSpeed;
    public float paperSpeedMultiplier;

    public int numPapers;
    public int timePausePaperInterval;
    
    public List<GameObject> paperPrefabs;

    public float timePauseDuration;
    private Coroutine currentTimePause;


    private void OnEnable() {
        Paper.OnPaperComplete += PaperDestroyed;
        Paper.OnPaperShred += PaperDestroyed;    
    }

    private void OnDisable() {
        Paper.OnPaperComplete -= PaperDestroyed;
        Paper.OnPaperShred -= PaperDestroyed;
    }


    private void Update() {
        if (isTimePaused) {
            return;
        }

        dispenseTimer += Time.deltaTime;

        if (dispenseTimer >= dispenseBaseRate / dispenseSpeedMultiplier) {
            dispenseTimer -= dispenseBaseRate / dispenseSpeedMultiplier;
            dispensers[Random.Range(0, dispensers.Count)].DispensePaper(paperPrefabs);

            numPapers++;
        }
    }


    private void PaperDestroyed() {
        numPapers--;

        if (numPapers == 0) {
            if (currentTimePause != null) {
                StopCoroutine(currentTimePause);
            }
            isTimePaused = false;
        }
    }


    public void DoTimePause() {
        isTimePaused = true;

        if (currentTimePause != null) {
            StopCoroutine(currentTimePause);
        }
        currentTimePause = Utilities.Invoke(() => isTimePaused = false, timePauseDuration, this);
    }
    
}
