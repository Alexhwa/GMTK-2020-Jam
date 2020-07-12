using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public Camera cam { get; private set; }
    public bool isTransitioning;
    public int sceneBuildIndex { get; private set; }

    public Animator faderAnimator;

    public int numLevels;
    public int firstLevelBuildIndex;


    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void OnSceneLoaded(Scene scene, LoadSceneMode loadMode) {
        if (isTransitioning) {
            faderAnimator.SetTrigger("FadeIn");
        }
        isTransitioning = false;
    
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }


    public void LoadScene(int buildIndex) {
        if (isTransitioning) {
            return;
        }
        isTransitioning = true;

        sceneBuildIndex = buildIndex;

        faderAnimator.SetTrigger("FadeOut");
    }

    public void LoadNextLevel() {
        if (sceneBuildIndex + 1 >= firstLevelBuildIndex + numLevels) {
            LoadScene(0);
        }
        else {
            LoadScene(sceneBuildIndex + 1);
        }
    }

    public void ReloadLevel() {
        LoadScene(sceneBuildIndex);
    }

    public void LoadMainMenu() {
        LoadScene(0);
    }

    public void ActuallyLoadScene() {
        SceneManager.LoadScene(sceneBuildIndex);
    }
}
