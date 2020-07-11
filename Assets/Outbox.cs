using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outbox : MonoBehaviour
{
    public GameObject submitParticles;
    public GameObject floatingText;

    public void PaperSubmitted(BigPaper paper) {
        Instantiate(submitParticles, transform.position, Quaternion.identity);
        Instantiate(floatingText, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
    }
}
