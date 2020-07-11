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

    private void OnEnable() {
        BigPaper.OnPaperTrashed += OnPaperTrash;
        BigPaper.OnPaperSubmit += OnPaperSubmit;   
    }
    private void OnDisable() {
        BigPaper.OnPaperTrashed -= OnPaperTrash;
        BigPaper.OnPaperSubmit -= OnPaperSubmit;
    }


    private void Start() {
        foreach (Image check in checkmarks) {
            check.color = check.color.WithAlpha(0);
        }
    }

    public void CheckboxClicked(int index) {
        checkmarks[index].color = checkmarks[index].color.WithAlpha(1);

        if (checkmarks.All(check => check.color.a == 1)) {
            // completed
            paper.PaperCompleted();
        }
    }


    private void OnPaperTrash() {
        if (!paper.isActive) {
            foreach (Image check in checkmarks) {
                (paper.paperDisappearTween as Sequence)
                    .Insert(0, check.DOFade(0, paper.fadeTime));
            }
        }
    }
    private void OnPaperSubmit() {
        if (paper.isSubmitted) {
            foreach (Image check in checkmarks) {
                (paper.paperDisappearTween as Sequence)
                    .Insert(0, check.DOFade(0, paper.fadeTime));
            }
        }
    }
}
