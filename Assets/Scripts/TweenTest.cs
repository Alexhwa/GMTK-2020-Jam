using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Tween tween = DOTween.Sequence()
            .Insert(0, transform.DOMoveY(400, 0.5f).SetEase(Ease.OutCubic).OnComplete(() => Debug.Log("moved y")))
            .Insert(0.1f, transform.DOMoveX(400, 0.6f).SetEase(Ease.InCubic).OnComplete(() => Debug.Log("moved x")));
    }
}
