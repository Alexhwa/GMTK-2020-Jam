using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StampPageController : MonoBehaviour
{
    public BigPaper paper;

    public void Stamped() {
        paper.PaperCompleted();
    }
}
