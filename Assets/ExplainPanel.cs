using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ExplainPanel : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public LevelPlayer levelPlayer;

    [Header("Animation")]
    public CanvasGroup back;
    public float backMoveOffset;
    public float fadeTime;

    private void Start() {
        (back.transform as RectTransform).anchoredPosition += new Vector2(0, backMoveOffset);
        scoreText.text = "Target Score: " + levelPlayer.threeStarThreshold;
    }


    public void PressedStart() {
        back.interactable = false;
        back.blocksRaycasts = false;
        back.DOFade(0, fadeTime);

        levelPlayer.isActive = true;
    }
}
