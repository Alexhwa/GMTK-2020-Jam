using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FloatingText : MonoBehaviour
{
    public float floatSpeed;
    public float lifeTime;
    public float fadeTime;
    public TMPro.TMP_Text text;

    public bool floatUp = true;

    private float aliveTimer;
    private Tween fadeTween;

    private void Start() {
        Color color = text.color;
        text.color = Color.white;
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        text.DOColor(color, 0.3f);
        transform.DOScale(new Vector3(1, 1, 1), 0.3f).SetEase(Ease.OutCubic);
    }

    private void FixedUpdate() {
        transform.position += Vector3.up * floatSpeed * Time.fixedDeltaTime * (floatUp ? 1 : -1);

        aliveTimer += Time.fixedDeltaTime;
        if (fadeTween == null && aliveTimer >= lifeTime - fadeTime - 0.1f) {
            fadeTween = text.DOFade(0, fadeTime);
        }

        if (aliveTimer >= lifeTime) {
            Destroy(gameObject);
        }
    }
}
