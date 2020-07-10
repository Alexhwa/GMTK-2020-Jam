using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Paper : MonoBehaviour
{
    public bool isActive { get; protected set; }
    public bool isClicked { get; protected set; }

    public float moveSpeed;
    [HideInInspector] public Vector3 moveDirection;

    [Header("Components")]
    public Collider2D coll;
    public SpriteRenderer sprite;

    [Header("Animation")]
    public float fadeTime;
    public float rotateTo;
    public float scaleTo;
    public Ease ease;
    protected Tween paperDisappearTween;

    public static EmptyDelegate OnPaperComplete;


    protected virtual void Start() {
        isActive = true;
    }

    protected virtual void Update() {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        CheckInteract();
    }

    protected void CheckInteract() {
        if (!isActive) {
            return;
        }

        Vector2 mousePoint = Managers.ScenesManager?.cam?.ScreenToWorldPoint(Input.mousePosition).ToVector2() ?? new Vector2(-1000, -1000);

        if (!isClicked && Input.GetMouseButtonDown(0) && coll.bounds.Contains(mousePoint)) {
            OnClicked(mousePoint);
        }

        if (isClicked && Input.GetMouseButton(0)) {
            OnHeld(mousePoint);
        }

        if (isClicked && Input.GetMouseButtonUp(0)) {
            OnUnclicked(mousePoint);
        }
    }

    protected virtual void OnClicked(Vector2 mousePoint) {
        isClicked = true;
    }

    protected virtual void OnHeld(Vector2 mousePoint) {

    }

    protected virtual void OnUnclicked(Vector2 mousePoint) {
        isClicked = false;
    }

    protected virtual void PaperCompleted() {
        isActive = false;
        paperDisappearTween = DOTween.Sequence()
            .Insert(0, sprite.DOFade(0, fadeTime).OnComplete(() => Destroy(gameObject)))
            .Insert(0, sprite.transform.DORotate(new Vector3(0, 0, rotateTo), fadeTime).SetEase(ease))
            .Insert(0, sprite.transform.DOScale(new Vector3(scaleTo, scaleTo, 1), fadeTime).SetEase(ease));

        OnPaperComplete?.Invoke();
    }

    public virtual void ShredPaper() {
        Debug.Log("oh no");
        Destroy(gameObject);
    }
}
