using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Paper : MonoBehaviour
{
    public bool isActive;
    public Collider2D coll;
    public SpriteRenderer sprite;

    [Header("Animation")]
    public float fadeTime;
    public float rotateTo;
    public float scaleTo;
    public Ease ease;

    public static EmptyDelegate OnPaperComplete;

    private void Update() {
        if (!isActive) {
            return;
        }

        Camera cam = Managers.ScenesManager?.cam;
        if (cam == null) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePoint = cam.ScreenToWorldPoint(Input.mousePosition).ToVector2();

            if (coll.bounds.Contains(mousePoint)) {
                Debug.Log("clicked paper");
                PaperCompleted();
            }
        }
    }

    public void PaperCompleted() {
        isActive = false;
        DOTween.Sequence()
            .Insert(0, sprite.DOFade(0, fadeTime).OnComplete(() => Destroy(gameObject)))
            .Insert(0, sprite.transform.DORotate(new Vector3(0, 0, rotateTo), fadeTime).SetEase(ease))
            .Insert(0, sprite.transform.DOScale(new Vector3(scaleTo, scaleTo, 1), fadeTime).SetEase(ease));

        OnPaperComplete?.Invoke();
    }
}
