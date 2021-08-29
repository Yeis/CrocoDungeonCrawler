using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class DifficultyController : MonoBehaviour {

    public int easyMinRange = 6;
    public int easyMaxRange = 9;
    public int mediumMinRange = 5;
    public int mediumMaxRange = 7;
    public int hardMinRange = 3;
    public int hardMaxRange = 6;
    public int mediumColorShiftPercentage = 10;
    public int hardColorShiftPercentage = 20;
    public int penaltyDelayTutorial = 3;
    public int penaltyDelayEasy = 3;
    public int penaltyDelayMedium = 4;
    public int penaltyDelayHard = 5;
    public string GameScene = "VictoryScreen";

    public GameObject relic;
    public TMP_Text crocoCounterText;
    public TMP_Text requiredNumberOfCrocosText;
    public GameObject transitioner;
    public GameObject easyBackground;
    public GameObject mediumBackground;
    public GameObject bossBackground;
    public GameObject bossObject;
    public GameObject easyBossWaveObject;
    public GameObject mediumBossWaveObject;
    public GameObject hardBossWaveObject;
    public GameObject inputObject;
    public GameObject spawnerObject;
    public AudioSurvivor audioSurvivor;

    private InputController inputController;
    private int shiftCounter = 0;
    private int crocoCounter = 0;
    private RelicController relicController;
    private int easyDifficultyLimit;
    private int mediumDifficultyLimit;
    private int hardDifficultyLimit;
    private TransitionController transitionController;
    private int requiredNumberOfCrocos = 0;
    private RoomDifficulty roomDifficulty;
    private bool isInBossEncounter = false;

    private Dictionary<GameObject, Gem> vulnerabilityGems = new Dictionary<GameObject, Gem>();
    private List<GameObject> vulterabilityGemObjects = new List<GameObject>();
    private int vulnerabilityGemCounter = 0;
    private GameObject bossComboObject;
    private Spawner spawnerController;


    public Gem targetedGem;

    void Awake() {
        spawnerController = spawnerObject.GetComponent<Spawner>();
        relicController = relic.GetComponent<RelicController>();
        transitionController = transitioner.GetComponent<TransitionController>();
        inputController = inputObject.GetComponent<InputController>();

        easyDifficultyLimit = Random.Range(easyMinRange, easyMaxRange + 1);
        mediumDifficultyLimit = Random.Range(mediumMinRange, mediumMaxRange + 1);
        hardDifficultyLimit = Random.Range(hardMinRange, hardMaxRange + 1);
    }

    void Start() {
        audioSurvivor = GameObject.FindGameObjectWithTag("AudioSurvivor").GetComponent<AudioSurvivor>();
    }

    public void startEncounter(int requiredCrocos, RoomDifficulty difficulty) {
        requiredNumberOfCrocos = requiredCrocos;
        roomDifficulty = difficulty;

        switch (roomDifficulty) {
            case RoomDifficulty.veryEasy:
            case RoomDifficulty.easy:
                easyBackground.SetActive(true);
                mediumBackground.SetActive(false);
                bossBackground.SetActive(false);
                break;
            case RoomDifficulty.medium:
                easyBackground.SetActive(false);
                mediumBackground.SetActive(true);
                bossBackground.SetActive(false);
                break;
            case RoomDifficulty.hard:
                easyBackground.SetActive(false);
                mediumBackground.SetActive(false);
                bossBackground.SetActive(true);

                // ACTIVATE BOSS BATTLE
                bossObject.SetActive(true);
                audioSurvivor.SelectSong();
                isInBossEncounter = true;
                break;
        }

        crocoCounterText.text = crocoCounter.ToString("00");
        requiredNumberOfCrocosText.text = requiredNumberOfCrocos.ToString("00");
    }

    public void killCroco(KeyValuePair<GameObject, Gem> gemPair) {
        if (!isInBossEncounter) {
            crocoCounter += 1;
            crocoCounterText.text = crocoCounter.ToString("00");

            if (crocoCounter == requiredNumberOfCrocos) {
                endEncounter(true);
                return;
            }
        }

        shiftCounter += 1;
        switch (roomDifficulty) {
            case RoomDifficulty.veryEasy:
                if (shiftCounter == 5) {
                    gemShift(1, false, false);
                    shiftCounter = 0;
                }
                break;
            case RoomDifficulty.easy:
                if (shiftCounter == easyDifficultyLimit) {
                    gemShift(Random.Range(1, 3), false, false);
                    shiftCounter = 0;
                    easyDifficultyLimit = Random.Range(easyMinRange, easyMaxRange + 1);
                }
                break;
            case RoomDifficulty.medium:
                if (shiftCounter == mediumDifficultyLimit) {
                    gemShift(Random.Range(1, 4), true, false);
                    colorShift(mediumColorShiftPercentage);
                    shiftCounter = 0;
                    mediumDifficultyLimit = Random.Range(mediumMinRange, mediumMaxRange + 1);
                }
                break;
            case RoomDifficulty.hard:
                if (shiftCounter == hardDifficultyLimit) {
                    gemShift(Random.Range(1, 4), true, true);
                    colorShift(hardColorShiftPercentage);
                    shiftCounter = 0;
                    hardDifficultyLimit = Random.Range(hardMinRange, hardMaxRange + 1);
                }
                break;

            default: break;
        }
    }

    public void Victory(){
        SceneManager.LoadScene(GameScene);
    }

    public void penalizeGem(KeyValuePair<GameObject, Gem> gemPair) {
        var penaltyTime = 0;
        switch (roomDifficulty) {
            case RoomDifficulty.veryEasy:
                penaltyTime = penaltyDelayTutorial;
                break;
            case RoomDifficulty.easy:
                penaltyTime = penaltyDelayEasy;
                break;
            case RoomDifficulty.medium:
                penaltyTime = penaltyDelayMedium;
                break;
            case RoomDifficulty.hard:
                penaltyTime = penaltyDelayHard;
                break;
            default: break;
        }

        StartCoroutine(relicController.penalizeGem(gemPair, penaltyTime));
    }

    public void displayBossComboSequence(BossWave bossWave) {
        inputController.isBossVulnerable = true;

        bossComboObject = easyBossWaveObject;
        switch (bossWave) {
            case BossWave.easyWave:
                easyBossWaveObject.SetActive(true);
                bossComboObject = easyBossWaveObject;
                break;
            case BossWave.mediumWave:
                mediumBossWaveObject.SetActive(true);
                bossComboObject = mediumBossWaveObject;
                break;
            case BossWave.hardWave:
                hardBossWaveObject.SetActive(true);
                bossComboObject = hardBossWaveObject;
                break;
        }


        foreach (Transform child in bossComboObject.transform)
            vulterabilityGemObjects.Add(child.gameObject);

        List<GemColor> gemColorList = new List<GemColor>() { GemColor.blue, GemColor.green, GemColor.orange, GemColor.pink };
        var randy = new System.Random();

        for (int i = 0; i < vulterabilityGemObjects.Count; i++) {
            var randomGem = new Gem(gemColorList[randy.Next(0, 4)]);
            vulnerabilityGems[vulterabilityGemObjects[i]] = randomGem;
        }

        foreach (var gem in vulnerabilityGems) {
            updateGem(gem, false);
        }

        inputController.targetedVulnerableGem = vulnerabilityGems.First();
    }

    public void updateGem(KeyValuePair<GameObject, Gem> gemPair, bool destroyed) {
        if (destroyed) {
            gemPair.Key.GetComponent<SpriteRenderer>().sprite = relicController.gemSprites[((int)GemColor.broken)];
        } else {
            gemPair.Key.GetComponent<SpriteRenderer>().sprite = relicController.gemSprites[((int)gemPair.Value.gemColor)];
        }
    }

    public void attackVulnerabilityGem() {
        updateGem(vulnerabilityGems.ElementAt(vulnerabilityGemCounter), true);
        Boss boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();

        
        vulnerabilityGemCounter += 1;
        switch (spawnerController.currentBossWave) {
            case BossWave.easyWave:
                if (vulnerabilityGemCounter >= 3) {
                    boss.TakeDamage();
                    spawnerController.advanceBossWave();
                    hideVulnerabilityCombo();
                    return;
                }
                break;

            case BossWave.mediumWave:
                if (vulnerabilityGemCounter >= 4) {
                    boss.TakeDamage();
                    spawnerController.advanceBossWave();
                    hideVulnerabilityCombo();
                    return;

                }

                break;
            case BossWave.hardWave:
                if (vulnerabilityGemCounter >= 5) {

                    Victory();

                    boss.TakeDamage();
                    print("we did it boys");

                    hideVulnerabilityCombo();
                    return;
                }

                break;
        }

        inputController.targetedVulnerableGem = vulnerabilityGems.ElementAt(vulnerabilityGemCounter);

    }

    public void hideVulnerabilityCombo() {
        vulnerabilityGems.Clear();
        vulterabilityGemObjects.Clear();
        bossComboObject.SetActive(false);
        vulnerabilityGemCounter = 0;
        inputController.isBossVulnerable = false;
    }

    public void triggerGameOver() {
        endEncounter(false);
    }

    /// --- Private methods---

    private void setVulnerabilityGem() {

    }

    private void endEncounter(bool hasWon) {
        requiredNumberOfCrocos = 0;
        crocoCounter = 0;
        roomDifficulty = RoomDifficulty.inactive;

        crocoCounterText.text = crocoCounter.ToString("00");
        requiredNumberOfCrocosText.text = requiredNumberOfCrocos.ToString("00");

        if (hasWon) {
            transitionController.endEncounter();
        } else {
            // TODO: Game over 
        }
    }

    private InputControl getUniqueRandomAlphaInput() {
        var exclusiveSymbols = RelicSymbols.alphaInputs().Where(x => !relicController.currentSymbols.Contains(x));
        var index = Random.Range(0, exclusiveSymbols.Count());

        return exclusiveSymbols.ToArray()[index];
    }

    private InputControl getUniqueRandomAlphaNumericInput() {
        var probabilityRange = Random.Range(0, 10);
        IEnumerable<InputControl> exclusiveSymbols;

        if (probabilityRange < 8) {
            exclusiveSymbols = RelicSymbols.alphaInputs().Where(x => !relicController.currentSymbols.Contains(x));
        } else {
            exclusiveSymbols = RelicSymbols.numericInputs().Where(x => !relicController.currentSymbols.Contains(x));
        }
        var index = Random.Range(0, exclusiveSymbols.Count());
        return exclusiveSymbols.ToArray()[index];
    }

    private InputControl getUniqueRandomAlphaNumericSymbolInput() {
        var probabilityRange = Random.Range(0, 10);
        IEnumerable<InputControl> exclusiveSymbols;

        if (probabilityRange < 6) {
            exclusiveSymbols = RelicSymbols.alphaInputs().Where(x => !relicController.currentSymbols.Contains(x));
        } else if (probabilityRange < 8) {
            exclusiveSymbols = RelicSymbols.numericInputs().Where(x => !relicController.currentSymbols.Contains(x));
        } else {
            exclusiveSymbols = RelicSymbols.symbolInputs().Where(x => !relicController.currentSymbols.Contains(x));
        }
        var index = Random.Range(0, exclusiveSymbols.Count());
        return exclusiveSymbols.ToArray()[index];
    }

    private void gemShift(int numberOfGems, bool includeNums, bool includeSymbolsAndNums) {
        var exclude = new HashSet<int>();
        var gemsAffected = new List<KeyValuePair<GameObject, Gem>>();

        for (int i = 0; i < numberOfGems; i++) {
            var range = Enumerable.Range(0, 4).Where(i => !exclude.Contains(i));
            var random = new System.Random();
            int index = random.Next(0, 4 - exclude.Count);
            var gemToUpdate = range.ElementAt(index);

            var currentGem = relicController.gemObjects[gemToUpdate];
            var newSymbol = includeSymbolsAndNums ? getUniqueRandomAlphaNumericSymbolInput() : includeNums ? getUniqueRandomAlphaNumericInput() : getUniqueRandomAlphaInput();

            relicController.gemDictionary[currentGem].symbol = newSymbol;
            relicController.currentSymbols[gemToUpdate] = newSymbol;

            gemsAffected.Add(relicController.gemDictionary.ElementAt(gemToUpdate));
            exclude.Add(gemToUpdate);
        }

        foreach (var gem in gemsAffected) {
            relicController.updateGem(gem);
        }
    }

    // Percentage is a number from 0 to 10 that will be compared against a random generated number of the same range
    // It will define how frequently the color shift will be executed. For example:
    // If percentage is 20, 20 percent of the time the color shift will happen. 
    private void colorShift(int percentage) {

        // Do not colorShift while ANY gem is penalized 
        var penalizedGem = relicController.gemDictionary.Where(x => x.Value.isPenalized).FirstOrDefault();
        // If there is absolutely no penalized gems...
        if (penalizedGem.Key != null) {
            var probabilityRange = Random.Range(0, 10);

            if (probabilityRange > 9 - (percentage / 10)) {
                List<GemColor> gemColorList = new List<GemColor>() { GemColor.blue, GemColor.green, GemColor.orange, GemColor.pink };
                var random = new System.Random();
                var randomizedGemColorList = gemColorList.OrderBy(item => random.Next());

                var gemObjectCounter = 0;
                foreach (var gemColor in randomizedGemColorList) {
                    relicController.gemDictionary[relicController.gemObjects[gemObjectCounter]].gemColor = gemColor;
                    gemObjectCounter += 1;
                }
            }

        }

        foreach (var gem in relicController.gemDictionary) {
            relicController.updateGem(gem);
        }
    }

}
