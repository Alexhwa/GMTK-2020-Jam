using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MenuPaperController : MonoBehaviour
{
    public BigPaper paper;
    public MainMenu mainMenu;
    private int id;
    public TextMeshProUGUI idText;

    public UnityEvent OnSubmit;


    private void OnEnable() {
        paper.OnSubmit.AddListener(OnPaperSubmit);
    }
    private void OnDisable() {
        paper.OnSubmit.RemoveListener(OnPaperSubmit);  
    }


    public void SetId(int value) {
        id = value;
        if (idText != null) {
            idText.text = "Day " + (id + 1);
        }
    }

    private void OnPaperSubmit() {
        OnSubmit?.Invoke();
    }


    private void Start() {
        paper.isCompleted = true;
    }


    public void SpawnLevels() {
        (paper.paperSpawner as MenuPaperSpawner).SpawnLevelSelect();
    }

    public void SelectLevel() {
        mainMenu.SelectLevel(Managers.ScenesManager.firstLevelBuildIndex + id);
    }

    public void SpawnCredits() {
        (paper.paperSpawner as MenuPaperSpawner).SpawnCredits();
    }

    public void RespawnCreditsPrompt() {
        (paper.paperSpawner as MenuPaperSpawner).SpawnCreditsPrompt();
    }

}
