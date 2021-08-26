using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Map;
using System.IO;

public class TextScript : MonoBehaviour {

    public TextMeshProUGUI textDisplay;
    private List<string> sentences = new List<string>();
    private int index;
    public float textSpeed = 0.02f;
    public GameObject continueButton;
    public GameObject northButton;
    public GameObject eastButton;
    public GameObject southButton;
    public GameObject westButton;

    public GameObject mapGameLogic;
    private MapGameLogic mapController;
    private AudioSource source;

    public GameObject tempCombatButton;

    void Awake() {
        mapController = mapGameLogic.GetComponent<MapGameLogic>();
    }

    void Start() {
        // TODO: Populate string[] with room dialog

        readTextFile("Assets/Assets/Texts/testDialogFile.txt");

        source = GetComponent<AudioSource>();
        StartCoroutine(Type());
    }

    void Update() {

        if(textDisplay.text == sentences[index]) {
            if(index == sentences.Count - 1) {
                northButton.SetActive(mapController.characterPositionNode.topRoom != null);
                eastButton.SetActive(mapController.characterPositionNode.rightRoom != null);
                westButton.SetActive(mapController.characterPositionNode.leftRoom != null);
                southButton.SetActive(mapController.characterPositionNode.bottomRoom != null);
            } else {
                continueButton.SetActive(true);
            }
        }
    }

    private void readTextFile(string file_path) {
        StreamReader input_stream = new StreamReader(file_path);

        while(!input_stream.EndOfStream) {
            string input_line = input_stream.ReadLine();
            sentences.Add(input_line);
        }

        input_stream.Close();  
    }

    public void NextSentence() {
        source.Play();
        continueButton.SetActive(false);

        if(index < sentences.Count - 1) {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        } else { 
            textDisplay.text = "";
            continueButton.SetActive(false);
        }
    }

    public void NextRoom() {
        northButton.SetActive(false);
        eastButton.SetActive(false);
        westButton.SetActive(false);
        southButton.SetActive(false);
        textDisplay.text = "";
        index = 0;

        if(mapController.characterPositionNode.characterVisited) {
            sentences.Clear();
            readTextFile("Assets/Assets/Texts/visitedRoomDialog.txt");
        } else {
            sentences.Clear();
            //TODO: Populate sentences list with appropiate room
            //eg. readTextFile(characterPositionNode.descriptionDialogPath);
            sentences.Add("ok this is a new room but I dont know wtf to say rn stg fr fr lol");
        }

        // mocking combat
        tempCombatButton.SetActive(true);
    }

    public void mockCombat() {
        StartCoroutine(Type());
        tempCombatButton.SetActive(false);
    }

    IEnumerator Type() {
        foreach(char letter in sentences[index].ToCharArray()) {
            textDisplay.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
