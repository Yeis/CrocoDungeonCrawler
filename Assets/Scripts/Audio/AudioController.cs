using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    //Clips
    public AudioClip playerAttackClip;
    public AudioClip playerTakingDamageClip;
    public AudioClip crocoAttackClip;
    public AudioClip crocoDyingClip;
    public AudioClip continueClip;
    public AudioClip startEncounterClip;
    public AudioClip pickItemClip;
    public AudioClip brokenRelicClip;
    public AudioClip clickButtonClip;
    public AudioClip winEncounterClip;


    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPlayerAttackClip() {
        audioSource.PlayOneShot(playerAttackClip, 0.5f);
    }

    public void PlayPlayerTakingDamageClip() {
        audioSource.PlayOneShot(playerTakingDamageClip, 0.5f);
    }

    public void PlayCrocoAttackClip() {
        audioSource.PlayOneShot(crocoAttackClip, 0.5f);
    }

    public void PlayCrocoDyingClip() {
        audioSource.PlayOneShot(crocoDyingClip, 0.5f);
    }

    public void PlayPickItemClip() {
        audioSource.PlayOneShot(pickItemClip, 0.5f);
    }

    public void PlayContinueClip() {
        audioSource.PlayOneShot(continueClip, 0.5f);
    }

    public void PlayBrokenRelicClip() {
        audioSource.PlayOneShot(brokenRelicClip, 0.5f);
    }

    public void PlayClickButtonClip() {
        audioSource.PlayOneShot(clickButtonClip, 0.5f);
    }

    public void PlayWinEncounterClip() {
        audioSource.PlayOneShot(winEncounterClip, 0.5f);
    }

    public void PlayStartEncounterClip() {
        audioSource.PlayOneShot(startEncounterClip, 0.5f);
    }
}
