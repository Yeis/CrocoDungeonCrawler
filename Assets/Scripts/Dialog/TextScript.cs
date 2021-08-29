using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Map;
using System.IO;
using UnityEngine.UI;

public class TextScript : MonoBehaviour {

    public GameObject dialogUI;
    public TextMeshProUGUI textDisplay;
    private string[] easyDialogs = Directory.GetFiles("Assets/Assets/Texts/EasyTexts", "*.txt");
    private string[] mediumDialogs = Directory.GetFiles("Assets/Assets/Texts/MediumTexts", "*.txt");
    private string[] postCombatDialogs = Directory.GetFiles("Assets/Assets/Texts/PostCombatTexts", "*.txt");
    private string[] specialDialogs = Directory.GetFiles("Assets/Assets/Texts/SpecialTexts", "*.txt");
    private List<string> sentences = new List<string>();
    private int index;
    public float textSpeed = 0.02f;
    public GameObject continueButton;
    public GameObject northButton;
    public GameObject eastButton;
    public GameObject southButton;
    public GameObject westButton;
    public GameObject transitionObject;

    public GameObject mapGameLogic;
    private MapGameLogic mapController;
    private AudioSource source;

    public GameObject startCombatButton;
    private bool postCombatStatus = false;
    private TransitionController transitionController;

    //Room Difficulty 
    public GameObject roomDifficultyManagerObject;
    private RoomDifficultyManager roomDifficultyManager;
    public RoomType roomType;
    public GameObject eventRoomPotion;

    //Player logic

    public GameObject player;

    private Player playerController;

    void Awake() {
        playerController = player.GetComponent<Player>();
        mapController = mapGameLogic.GetComponent<MapGameLogic>();
        roomDifficultyManager = roomDifficultyManagerObject.GetComponent<RoomDifficultyManager>();

        transitionController = transitionObject.GetComponent<TransitionController>();
        startCombatButton.GetComponent<Button>().onClick.AddListener(startCombat);
    }

    void startCombat() {
        transitionController.startEncounter(roomDifficultyManager.difficultyLevel);
    }

    void Start() {
        readTextFile("Assets/Assets/Texts/SpecialTexts/initRoomText.txt");

        source = GetComponent<AudioSource>();
        StartCoroutine(Type());
    }

    void Update() {
        // if(!postCombatStatus){
        //     if(textDisplay.text == sentences[index]) {
        //         if(index == sentences.Count - 1) {
        //             startCombatButton.SetActive(true);
        //         } else {
        //             continueButton.SetActive(true);
        //         }
        //     }
        // } else {

        // }

        if (textDisplay.text == sentences[index]) {
            if (postCombatStatus) {
                northButton.SetActive(mapController.characterPositionNode.topRoom != null);
                eastButton.SetActive(mapController.characterPositionNode.rightRoom != null);
                westButton.SetActive(mapController.characterPositionNode.leftRoom != null);
                southButton.SetActive(mapController.characterPositionNode.bottomRoom != null);
            } else if (index == sentences.Count - 1) {
                startCombatButton.SetActive(true);
            } else {
                continueButton.SetActive(true);
            }
        }
    }

    private void readTextFile(string file_path) {
        StreamReader input_stream = new StreamReader(file_path);

        while (!input_stream.EndOfStream) {
            string input_line = input_stream.ReadLine();
            sentences.Add(input_line);
        }

        input_stream.Close();
    }

    public void NextSentence() {
        source.Play();
        continueButton.SetActive(false);

        if (index < sentences.Count - 1) {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        } else {
            textDisplay.text = "";
            continueButton.SetActive(false);
        }
    }

    public void NextRoom() {
        eventRoomPotion.SetActive(false);
        postCombatStatus = false;
        northButton.SetActive(false);
        eastButton.SetActive(false);
        westButton.SetActive(false);
        southButton.SetActive(false);
        textDisplay.text = "";
        index = 0;

        if (mapController.characterPositionNode.characterVisited && !mapController.characterPositionNode.isBossInRoom) {
            sentences.Clear();
            readTextFile("Assets/Assets/Texts/SpecialTexts/visitedRoomDialog.txt");
            postCombatStatus = true;
            StartCoroutine(Type());
        } else if (mapController.characterPositionNode.isBossInRoom) {
            sentences.Clear();
            //TODO: check if room is open or closed

            if(roomDifficultyManager.roomCounter >= 2){
                readTextFile("Assets/Assets/Texts/SpecialTexts/bossRoomOpenText.txt");
                StartCoroutine(Type());
            } else {
                readTextFile("Assets/Assets/Texts/SpecialTexts/bossRoomClosedText.txt");
                postCombatStatus = true;
                StartCoroutine(Type());
            }

            
        } else {
            switch (roomDifficultyManager.difficultyLevel) {
                case RoomType.easyRoom:
                    sentences.Clear();
                    readRandomFrom(easyDialogs);
                    StartCoroutine(Type());
                    return;

                case RoomType.mediumRoom:
                    sentences.Clear();
                    readRandomFrom(mediumDialogs);
                    StartCoroutine(Type());
                    return;

                case RoomType.eventRoom:
                    sentences.Clear();
                    readTextFile("Assets/Assets/Texts/SpecialTexts/eventRoomText.txt");
                    postCombatStatus = true;
                    StartCoroutine(Type());
                    eventRoomPotion.SetActive(true);
                    playerController.AddHealth(100 - playerController.healthPoints);
                    return;

                default:
                    return;
            }
        }

        //startCombatButton.SetActive(true);

    }

    private void readRandomFrom(string[] dialogList) {
        int randy = Random.Range(0, 5);
        sentences.Clear();
        readTextFile(dialogList[randy]);
    }

    private void mockCombat() {
        StartCoroutine(Type());
        startCombatButton.SetActive(false);
    }

    public void prepCloseUI() {
        // northButton.SetActive(mapController.characterPositionNode.topRoom != null);
        // eastButton.SetActive(mapController.characterPositionNode.rightRoom != null);
        // westButton.SetActive(mapController.characterPositionNode.leftRoom != null);
        // southButton.SetActive(mapController.characterPositionNode.bottomRoom != null);
        startCombatButton.SetActive(false);
        dialogUI.SetActive(false);
    }

    public void reopenUI() {
        startCombatButton.SetActive(false);
        dialogUI.SetActive(true);
        postCombatStatus = true;
        sentences.Clear();
        textDisplay.text = "";
        index = 0;
        readRandomFrom(postCombatDialogs);
        StartCoroutine(Type());
    }

    IEnumerator Type() {
        foreach (char letter in sentences[index].ToCharArray()) {
            textDisplay.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
