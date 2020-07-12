using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class InterfaceManager : MonoBehaviour
{
    private int timesDialogueActivated = 0;

    public GameObject dialogueUI;
    public GameObject dimScreen; 

    public TMP_Animated animatedText;
    public DialogueData[] levelDialogue;

    private DialogueData currentDialogue;
    private int dialogueIndex;
    public bool canExit;
    public bool nextDialogue;
    private bool inDialogue;
    private bool dialogueFinished;

    private string dialogueStartPauseTxt = "<pause=.9>";
    private Vector2 startPos;

    [System.Serializable] public class DialogueEndEvent : UnityEvent { }
    public DialogueEndEvent onDialogueEnd;

    private AudioManager audioManager;

    private void Start()
    {
        animatedText.onDialogueFinish.AddListener(() => FinishDialogue());
        animatedText.onTextReveal.AddListener(c => PlayVoice(c));
        startPos = dialogueUI.GetComponent<RectTransform>().anchoredPosition;
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && inDialogue && dialogueFinished)
        {
            if (canExit)
            {
                Sequence s = DOTween.Sequence();
                s.AppendInterval(.8f);
                print("Dialogue ");
            }

            if (nextDialogue)
            {
                animatedText.ReadText(currentDialogue.conversationBlock[dialogueIndex]);
                dialogueFinished = false;
            }
        }
        
    }

    public void ActivateDialogue()
    {
        animatedText.text = string.Empty;
        inDialogue = true;
        canExit = false;
        dialogueFinished = false;
        if (timesDialogueActivated >= levelDialogue.Length)
        {
            timesDialogueActivated = levelDialogue.Length - 1;
        }
        currentDialogue = levelDialogue[timesDialogueActivated];
        ActivateUI();
        AdjustDialogueData(currentDialogue);
        dialogueUI.transform.DOKill();
        Sequence s = DOTween.Sequence().Append(dialogueUI.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 20), 1f).SetEase(currentDialogue.ease));
        s.target = dialogueUI.transform;
        animatedText.ReadText(currentDialogue.conversationBlock[dialogueIndex]);
        
    }
    private void ActivateUI()
    {
        dialogueUI.SetActive(true);
        dimScreen.GetComponent<CanvasGroup>().DOFade(1, .7f);
    }
    private void DeactivateUI()
    {
        Sequence s = DOTween.Sequence().Append(dialogueUI.GetComponent<RectTransform>().DOAnchorPos(startPos, 1f)).SetEase(Ease.InCubic);
        s.target = dialogueUI.transform;
        dimScreen.GetComponent<CanvasGroup>().DOFade(0, .7f);

    }

    public void FinishDialogue()
    {
        if (dialogueIndex < currentDialogue.conversationBlock.Count - 1)
        {
            dialogueIndex++;
            nextDialogue = true;
            dialogueFinished = true;
        }
        else
        {
            nextDialogue = false;
            canExit = true;
            inDialogue = false;
            timesDialogueActivated++;
            dialogueIndex = 0;
            onDialogueEnd.Invoke();
            DeactivateUI();
        }
    }
    private void AdjustDialogueData(DialogueData data)
    {
        var convoBlock = data.conversationBlock;
        if (convoBlock.Count <= 0)
        {
            convoBlock.Add(string.Empty);
        }
        if(convoBlock[convoBlock.Count - 1] != string.Empty)
        {
            convoBlock.Add(string.Empty);
        }
        if (convoBlock[0].Length >= 10)
        {
            if (!convoBlock[0].Substring(0, 10).Equals(dialogueStartPauseTxt))
            {
                convoBlock[0] = dialogueStartPauseTxt + convoBlock[0];
            }
        }
        else
        {
            convoBlock[0] = dialogueStartPauseTxt + convoBlock[0];
        }
    }
    private void PlayVoice(char c)
    {
        if(currentDialogue == null)
        {
            return;
        }
        string punctuation = ",.:!?/-=;'";
        if (punctuation.Contains(c.ToString()))
        {
            //currentDialogue.character.punc
        }
    }
}