using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class InterfaceManager : MonoBehaviour
{
    private int timesDialogueActivated = 0;
    private bool inDialogue;
    public GameObject dialogueUI;
    public TMP_Animated animatedText;
    public DialogueData[] levelDialogue;

    private DialogueData currentDialogue;
    private int dialogueIndex;
    public bool canExit;
    public bool nextDialogue;

    private string dialogueStartPauseTxt = "<pause=.9>";
    private Vector2 startPos;

    private void Start()
    {
        animatedText.onDialogueFinish.AddListener(() => FinishDialogue());
        startPos = dialogueUI.GetComponent<RectTransform>().anchoredPosition;
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && inDialogue)
        {
            if (canExit)
            {
                Sequence s = DOTween.Sequence();
                s.AppendInterval(.8f);
                print("Dialogue finished");
            }

            if (nextDialogue)
            {
                animatedText.ReadText(currentDialogue.conversationBlock[dialogueIndex]);
                print("Dialogue progressed");
            }
        }
    }

    public void ActivateDialogue()
    {
        animatedText.text = string.Empty;
        inDialogue = true;
        canExit = false;
        
        if(timesDialogueActivated >= levelDialogue.Length)
        {
            timesDialogueActivated = levelDialogue.Length - 1;
        }
        currentDialogue = levelDialogue[timesDialogueActivated];
        ActivateUI();
        AdjustDialogueData(currentDialogue);
        dialogueUI.transform.DOKill();
        Sequence s = DOTween.Sequence().Append(dialogueUI.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 20), .7f)).SetEase(currentDialogue.ease);
        s.target = dialogueUI.transform;
        animatedText.ReadText(currentDialogue.conversationBlock[dialogueIndex]);
        print("Dialogue activated");
        
    }
    private void ActivateUI()
    {
        dialogueUI.SetActive(true);

    }
    private void DeactivateUI()
    {
        Sequence s = DOTween.Sequence().Append(dialogueUI.GetComponent<RectTransform>().DOAnchorPos(startPos, .7f)).SetEase(Ease.OutBack);
    }

    public void FinishDialogue()
    {
        if (dialogueIndex < currentDialogue.conversationBlock.Count - 1)
        {
            dialogueIndex++;
            nextDialogue = true;
        }
        else
        {
            nextDialogue = false;
            canExit = true;
            inDialogue = false;
            timesDialogueActivated++;
            dialogueIndex = 0;
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
}