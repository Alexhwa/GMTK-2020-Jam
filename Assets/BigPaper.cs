using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BigPaper : LevelElement
{
    public bool isActive { get; protected set; }

    [Header("Components")]
    public Collision collision;
    public Collider2D coll { get { return collision.coll; }}
    public CanvasGroup canvasGroup;

    [Header("Animation")]
    public float fadeTime;
    public float rotateTo;
    public float scaleTo;
    public Ease ease;
    protected Tween paperDisappearTween;

    public static EmptyDelegate OnPaperComplete;



    protected virtual void PaperCompleted() {
        isActive = false;
        paperDisappearTween = DOTween.Sequence()
            .Insert(0, canvasGroup.DOFade(0, fadeTime).OnComplete(() => Destroy(gameObject)))
            .Insert(0, transform.DORotate(new Vector3(0, 0, rotateTo), fadeTime).SetEase(ease))
            .Insert(0, transform.DOScale(new Vector3(scaleTo, scaleTo, 1), fadeTime).SetEase(ease));

        OnPaperComplete?.Invoke();
    }
}
