using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class BigPaper : MonoBehaviour
{
    public bool isActive { get; protected set; }
    public bool isCompleted { get; private set; }
    public bool isSubmitted { get; private set; }

    public bool isCounterfeit;

    public float lifeTime;
    private float aliveTimer;

    public AudioClip completedSound;

    [HideInInspector] public PaperSpawner paperSpawner;
    public Collider2D table;

    [Header("Components")]
    public Collision collision;
    public Collider2D coll { get { return collision.coll; }}
    public CanvasGroup canvasGroup;
    public Canvas canvas;
    public RectTransform sheenRect;
    public Image completeBorder;

    [Header("Animation")]
    public float completeTime;
    public float sheenMoveAmount;
    public float fadeTime;
    public float rotateTo;
    public float scaleTo;
    public Ease ease;
    public Tween paperDisappearTween;

    private Vector2 dragOffset;

    public UnityEvent OnSortToTop;
    public UnityEvent OnComplete;
    public UnityEvent OnSubmit;
    public UnityEvent OnTrashed;
    public UnityEvent OnDestroyed;

    public delegate void PaperDelegate(BigPaper paper);
    public static PaperDelegate OnPaperTrashed;
    public static PaperDelegate OnPaperSubmitted;

    
    private void Start() {
        isActive = true;
    }

    public void BeginDrag() {
        if (!isActive) {
            return;
        }

        SortToTop();

        transform.DOKill();
        transform.DORotate(new Vector3(0, 0, 0), 0.3f).SetEase(Ease.InOutCubic);
        dragOffset = GetMousePoint() - transform.position.ToVector2();
    }

    public void Drag() {
        if (!isActive) {
            return;
        }

        SortToTop();

        transform.position = Utilities.ClampInRect(GetMousePoint() - dragOffset, new Rect(table.bounds.min, table.bounds.size));
    }

    public void EndDrag() {
        if (!isActive) {
            return;
        }

        transform.position = Utilities.ClampInRect(GetMousePoint() - dragOffset, new Rect(table.bounds.min, table.bounds.size));

        if (collision.IsColliding && collision.Collider.CompareTag("Out") && isCompleted) {
            Outbox outbox = collision.Collider.GetComponent<Outbox>();
            if (outbox.isActive) {
                outbox.PaperSubmitted(this);
                PaperSubmitted(outbox.transform.right);
            }
        }
    }


    private void Update() {
        if (isActive && collision.IsColliding && collision.Collider.CompareTag("Trash")) {
            collision.Collider.GetComponent<TrashBin>().Trash(this);
            PaperTrashed();
        }

        aliveTimer += Time.deltaTime;
        if (aliveTimer >= lifeTime) {
            PaperDestroy();
        }

        if (lifeTime - aliveTimer < 1f) {
            canvasGroup.alpha = (lifeTime - aliveTimer) % 0.1f < 0.05f ? 1 : 0.7f;
        }
        else if (isActive) {
            canvasGroup.alpha = 1;
        }
    }


    public virtual void PaperCompleted() {
        isCompleted = true;

        sheenRect.DOAnchorPosY(sheenRect.anchoredPosition.y - sheenMoveAmount, completeTime).SetEase(Ease.InOutQuad);
        completeBorder.DOFade(1, completeTime).SetEase(Ease.InOutQuad);

        Managers.AudioManager.PlayOneShot(completedSound);

        OnComplete?.Invoke();
    }

    public void PaperSubmitted(Vector3 dir) {
        isActive = false;
        isSubmitted = true;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        paperDisappearTween = DOTween.Sequence()
            .Insert(0, transform.DOMove(transform.position + dir * 7f, fadeTime).SetEase(Ease.InBack).OnComplete(() => Destroy(gameObject)));

        OnSubmit?.Invoke();
        OnPaperSubmitted?.Invoke(this);
    }

    public void PaperTrashed() {
        isActive = false;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        paperDisappearTween = DOTween.Sequence()
            .Insert(0, canvasGroup.DOFade(0, fadeTime).OnComplete(() => Destroy(gameObject)))
            .Insert(0, transform.DORotate(new Vector3(0, 0, rotateTo), fadeTime).SetEase(ease))
            .Insert(0, transform.DOScale(new Vector3(scaleTo, scaleTo, 1), fadeTime).SetEase(ease));

        OnTrashed?.Invoke();
        OnPaperTrashed?.Invoke(this);
    }

    public void PaperDestroy() {
        isActive = false;

        OnDestroyed?.Invoke();
        Destroy(gameObject);
    }


    public void SortToTop() {
        if (paperSpawner != null && paperSpawner.SortToTop(this)) {
            OnSortToTop?.Invoke();

            aliveTimer = 0;
        }
    }

    public Vector2 GetMousePoint() => canvas.worldCamera.ScreenToWorldPoint(Input.mousePosition).ToVector2();
}
