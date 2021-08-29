using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 2.5f;
    public string GameScene;
    public AudioSurvivor audioSurvivor;

    void Start() {
        audioSurvivor = GameObject.FindGameObjectWithTag("AudioSurvivor").GetComponent<AudioSurvivor>();
        audioSurvivor.SelectSong();
    }

    public void CouroutineStart()
    {
        StartCoroutine("LoadLevel");
    }

    IEnumerator LoadLevel()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(GameScene);
        
    }
}
