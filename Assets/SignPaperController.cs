using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SignPaperController : MonoBehaviour
{
    public Canvas canvas;
    public BigPaper paper;
    public RectTransform signArea;
    public GameObject linePrefab;
    private LineRenderer lineRenderer;

    public Vector2 previousDrawPoint;

    private float drawDistance;
    public float requiredDrawDistance;


    private void OnEnable() {
        BigPaper.OnPaperSubmit += OnPaperSubmit;   
    }
    private void OnDisable() {
        BigPaper.OnPaperSubmit -= OnPaperSubmit;
    }

    private void Start() {
        GameObject line = Instantiate(linePrefab, transform.position, Quaternion.identity);
        lineRenderer = line.GetComponent<LineRenderer>();
    }

    private void Update() {
        lineRenderer.transform.position = transform.position;
    }

    public void BeginDrag() {
        Vector2 mousePoint = paper.GetMousePoint();
        previousDrawPoint = mousePoint;

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, lineRenderer.transform.InverseTransformPoint(mousePoint));
    }

    public void Drag() {
        Vector2 mousePoint = paper.GetMousePoint();
        Vector2 drawPoint = Utilities.ClampInRect(mousePoint, Utilities.RectFromCenter(signArea.position, signArea.rect.size));

        if ((drawPoint - previousDrawPoint).sqrMagnitude >= 0.001f) {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, lineRenderer.transform.InverseTransformPoint(drawPoint));
        }

        drawDistance += (drawPoint - previousDrawPoint).magnitude;
        if (drawDistance >= requiredDrawDistance) {
            paper.PaperCompleted();
        }

        previousDrawPoint = drawPoint;
    }


    private void OnPaperSubmit() {
        if (paper.isSubmitted) {
            (paper.paperDisappearTween as Sequence)
                .Insert(0, lineRenderer.DOColor(new Color2(), new Color2(), paper.fadeTime).OnComplete(() => Destroy(lineRenderer.gameObject)));
        }
    }    
}
