﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BigPaper : LevelElement
{
    public bool isActive { get; protected set; }
    public bool isCompleted { get; private set; }
    public bool isSubmitted { get; private set; }

    [Header("Components")]
    public Collision collision;
    public Collider2D coll { get { return collision.coll; }}
    public CanvasGroup canvasGroup;
    public Canvas canvas;

    [Header("Animation")]
    public float fadeTime;
    public float rotateTo;
    public float scaleTo;
    public Ease ease;
    public Tween paperDisappearTween;

    private Vector2 dragOffset;

    public static EmptyDelegate OnPaperComplete;
    public static EmptyDelegate OnPaperSubmit;
    public static EmptyDelegate OnPaperTrashed;

    
    private void Start() {
        isActive = true;
    }

    public void BeginDrag() {
        transform.DOKill();
        transform.DORotate(new Vector3(0, 0, 0), 0.3f).SetEase(Ease.InOutCubic);
        dragOffset = GetMousePoint() - transform.position.ToVector2();
    }

    public void Drag() {
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
    }


    public virtual void PaperCompleted() {
        isCompleted = true;
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

        OnPaperSubmit?.Invoke();
    }

    public void PaperTrashed() {
        isActive = false;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        paperDisappearTween = DOTween.Sequence()
            .Insert(0, canvasGroup.DOFade(0, fadeTime).OnComplete(() => Destroy(gameObject)))
            .Insert(0, transform.DORotate(new Vector3(0, 0, rotateTo), fadeTime).SetEase(ease))
            .Insert(0, transform.DOScale(new Vector3(scaleTo, scaleTo, 1), fadeTime).SetEase(ease));

        OnPaperTrashed?.Invoke();
    }


    public Vector2 GetMousePoint() => canvas.worldCamera.ScreenToWorldPoint(Input.mousePosition).ToVector2();
}
