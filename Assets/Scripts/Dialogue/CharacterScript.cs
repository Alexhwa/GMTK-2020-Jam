using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Controls the sprite of the character
//Has actions for character emote changes and movements
public class CharacterScript : MonoBehaviour
{
    public Image nameBubble;
    public TMPro.TMP_Text nameText;

    public Image charSprite;
    public InterfaceManager interfaceManager;

    private CharacterData currentChar;
    private void Start()
    {
        interfaceManager.animatedText.onEmotionChange.AddListener(emotion => SetEmotion(emotion));
    }

    public void SetCharacterValsInUI(CharacterData charData)
    {
        currentChar = charData;
        nameBubble.color = charData.characterColor;
        nameText.text = charData.characterName;
        nameText.color = charData.characterNameColor;
        charSprite.sprite = charData.neutralSprite;
    }
    private void SetEmotion(Emotion emotion)
    {
        switch (emotion)
        {
            case Emotion.neutral:
                charSprite.sprite = currentChar.neutralSprite;
                break;
            case Emotion.sad:
                charSprite.sprite = currentChar.worriedSprite;
                break;
            case Emotion.suprised:
                charSprite.sprite = currentChar.suprisedSprite;
                break;
            default:
                charSprite.sprite = currentChar.neutralSprite;
                break;
        }
    }
}
