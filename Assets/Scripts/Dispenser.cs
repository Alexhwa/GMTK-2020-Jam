using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : LevelElement
{
    public void DispensePaper(List<GameObject> paperPrefabs) {
        GameObject newPaper = Instantiate(paperPrefabs[Random.Range(0, paperPrefabs.Count)], transform.position, Quaternion.identity);
    }
}
