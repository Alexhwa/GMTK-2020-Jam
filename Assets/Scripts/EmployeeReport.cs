using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class EmployeeReport : MonoBehaviour
{
    public float backMoveOffset;
    public float panelMoveOffset;
    public float fadeTime;
    public Color unreachedStarColor;

    public AudioClip ambienceSound;

    [Header("Components")]
    public CanvasGroup back;
    public CanvasGroup panel;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public Image[] stars;
    public TextMeshProUGUI[] starTexts;
    public Button nextDayButton;

    private void OnEnable() {
        Managers.AudioManager.PlayLoop(Managers.AudioManager.ambienceAudio, ambienceSound, 2);
    }


    public void InitializeReport(LevelPlayer player) {
        timeText.text = "Time: " + Mathf.FloorToInt(player.levelTime / 60) + ":" + ("" + (100 + Mathf.FloorToInt(player.levelTime % 60))).Substring(1);
        scoreText.text = "Score: " + player.score;

        starTexts[0].text = "" + player.oneStarThreshold;
        starTexts[1].text = "" + player.twoStarThreshold;
        starTexts[2].text = "" + player.threeStarThreshold;

        if (player.score < player.threeStarThreshold) {
            stars[2].color = unreachedStarColor;
            starTexts[2].color = Color.white;
        }
        if (player.score < player.twoStarThreshold) {
            stars[1].color = unreachedStarColor;
            starTexts[1].color = Color.white;
        }
        if (player.score < player.oneStarThreshold) {
            stars[0].color = unreachedStarColor;
            starTexts[0].color = Color.white;

            nextDayButton.interactable = false;
        }

        back.interactable = true;
        back.blocksRaycasts = true;
        back.alpha = 0;
        (back.transform as RectTransform).anchoredPosition -= new Vector2(0, backMoveOffset);
        (panel.transform as RectTransform).anchoredPosition += new Vector2(0, panelMoveOffset);
        back.DOFade(1, fadeTime);
        (panel.transform as RectTransform).DOAnchorPosY((panel.transform as RectTransform).anchoredPosition.y - panelMoveOffset, fadeTime).SetEase(Ease.OutBack);
    }


    public void Quit() {
        Managers.AudioManager.StopLoop(Managers.AudioManager.ambienceAudio, 1);
        Managers.ScenesManager.LoadMainMenu();
    }

    public void Next() {
        Managers.AudioManager.StopLoop(Managers.AudioManager.ambienceAudio, 1);
        Managers.ScenesManager.LoadNextLevel();
    }

    public void Reset() {
        Managers.AudioManager.StopLoop(Managers.AudioManager.ambienceAudio, 1);
        Managers.ScenesManager.ReloadLevel();
    }
}
