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

    /// Control order - [0]North, [1]West, [2]South, [3]East 
    private List<Gem> gems;
    private Keyboard keyboard;

    void Awake() {
        keyboard = Keyboard.current;

        // Default keys WASD
        currentSymbols = new List<InputControl> {
            keyboard.wKey,
            keyboard.aKey,
            keyboard.sKey,
            keyboard.dKey
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

    public void gemInteraction(KeyValuePair<GameObject, Gem> gemPair, bool isPressing) {
        GemColor gemSelected;
        switch (gemPair.Value.gemColor) {
            case GemColor.blue:
                gemSelected = isPressing ? GemColor.blueSelected : GemColor.blue;
                break;
            case GemColor.green:
                gemSelected = isPressing ? GemColor.greenSelected : GemColor.green;
                break;
            case GemColor.orange:
                gemSelected = isPressing ? GemColor.orangeSelected : GemColor.orange;
                break;
            case GemColor.pink:
                gemSelected = isPressing ? GemColor.pinkSelected : GemColor.pink;
                break;
            default: gemSelected = GemColor.disabled; break;
        }
        gemPair.Key.GetComponent<SpriteRenderer>().sprite = gemSprites[((int)gemSelected)];
    }

    public void updateGems() {
        foreach (var gemPair in gemDictionary) {
            var gemTextObject = gemPair.Key.transform.Find("GemText");

            var gemLabel = gemTextObject.GetComponent<TMP_Text>();
            gemLabel.text = gemPair.Value.symbol.displayName;

            gemPair.Key.GetComponent<SpriteRenderer>().sprite = gemSprites[((int)gemPair.Value.gemColor)];
        }
    }
}
