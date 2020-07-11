using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : LevelElement
{
    public void DispensePaper(List<GameObject> paperPrefabs) {
        GameObject newPaper = Instantiate(paperPrefabs[Random.Range(0, paperPrefabs.Count)], transform.position, Quaternion.identity);
        Paper paper = newPaper.GetComponent<Paper>();
        
        paper.levelRunner = levelRunner;
        if (levelRunner.numPapers != 0 && (levelRunner.numPapers % levelRunner.timePausePaperInterval == 0)) {
            paper.SetTimeStop();
        }
    }
}
