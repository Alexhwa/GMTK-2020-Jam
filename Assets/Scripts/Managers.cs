using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers instance;

    public AudioManager audioManager;
    public static AudioManager AudioManager => instance?.audioManager;

    public ScenesManager scenesManager;
    public static ScenesManager ScenesManager => instance?.scenesManager;
    
    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
