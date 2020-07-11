using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    public RectTransform displayPanel;
    public LevelPlayer levelPlayer;
    public Slider slider;
    public Image fillImage;
    public TextMeshProUGUI bigText;

    public float panelOffset;
    private Vector2 panelBasePosition;

    [Header("Color")]
    public Color baseColor;
    public float fullHue;
    public float emptyHue;

    private float previousTimeDisplay;

    private void Start() {
        panelBasePosition = displayPanel.anchoredPosition;
        displayPanel.anchoredPosition -= new Vector2(0, panelOffset);

        previousTimeDisplay = -100;
        bigText.color = bigText.color.WithAlpha(0);
    }

    private void Update() {
        slider.value = levelPlayer.TimeRemaining / levelPlayer.levelTime;
        fillImage.color = baseColor.WithHue(Mathf.Lerp(emptyHue / 360f, fullHue / 360f, slider.value));

        // do position of panel
        displayPanel.anchoredPosition = Vector2.Lerp(
            displayPanel.anchoredPosition,
            levelPlayer.levelTimer > 0 && levelPlayer.TimeRemaining > 0 ? panelBasePosition : panelBasePosition - new Vector2(0, panelOffset),
            0.1f
        );

        // check all displays
        if (previousTimeDisplay < -2 && levelPlayer.levelTimer >= -2) {
            Display("Ready...");
            previousTimeDisplay = -2;
        }
        else if (previousTimeDisplay < 0 && levelPlayer.levelTimer >= 0) {
            Display("Go !");
            previousTimeDisplay = 100;
        }
        else if (levelPlayer.TimeRemaining <= 5 && previousTimeDisplay > Mathf.Ceil(levelPlayer.TimeRemaining)) {
            previousTimeDisplay = Mathf.Ceil(levelPlayer.TimeRemaining);

            if (previousTimeDisplay == 0) {
                Display("Finished !");
            }
            else {
                Display("" + (int)previousTimeDisplay);
            }
        }
    }


    private void Display(string text) {
        bigText.DOKill();

        bigText.text = text;
        bigText.color = bigText.color.WithAlpha(0);
        bigText.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        Tween displayTween = DOTween.Sequence()
            .Insert(0, bigText.transform.DOScale(new Vector3(1, 1, 1), 0.3f).SetEase(Ease.OutBack))
            .Insert(0, DOTween.To(() => bigText.color.a, alpha => bigText.color = bigText.color.WithAlpha(alpha), 1, 0.3f))
            .Insert(0.7f, DOTween.To(() => bigText.color.a, alpha => bigText.color = bigText.color.WithAlpha(alpha), 0, 0.3f));
        displayTween.target = bigText;
    }
}
