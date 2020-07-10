using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    public float dispenseBaseRate;
    public float dispenseSpeedMultiplier;

    private float dispenseTimer;

    public List<GameObject> paperPrefabs;

    private void Update() {
        dispenseTimer += Time.deltaTime;

        if (dispenseTimer >= dispenseBaseRate / dispenseSpeedMultiplier) {
            dispenseTimer -= dispenseBaseRate / dispenseSpeedMultiplier;
            DispensePaper();
        }
    }

    private void DispensePaper() {
        GameObject newPaper = Instantiate(paperPrefabs[Random.Range(0, paperPrefabs.Count)], transform.position, Quaternion.identity);
    }
}
