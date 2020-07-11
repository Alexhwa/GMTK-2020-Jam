using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TimerDisplay : MonoBehaviour
{
    public LevelPlayer levelPlayer;
    public Slider slider;
    public Image fillImage;

    [Header("Color")]
    public Color baseColor;
    public float fullHue;
    public float emptyHue;

    private void Update() {
        slider.value = levelPlayer.TimeRemaining / levelPlayer.levelTime;
        fillImage.color = baseColor.WithHue(Mathf.Lerp(emptyHue / 360f, fullHue / 360f, slider.value));
    }
}
