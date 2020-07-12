using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPaperSpawner : PaperSpawner
{
    public Transform spawnLocationsParent;
    private Transform[] spawnLocations;

    public GameObject creditsPromptPaper;
    public GameObject creditsPaper;
    public GameObject levelSelectPaper;

    public MainMenu mainMenu;


    private void Awake() {
        spawnLocations = new Transform[spawnLocationsParent.childCount];
        for (int i = 0; i < spawnLocations.Length; i++) {
            spawnLocations[i] = spawnLocationsParent.GetChild(i);
        }
    }


    public void SpawnLevelSelect() {
        for (int i = 0; i < Managers.ScenesManager.numLevels; i++) {
            BigPaper paper = SpawnPaper(levelSelectPaper, spawnLocations[i].position);
            MenuPaperController menuPaper = paper.GetComponent<MenuPaperController>();
            menuPaper.SetId(i);
            menuPaper.mainMenu = mainMenu;
        }
    }

    public void SpawnCreditsPrompt() {
        SpawnPaper(creditsPromptPaper, spawnLocations[spawnLocations.Length - 1].position);
    }
    public void SpawnCredits() {
        SpawnPaper(creditsPaper, spawnLocations[spawnLocations.Length - 1].position);
    }


    private void OnDrawGizmos() {
        if (spawnLocationsParent != null) {
            Gizmos.color = Color.cyan;
            
            foreach (Transform child in spawnLocationsParent) {
                Gizmos.DrawWireSphere(child.transform.position, 0.2f);
            }
        }
    }
}
