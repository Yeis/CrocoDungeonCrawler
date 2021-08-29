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

    private static string[] easyDialog1 = { "You can feel a chaotic force getting closer.", "A strong presence, similar to the one emanating from your relic", "You hear steps coming close.", "There's more of them." };
    private static string[] easyDialog2 = { "You don't really know why you press on forward", "Your relic starts acting strange, as if something's coming close.", "You hear steps coming close.", "There's more of them." };
    private static string[] easyDialog3 = { "You can feel a chaotic force getting closer.", "A strong presence, similar to the one emanating from your relic", "You don't even realize before it's too late.", "There's more of them." };

    private string[][] easyDialogs = { easyDialog1, easyDialog2, easyDialog3 };

    private static string[] mediumDialog1 = { "You realize they will never stop coming.", "It seems you're not the only one allured by the relic.", "You prepare for battle." };
    private static string[] mediumDialog2 = { "Your relic grows stronger, but something's wrong", "Its strength seems affected by something near.", "Prepare for another fight." };
    private static string[] mediumDialog3 = { "Its getting harder and harder to remain determined.", "But the relic's call pulls you forward.", "Satisfy its wishes, and great rewards will come." };
    private static string[] mediumDialog4 = { "You feel watched.", "It seems you've gathered the attention of a mysterious force.", "You hold the relic and prepare for combat" };
    private static string[] mediumDialog5 = { "You hear a distant laughter.", "With it, you feel a strong presence worthy of a whole kingdom.", "But what is royalty, for curious adventurer?" };

    private string[][] mediumDialogs = { mediumDialog1, mediumDialog2, mediumDialog3, mediumDialog4, mediumDialog5 };

    private static string[] postCombatDialog1 = { "You wipe off your sweat from your forehead, and press on." };
    private static string[] postCombatDialog2 = { "There's no time for panicking, you must continue forward." };
    private static string[] postCombatDialog3 = { "You can feel the relic getting more unstable." };
    private static string[] postCombatDialog4 = { "You look behind you, and then forward. The relic's call continues." };
    private static string[] postCombatDialog5 = { "As your foes flee, you can feel the relic getting stronger." };
    private static string[] postCombatDialog6 = { "You supress your fear with this last victory, and press on." };
    private static string[] postCombatDialog7 = { "As the last soldier falls, you grasp the relic and feel its power." };
    private static string[] postCombatDialog8 = { "You hear grunts beyond the cave's corridors, but continue forward." };
    private static string[] postCombatDialog9 = { "Without stutter, you continue on deeper into the cave." };
    private static string[] postCombatDialog10 = { "As you step onward, you feel the relic's power growing stronger." };

    private string[][] postCombatDialogs = { postCombatDialog1, postCombatDialog2, postCombatDialog3, postCombatDialog4, postCombatDialog5, postCombatDialog6, postCombatDialog7, postCombatDialog8, postCombatDialog9, postCombatDialog10 };

    private static string[] bossCloseDialog = { "You're not ready to access this room. You must defeat more enemies." };
    private static string[] bossOpenDialog = { "You\'ve proven yourself strong enough.", "You can feel the relic\'s call beyond this last room.", "You gather your remaining strength and press forward one last time." };
    private static string[] eventRoomDialog = { "You find an old health potion laying on the ground. Hp restored." };
    private static string[] initRoomDialog = { "The relic's call has ordered you into this dark cave.", "As you hold it, you pay attention at its 4 gems.", "Four symbols appear in its crystals.", "Replicate these runes to embrace the relic's strength.", "You hear whispers, voices of past challengers.", "Before you can collect your thoughts, you see them.", "An army worthy of a mad king." };
    private static string[] visitedRoomDialog = { "You've already visited this room." };

    private string[][] specialDialogs = { bossCloseDialog, bossOpenDialog, eventRoomDialog, initRoomDialog, visitedRoomDialog };


    //private string[] specialDialogs = Directory.GetFiles("Assets/Assets/Texts/SpecialTexts", "*.txt");
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
        readTextFile(specialDialogs[3]);

        source = GetComponent<AudioSource>();
        StartCoroutine(Type());
    }

    void Update() {
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

    private void readTextFile(string[] textArray) {
        foreach (var line in textArray) {
            sentences.Add(line);
        }
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
            readTextFile(specialDialogs[4]);
            postCombatStatus = true;
            StartCoroutine(Type());
        } else if (mapController.characterPositionNode.isBossInRoom) {
            sentences.Clear();
            if (roomDifficultyManager.roomCounter >= 2) {
                readTextFile(specialDialogs[1]);
                StartCoroutine(Type());
            } else {
                readTextFile(specialDialogs[0]);
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
                    readTextFile(specialDialogs[2]);
                    postCombatStatus = true;
                    StartCoroutine(Type());
                    eventRoomPotion.SetActive(true);
                    playerController.AddHealth(100 - playerController.healthPoints);
                    return;

                default:
                    return;
            }
        }
    }

    private void readRandomFrom(string[][] dialogList) {
        int randy = Random.Range(0, dialogList.Length);
        sentences.Clear();
        readTextFile(dialogList[randy]);
    }

    private void mockCombat() {
        StartCoroutine(Type());
        startCombatButton.SetActive(false);
    }

    public void prepCloseUI() {
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
