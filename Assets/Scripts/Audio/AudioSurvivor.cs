using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioSurvivor : MonoBehaviour {
    public AudioClip gameOverClip;
    public AudioClip mainTrackClip;
    public AudioClip victoryClip;
    public AudioClip bossBatleClip;
    public GameObject bossGameObject;

    private AudioSource audioSource;

    private static AudioSurvivor instance = null;
    public static AudioSurvivor Instance {
        get { return instance; }
    }
    // Update is called once per frame
    void Update() {

    }

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        SelectSong();
        if (instance != null & instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }


    public void SelectSong() {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "GameOver") {
            audioSource.clip = gameOverClip;

        } else if (sceneName == "VictoryScreen") {
            audioSource.clip = victoryClip;
        } else {
            audioSource.clip = mainTrackClip;
        }
        audioSource.Play();
    }

    public void PlayBossMusic() {
        audioSource.clip = bossBatleClip;
        audioSource.Play();

    }
}