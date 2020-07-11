using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Color characterColor;
    public Color characterNameColor;
    public DialogueData dialogue;
}
