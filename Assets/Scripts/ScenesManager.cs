using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public Camera cam { get; private set; }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void OnSceneLoaded(Scene scene, LoadSceneMode loadMode) {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
}
