using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextScript : MonoBehaviour {

    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float textSpeed = 0.02f;
    public GameObject continueButton;
    private AudioSource source;

    void Start() {
        source = GetComponent<AudioSource>();
        StartCoroutine(Type());
    }

    void Update() {
        if(textDisplay.text == sentences[index]) {
            continueButton.SetActive(true);
        }
    }

    public void NextSentence() {
        source.Play();
        continueButton.SetActive(false);

        if(index < sentences.Length - 1) {

            if(index == sentences.Length - 2) {
                continueButton.GetComponent<TextMeshProUGUI>().text = "     END";
            }

            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        } else { 
            textDisplay.text = "";
            continueButton.SetActive(false);
        }
    }

    IEnumerator Type() {
        foreach(char letter in sentences[index].ToCharArray()) {
            textDisplay.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
