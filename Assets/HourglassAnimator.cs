using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HourglassAnimator : MonoBehaviour
{
    public float rotateDelay;
    public float rotateTime;
    private float rotateTimer;
    private float targetZRot;

    private void Update() {
        rotateTimer += Time.deltaTime;

        if (rotateTimer >= rotateDelay) {
            rotateTimer -= rotateDelay;
            targetZRot += 180;
            transform.DORotate(new Vector3(0, 0, targetZRot - 0.5f), rotateTime)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() => transform.rotation = Quaternion.Euler(0, 0, targetZRot));
        }
    }
}
