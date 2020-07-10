using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : LevelElement
{
    public Collision collision;

    private void Update() {
        if (collision.IsColliding) {
            Paper paper = collision.Collider.GetComponent<Paper>();
            paper.ShredPaper();
        }
    }
}
