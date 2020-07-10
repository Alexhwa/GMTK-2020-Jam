using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CheckPaper : Paper
{
    [Header("Check")]
    public List<Checkbox> checkboxes;
    private int numCheckboxesLeft;

    protected override void Start() {
        base.Start();
        numCheckboxesLeft = checkboxes.Count;
    }

    protected override void OnClicked(Vector2 mousePoint) {
        base.OnClicked(mousePoint);

        Debug.Log("clicking at " + mousePoint + " " + checkboxes[0].hitbox.bounds.Contains(mousePoint));

        foreach (Checkbox checkbox in checkboxes) {
            if (!checkbox.isChecked && checkbox.hitbox.bounds.Contains(mousePoint)) {
                checkbox.OnChecked();
                numCheckboxesLeft--;
            }
        }

        if (numCheckboxesLeft == 0) {
            PaperCompleted();
        }
    }

    protected override void PaperCompleted() {
        base.PaperCompleted();

        foreach (Checkbox checkbox in checkboxes) {
            (paperDisappearTween as Sequence).Insert(0, checkbox.checkSr.DOFade(0, fadeTime));
        }
    }
}
