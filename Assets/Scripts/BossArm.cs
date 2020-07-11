using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArm : MonoBehaviour
{
    private Animator anim;
    public Object armSwingAnim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void SwingArm()
    {
        anim.Play(armSwingAnim.name);
    }
}
