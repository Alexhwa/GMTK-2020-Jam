using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Dialogue Data", menuName = "Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [TextArea(4, 4)]
    public List<string> conversationBlock;
    public CharacterData character;
    [Tooltip("What level this dialogue block will appear on starting on 0.")]
    public Ease ease;
}
