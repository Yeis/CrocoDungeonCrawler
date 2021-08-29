using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioSurvivor : MonoBehaviour
{
    public AudioClip gameOverClip;
    public AudioClip mainTrackClip;
    public AudioClip victoryClip;
    public AudioClip bossBatleClip;

    private AudioSource audioSource;

    private static AudioSurvivor instance = null;
    public static AudioSurvivor Instance
    {
        get { return instance; }
    }
    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        SelectSong();
        if (instance != null & instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }


    public void SelectSong() {
        string sceneName = SceneManager.GetActiveScene().name;
        GameObject bossGameObject = GameObject.FindGameObjectWithTag("Boss");
        if(sceneName == "GameOver") {
            audioSource.clip = gameOverClip;

        } 
        else if(sceneName == "VictoryScreen") {
            audioSource.clip = victoryClip;

        }
         else if(bossGameObject != null && bossGameObject.activeSelf) {
            audioSource.clip = bossBatleClip;

        }
        else {
            audioSource.clip = mainTrackClip;
        }
        audioSource.Play();
    }
}