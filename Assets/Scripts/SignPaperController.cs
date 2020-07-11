using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SignPaperController : MonoBehaviour
{
    public BigPaper paper;
    public Image signArea;
    public GameObject linePrefab;
    private LineRenderer lineRenderer;

    public Vector2 previousDrawPoint;

    private float drawDistance;
    public float requiredDrawDistance;
    public Color completedColor;

    public AudioClip scribbleSound;

    private void OnEnable() {
        paper.OnSortToTop.AddListener(OnSort);
        paper.OnDestroyed.AddListener(OnPaperDestroy);
        paper.OnTrashed.AddListener(OnPaperTrash);
        paper.OnSubmit.AddListener(OnPaperSubmit);   
    }
    private void OnDisable() {
        paper.OnSortToTop.RemoveListener(OnSort);
        paper.OnDestroyed.AddListener(OnPaperDestroy);
        paper.OnTrashed.RemoveListener(OnPaperTrash);
        paper.OnSubmit.RemoveListener(OnPaperSubmit);  
    }

    private void Start() {
        GameObject line = Instantiate(linePrefab, transform.position, Quaternion.identity);
        lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.sortingOrder = paper.canvas.sortingOrder + 1;
    }

    private void Update() {
        lineRenderer.transform.position = transform.position;
        lineRenderer.transform.rotation = transform.rotation;
    }

    public void BeginDrag() {
        Vector2 mousePoint = paper.GetMousePoint();
        previousDrawPoint = mousePoint;

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, lineRenderer.transform.InverseTransformPoint(mousePoint));

        Managers.AudioManager.PlayOneShot(scribbleSound);
    }

    public void Drag() {
        Vector2 mousePoint = paper.GetMousePoint();
        Vector2 drawPoint = Utilities.ClampInRect(mousePoint, Utilities.RectFromCenter(signArea.rectTransform.position, signArea.rectTransform.rect.size));

        if ((drawPoint - previousDrawPoint).sqrMagnitude >= 0.001f) {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, lineRenderer.transform.InverseTransformPoint(drawPoint));
        }

        drawDistance += (drawPoint - previousDrawPoint).magnitude;
        if (!paper.isCompleted && drawDistance >= requiredDrawDistance) {
            // completed
            paper.PaperCompleted();
            signArea.color = completedColor;
        }

        previousDrawPoint = drawPoint;
    }


    private void OnSort() {
        lineRenderer.sortingOrder = paper.canvas.sortingOrder + 1;
    }

    private void OnPaperTrash() {
        (paper.paperDisappearTween as Sequence)
            .Insert(0, lineRenderer.DOColor(new Color2(), new Color2(), paper.fadeTime).OnComplete(() => Destroy(lineRenderer.gameObject)));
    }
    private void OnPaperSubmit() {
        (paper.paperDisappearTween as Sequence)
            .OnComplete(() => Destroy(lineRenderer.gameObject));
    }
    private void OnPaperDestroy() {
        Destroy(lineRenderer.gameObject);
    }
}
