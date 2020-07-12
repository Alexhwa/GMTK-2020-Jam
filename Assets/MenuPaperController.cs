using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuPaperController : MonoBehaviour
{
    public BigPaper paper;

    public UnityEvent OnSubmit;


    private void OnEnable() {
        paper.OnSubmit.AddListener(OnPaperSubmit);
    }
    private void OnDisable() {
        paper.OnSubmit.RemoveListener(OnPaperSubmit);  
    }

    private void OnPaperSubmit() {
        OnSubmit?.Invoke();
    }


    private void Start() {
        paper.isCompleted = true;
    }

    public void NewGame() {
        Managers.ScenesManager.LoadNextLevel();
    }

    public void ContinueGame() {

    }

    public void SpawnCredits() {

    }

}
