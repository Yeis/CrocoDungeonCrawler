using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;

public class DifficultyController : MonoBehaviour {

    public int easyMinRange = 6;
    public int easyMaxRange = 9;
    public int mediumMinRange = 5;
    public int mediumMaxRange = 7;
    public int hardMinRange = 3;
    public int hardMaxRange = 6;
    public int mediumColorShiftPercentage = 10;
    public int hardColorShiftPercentage = 20;

    public GameObject relic;
    public RoomDifficulty roomDifficulty;

    private int shiftCounter = 0;
    private int crocoCounter = 0;
    private RelicController relicController;
    private int easyDifficultyLimit;
    private int mediumDifficultyLimit;
    private int hardDifficultyLimit;


    void Awake() {
        relicController = relic.GetComponent<RelicController>();

        easyDifficultyLimit = Random.Range(easyMinRange, easyMaxRange + 1);
        mediumDifficultyLimit = Random.Range(mediumMinRange, mediumMaxRange + 1);
        hardDifficultyLimit = Random.Range(hardMinRange, hardMaxRange + 1);
    }

    public void killCroco() {
        crocoCounter += 1;

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
        relicController.updateGems();
    }

    private void gemShift(int numberOfGems, bool includeNums, bool includeSymbolsAndNums) {
        var exclude = new HashSet<int>();

        for (int i = 0; i < numberOfGems; i++) {
            var range = Enumerable.Range(0, 4).Where(i => !exclude.Contains(i));
            var random = new System.Random();
            int index = random.Next(0, 4 - exclude.Count);
            var gemToUpdate = range.ElementAt(index);

            var currentGem = relicController.gemObjects[gemToUpdate];
            var newSymbol = includeSymbolsAndNums ? getUniqueRandomAlphaNumericSymbolInput() : includeNums ? getUniqueRandomAlphaNumericInput() : getUniqueRandomAlphaInput();

            relicController.gemDictionary[currentGem].symbol = newSymbol;
            relicController.currentSymbols[gemToUpdate] = newSymbol;

            exclude.Add(gemToUpdate);
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

    // Percentage is a number from 0 to 10 that will be compared against a random generated number of the same range
    // It will define how frequently the color shift will be executed. For example:
    // If percentage is 20, 20 percent of the time the color shift will happen. 
    private void colorShift(int percentage) {
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

}
