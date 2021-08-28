using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RelicController : MonoBehaviour {
    public Dictionary<GameObject, Gem> gemDictionary = new Dictionary<GameObject, Gem>();
    public List<GameObject> gemObjects;
    public List<InputControl> currentSymbols;
    public Sprite[] gemSprites;

    public TMP_Text northGemText;
    public TMP_Text westGemText;
    public TMP_Text southGemText;
    public TMP_Text eastGemText;

    /// Control order - [0]North, [1]West, [2]South, [3]East 
    private List<Gem> gems;
    private Keyboard keyboard;

    void Awake() {
        keyboard = Keyboard.current;

        // Default keys EMPTY
        currentSymbols = new List<InputControl> {
            keyboard.oem1Key,
            keyboard.oem1Key,
            keyboard.oem1Key,
            keyboard.oem1Key
        };

        gems = new List<Gem> {
            new Gem(currentSymbols[0], GemColor.blue, GemDirection.north),
            new Gem(currentSymbols[1], GemColor.pink, GemDirection.west),
            new Gem(currentSymbols[2], GemColor.orange, GemDirection.south),
            new Gem(currentSymbols[3], GemColor.green, GemDirection.east),
        };

        gemObjects = new List<GameObject> {
            transform.Find("IndicatorGemNorth").gameObject,
            transform.Find("IndicatorGemWest").gameObject,
            transform.Find("IndicatorGemSouth").gameObject,
            transform.Find("IndicatorGemEast").gameObject
        };

        gemDictionary.Add(gemObjects[0], gems[0]);
        gemDictionary.Add(gemObjects[1], gems[1]);
        gemDictionary.Add(gemObjects[2], gems[2]);
        gemDictionary.Add(gemObjects[3], gems[3]);
    }

    void Start() {
        updateGems();
    }

    public void setUpForCombat() {
        currentSymbols[0] = keyboard.wKey;
        currentSymbols[1] = keyboard.aKey;
        currentSymbols[2] = keyboard.sKey;
        currentSymbols[3] = keyboard.dKey;

        for (int i = 0; i < 4; i++) {
            gems[i].symbol = currentSymbols[i];
        }

        updateGems();
    }

    public void setUpForDialog() {
        foreach (var gemPair in gemDictionary) {
            GemColor gemColor;

            switch (gemPair.Value.direction) {
                case GemDirection.north:
                    gemColor = GemColor.blue;
                    break;
                case GemDirection.west:
                    gemColor = GemColor.green;
                    break;
                case GemDirection.south:
                    gemColor = GemColor.orange;
                    break;
                case GemDirection.east:
                    gemColor = GemColor.pink;
                    break;
                default: gemColor = GemColor.disabled; break;
            }
            gemPair.Value.symbol = keyboard.oem1Key;
            gemPair.Key.GetComponent<SpriteRenderer>().sprite = gemSprites[((int)gemColor)];
        }

        updateGems();
    }

    public void gemInteraction(KeyValuePair<GameObject, Gem> gemPair, bool isPressing, bool isMistake) {
        GemColor gemSelected;
        switch (gemPair.Value.gemColor) {
            case GemColor.blue:
                gemSelected = isPressing ? isMistake ? GemColor.disabled : GemColor.blueSelected : GemColor.blue;
                break;
            case GemColor.green:
                gemSelected = isPressing ? isMistake ? GemColor.disabled : GemColor.greenSelected : GemColor.green;
                break;
            case GemColor.orange:
                gemSelected = isPressing ? isMistake ? GemColor.disabled : GemColor.orangeSelected : GemColor.orange;
                break;
            case GemColor.pink:
                gemSelected = isPressing ? isMistake ? GemColor.disabled : GemColor.pinkSelected : GemColor.pink;
                break;
            default: gemSelected = GemColor.disabled; break;
        }
        gemPair.Key.GetComponent<SpriteRenderer>().sprite = gemSprites[((int)gemSelected)];
    }

    public void updateGems() {
        foreach (var gemPair in gemDictionary) {
            TMP_Text gemLabel;
            switch (gemPair.Value.direction) {
                case GemDirection.north:
                    gemLabel = northGemText;
                    break;
                case GemDirection.west:
                    gemLabel = westGemText;
                    break;
                case GemDirection.south:
                    gemLabel = southGemText;
                    break;
                case GemDirection.east:
                    gemLabel = eastGemText;
                    break;
                default:
                    // should never happen
                    gemLabel = northGemText;
                    break;
            }

            gemLabel.text = gemPair.Value.symbol == keyboard.oem1Key ? "" : gemPair.Value.symbol.displayName;
            gemPair.Key.GetComponent<SpriteRenderer>().sprite = gemSprites[((int)gemPair.Value.gemColor)];
        }
    }
}
