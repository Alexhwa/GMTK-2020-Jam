using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Paper : LevelElement
{
    public bool isActive { get; protected set; }
    public bool isClicked { get; protected set; }

    public bool shouldStopTime;

    private float moveSpeed;
    private Coroutine moveCoroutine;

    [Header("Components")]
    public Collision collision;
    public Collider2D coll { get { return collision.coll; }}
    public SpriteRenderer sprite;
    public Sprite timeSprite;

    [Header("Animation")]
    public float fadeTime;
    public float rotateTo;
    public float scaleTo;
    public Ease ease;
    protected Tween paperDisappearTween;

    public static EmptyDelegate OnPaperComplete;
    public static EmptyDelegate OnPaperShred;


    public void SetTimeStop() {
        shouldStopTime = true;
        sprite.sprite = timeSprite;
    }

    protected virtual void Start() {
        moveSpeed = levelRunner.paperBaseSpeed * levelRunner.paperSpeedMultiplier;
        isActive = true;
    }

    protected virtual void FixedUpdate() {
        if (moveCoroutine == null) {
            if (collision.IsColliding) {
                moveCoroutine = StartCoroutine(Move(collision.Collider.transform.position + collision.Collider.transform.right));
            }
        }

        CheckInteract();
    }

    protected IEnumerator Move(Vector3 targetPosition) {
        while (transform.position != targetPosition) {
            if (!levelRunner.isTimePaused) {
                Vector3 direction = targetPosition - transform.position;
                direction = Vector3.ClampMagnitude(direction, moveSpeed * Time.fixedDeltaTime);
                transform.position += direction;
            }

            yield return new WaitForFixedUpdate();
        }

        moveCoroutine = null;
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

        if (shouldStopTime) {
            levelRunner.DoTimePause();
        }
    }

    public virtual void ShredPaper() {
        OnPaperShred?.Invoke();
        Destroy(gameObject);
    }
}
