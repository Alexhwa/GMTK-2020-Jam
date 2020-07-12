using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    public void ActuallyLoadScene() {
        Managers.ScenesManager.ActuallyLoadScene();
    }
}
