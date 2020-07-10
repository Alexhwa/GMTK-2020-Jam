using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SignPaper : Paper
{
    [Header("Sign")]
    public Collider2D signCollider;
    public LineRenderer lineRenderer;
    public float requiredDrawDistance;
    private float drawDistance;
    private Vector2 previousDrawPoint;


    protected override void OnClicked(Vector2 mousePoint) {
        base.OnClicked(mousePoint);

        if (!signCollider.bounds.Contains(mousePoint)) {
            isClicked = false;
            return;
        }

        previousDrawPoint = mousePoint;

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, lineRenderer.transform.InverseTransformPoint(mousePoint));
    }

    protected override void OnHeld(Vector2 mousePoint) {
        base.OnHeld(mousePoint);

        Vector2 drawPoint = signCollider.bounds.ClosestPoint(mousePoint);

        if ((drawPoint - previousDrawPoint).sqrMagnitude >= 0.001f) {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, lineRenderer.transform.InverseTransformPoint(drawPoint));
        }

        drawDistance += (drawPoint - previousDrawPoint).magnitude;
        if (drawDistance >= requiredDrawDistance) {
            PaperCompleted();
        }

        previousDrawPoint = drawPoint;
    }


    protected override void PaperCompleted() {
        base.PaperCompleted();
        (paperDisappearTween as Sequence).Insert(0, lineRenderer.DOColor(new Color2(), new Color2(), fadeTime));
    }
}
