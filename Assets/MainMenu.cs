using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    
    public AudioClip menuSong;

    private void OnEnable() {
        Managers.AudioManager.PlayLoop(Managers.AudioManager.bgmAudio, menuSong, 1);
    }

    public void NewGame() {
        if (Managers.ScenesManager.isTransitioning) {
            return;
        }
        
        Managers.AudioManager.StopLoop(Managers.AudioManager.bgmAudio, 1);
        Managers.ScenesManager.LoadNextLevel();
    }

    public void SelectLevel(int buildIndex) {
        if (Managers.ScenesManager.isTransitioning) {
            return;
        }
        
        Managers.AudioManager.StopLoop(Managers.AudioManager.bgmAudio, 1);
        Managers.ScenesManager.LoadScene(buildIndex);
    }
}
