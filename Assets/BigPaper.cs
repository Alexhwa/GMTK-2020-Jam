﻿using System.Collections;
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

    public float lifeTime;
    private float aliveTimer;

    [HideInInspector] public PaperSpawner paperSpawner;

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
    public static EmptyDelegate OnPaperSubmitted;

    
    private void Start() {
        isActive = true;
    }

    public void BeginDrag() {
        SortToTop();

        transform.DOKill();
        transform.DORotate(new Vector3(0, 0, 0), 0.3f).SetEase(Ease.InOutCubic);
        dragOffset = GetMousePoint() - transform.position.ToVector2();
    }

    public void Drag() {
        SortToTop();

        transform.position = GetMousePoint() - dragOffset;
    }

    public void EndDrag() {
        if (isActive && collision.IsColliding && collision.Collider.CompareTag("Out") && isCompleted) {
            PaperSubmitted();
        }
    }


    private void Update() {
        if (isActive && collision.IsColliding && collision.Collider.CompareTag("Trash")) {
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

        OnComplete?.Invoke();
    }

    public void PaperSubmitted() {
        isActive = false;
        isSubmitted = true;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        paperDisappearTween = DOTween.Sequence()
            .Insert(0, canvasGroup.DOFade(0, fadeTime).OnComplete(() => Destroy(gameObject)))
            .Insert(0, transform.DORotate(new Vector3(0, 0, rotateTo), fadeTime).SetEase(ease))
            .Insert(0, transform.DOScale(new Vector3(scaleTo, scaleTo, 1), fadeTime).SetEase(ease));

        OnSubmit?.Invoke();
        OnPaperSubmitted?.Invoke();
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
