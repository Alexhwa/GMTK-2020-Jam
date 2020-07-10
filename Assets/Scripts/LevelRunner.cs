using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    public List<GameObject> paperPrefabs;


    private void Update() {
        dispenseTimer += Time.deltaTime;

        if (dispenseTimer >= dispenseBaseRate / dispenseSpeedMultiplier) {
            dispenseTimer -= dispenseBaseRate / dispenseSpeedMultiplier;
            dispensers[Random.Range(0, dispensers.Count)].DispensePaper(paperPrefabs);
        
            dispenseSpeedMultiplier = Mathf.Lerp(dispenseSpeedMultiplier, 4, 0.01f);
        }
    }
    
}
