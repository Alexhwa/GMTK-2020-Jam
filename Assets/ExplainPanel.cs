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
    private bool activated;

    public InterfaceManager interfaceManager;

    private void Start() {
        scoreText.text = "Target Score: " + levelPlayer.threeStarThreshold;
        interfaceManager.onDialogueEnd.AddListener(() => ActivateHelpBox());
    }


    public void PressedStart() {
        back.interactable = false;
        back.blocksRaycasts = false;
        back.DOKill();
        back.DOFade(0, fadeTime);

        levelPlayer.isActive = true;
    }
    public void ActivateHelpBox()
    {
        if (activated)
        {
            return;
        }
        activated = true;
        var backRect = (back.transform as RectTransform);
        backRect.anchoredPosition += new Vector2(0, backMoveOffset);
        backRect.GetComponent<CanvasGroup>().DOFade(1, 1.5f).SetEase(Ease.OutCubic);
    }
}
