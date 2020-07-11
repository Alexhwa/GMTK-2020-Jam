using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CheckPaperController : MonoBehaviour
{
    public BigPaper paper;
    public List<Image> checkmarks;
    public int requiredCheckmarks;

    public AudioClip scribbleSound;

    private void OnEnable() {
        // paper.OnTrashed.AddListener(OnPaperTrash);
        // paper.OnSubmit.AddListener(OnPaperSubmit);   
    }
    private void OnDisable() {
        // paper.OnTrashed.RemoveListener(OnPaperTrash);
        // paper.OnSubmit.RemoveListener(OnPaperSubmit);  
    }


    private void Start() {
        foreach (Image check in checkmarks) {
            check.color = check.color.WithAlpha(0);
        }
    }

    public void CheckboxClicked(int index) {
        Managers.AudioManager.PlayOneShot(scribbleSound);
        checkmarks[index].color = checkmarks[index].color.WithAlpha(1);

        if (checkmarks.Where(check => check.color.a == 1).Count() >= requiredCheckmarks) {
            // completed
            paper.PaperCompleted();
        }
    }

    /*
    private void OnPaperTrash() {
        foreach (Image check in checkmarks) {
            (paper.paperDisappearTween as Sequence)
                .Insert(0, check.DOFade(0, paper.fadeTime));
        }
    }
    private void OnPaperSubmit() {
        foreach (Image check in checkmarks) {
            (paper.paperDisappearTween as Sequence)
                .Insert(0, check.DOFade(0, paper.fadeTime));
        }
    }
    */
}
