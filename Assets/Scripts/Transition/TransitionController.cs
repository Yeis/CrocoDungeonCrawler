using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransitionController : MonoBehaviour {

    public GameObject spawnerObject;
    public GameObject relicObject;
    public GameObject difficultyObject;
    public GameObject inputObject;
    public GameObject crocoCounterObject;
    public TMP_Text crocoCounterText;
    public TMP_Text remainingCrocoText;
    public TMP_Text hpLabel;
    public GameObject hpBar;
    public GameObject textObject;

    private Spawner spawner;
    private RelicController relicController;
    private DifficultyController difficultyController;
    private InputController inputController;
    private TextScript textController;

    public int maxNumberOfCrocoSpawn = 100;
    public int requiredCrocosTutorial = 15;
    public int requiredCrocosEasy = 30;
    public int requiredCrocosMedium = 40;
    public int requiredCrocosBoss = 50;

    public void Awake() {
        spawner = spawnerObject.GetComponent<Spawner>();
        relicController = relicObject.GetComponent<RelicController>();
        difficultyController = difficultyObject.GetComponent<DifficultyController>();
        inputController = inputObject.GetComponent<InputController>();
        textController = textObject.GetComponent<TextScript>();
    }

    // TODO: agregar dificultad como parametro 
    public void startEncounter(RoomType roomType) {
        var requiredCrocos = 0;
        var roomDifficulty = RoomDifficulty.inactive;

        switch (roomType) {
            case RoomType.tutorial:
                requiredCrocos = requiredCrocosTutorial;
                roomDifficulty = RoomDifficulty.veryEasy;
                break;
            case RoomType.easyRoom:
                requiredCrocos = requiredCrocosEasy;
                roomDifficulty = RoomDifficulty.easy;
                break;
            case RoomType.mediumRoom:
                requiredCrocos = requiredCrocosMedium;
                roomDifficulty = RoomDifficulty.medium;
                break;
            case RoomType.bossRoom:
                requiredCrocos = requiredCrocosBoss;
                roomDifficulty = RoomDifficulty.hard;
                break;
        }

        spawner.startEncounter(maxNumberOfCrocoSpawn);
        relicController.setUpForCombat();
        difficultyController.startEncounter(requiredCrocos, roomDifficulty);
        inputController.setIsInCombat(true);

        crocoCounterObject.SetActive(true);
        crocoCounterText.gameObject.SetActive(true);
        remainingCrocoText.gameObject.SetActive(true);
        hpBar.SetActive(true);
        hpLabel.gameObject.SetActive(true);
    }

    public void endEncounter() {
        spawner.stopEncounter();
        relicController.setUpForDialog();
        inputController.setIsInCombat(false);

        crocoCounterObject.SetActive(false);
        crocoCounterText.gameObject.SetActive(false);
        remainingCrocoText.gameObject.SetActive(false);
        hpBar.SetActive(false);
        hpLabel.gameObject.SetActive(false);
        textController.reopenUI();
    }
}
