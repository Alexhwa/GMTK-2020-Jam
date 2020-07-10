using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkbox : MonoBehaviour
{
    public SpriteRenderer checkSr;
    public Collider2D hitbox;

    public bool isChecked;

    public void OnChecked() {
        isChecked = true;
        checkSr.color = checkSr.color.WithAlpha(0);
    }
}
