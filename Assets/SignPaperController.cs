using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SignPaperController : MonoBehaviour
{
    public Canvas canvas;
    public BigPaper paper;
    public LineRenderer lineRenderer;

    public Vector2 previousDrawPoint;

    private float drawDistance;
    public float requiredDrawDistance;

    public void BeginDrag() {
        Debug.Log("begin drag");
        Vector2 mousePoint = GetMousePoint();
        previousDrawPoint = mousePoint;

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, lineRenderer.transform.InverseTransformPoint(mousePoint));
    }

    public void Drag() {
        Vector2 mousePoint = GetMousePoint();
        Vector2 drawPoint = mousePoint;

        drawDistance += (drawPoint - previousDrawPoint).magnitude;
        if (drawDistance >= requiredDrawDistance) {
            paper.PaperCompleted();
        }

        previousDrawPoint = drawPoint;
    }


    private Vector2 GetMousePoint() => canvas.worldCamera.ScreenToWorldPoint(Input.mousePosition).ToVector2();
}
