using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TransitionController : MonoBehaviour {

    public GameObject spawnerObject;
    public GameObject relicObject;
    public GameObject difficultyObject;
    public GameObject inputObject;
    public GameObject crocoCounterObject;
    public TMP_Text crocoCounterText;
    public TMP_Text remainingCrocoText;
    public GameObject textObject;

    private Spawner spawner;
    private RelicController relicController;
    private DifficultyController difficultyController;
    private InputController inputController;
    private TextScript textController;

    public void Awake() {
        spawner = spawnerObject.GetComponent<Spawner>();
        relicController = relicObject.GetComponent<RelicController>();
        difficultyController = difficultyObject.GetComponent<DifficultyController>();
        inputController = inputObject.GetComponent<InputController>();
        textController = textObject.GetComponent<TextScript>();
    }

    // TODO: agregar dificultad como parametro 
    public void startEncounter(RoomType roomType) {
        var maxNumberOfCrocoSpawn = 100;
        var requiredCrocos = 0;
        var roomDifficulty = RoomDifficulty.inactive;

        switch (roomType) {
            case RoomType.tutorial:
                requiredCrocos = 15;
                roomDifficulty = RoomDifficulty.veryEasy;
                break;
            case RoomType.easyRoom:
                requiredCrocos = 30;
                roomDifficulty = RoomDifficulty.easy;
                break;
            case RoomType.mediumRoom:
                requiredCrocos = 40;
                roomDifficulty = RoomDifficulty.medium;
                break;
            case RoomType.bossRoom:
                requiredCrocos = 50;
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
    }

    public void endEncounter() {
        spawner.stopEncounter();
        relicController.setUpForDialog();
        inputController.setIsInCombat(false);

        crocoCounterObject.SetActive(false);
        crocoCounterText.gameObject.SetActive(false);
        remainingCrocoText.gameObject.SetActive(false);

        textController.reopenUI();
    }
}
