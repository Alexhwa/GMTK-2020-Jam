using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public GameObject submitParticles;
    public GameObject floatingText;
    public AudioClip trashSound;
    public AudioClip successSound;

    public void Trash(BigPaper paper) {
        if (!paper.isCounterfeit || paper.isCompleted) {
            Managers.AudioManager.PlayOneShot(trashSound);
        }
        else {
            Instantiate(submitParticles, transform.position, Quaternion.identity);
            Instantiate(floatingText, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
            Managers.AudioManager.PlayOneShot(successSound);
        }
    }
}
