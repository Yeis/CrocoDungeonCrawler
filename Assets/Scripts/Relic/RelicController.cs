using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class RelicController : MonoBehaviour {
    public RoomDifficulty roomDifficulty;
    public Dictionary<GameObject, Gem> gemDictionary = new Dictionary<GameObject, Gem>();

    public TMP_Text crocoCounterText;

    /// Control order - [0]North, [1]West, [2]South, [3]East 
    private List<Gem> gems;
    private List<GameObject> gemObjects;
    private List<InputControl> currentSymbols;
    private Keyboard keyboard;
    private Sprite[] gemSprites;

    private int crocoCounter = 0;
    private int shiftCounter = 0;

    void Awake() {
        keyboard = Keyboard.current;

        AsyncOperationHandle<Sprite[]> spriteHandle = Addressables.LoadAssetAsync<Sprite[]>("Assets/Assets/Images/Relic/Gems_sprite.png");
        spriteHandle.Completed += LoadSpritesWhenReady;

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

        crocoCounterText.text = crocoCounter.ToString();
    }

    void Start() { }

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

    public void killCroco() {
        crocoCounter += 1;
        crocoCounterText.text = crocoCounter.ToString();

        shiftCounter += 1;
        switch (roomDifficulty) {
            case RoomDifficulty.veryEasy:
                if (shiftCounter == 5) {
                    var gemToUpdate = Random.Range(0, 4);
                    var currentGem = gemObjects[gemToUpdate];
                    var newSymbol = getUniqueRandomAlphaInput();

                    gemDictionary[currentGem].symbol = newSymbol;
                    currentSymbols[gemToUpdate] = newSymbol;
                    shiftCounter = 0;
                }
                break;
            default: break;
        }
        setUpGems();
    }

    /// Private
    void OnSymbolSwitchClick() {
        gemDictionary[gemObjects[0]].symbol = keyboard.jKey;
        gemDictionary[gemObjects[1]].symbol = keyboard.kKey;
        gemDictionary[gemObjects[2]].symbol = keyboard.lKey;
        gemDictionary[gemObjects[3]].symbol = keyboard.semicolonKey;

        setUpGems();
    }

    void OnColorSwitchClick() {
        gemDictionary[gemObjects[0]].gemColor = GemColor.pink;
        gemDictionary[gemObjects[1]].gemColor = GemColor.orange;
        gemDictionary[gemObjects[2]].gemColor = GemColor.green;
        gemDictionary[gemObjects[3]].gemColor = GemColor.blue;

        setUpGems();
    }


    private void LoadSpritesWhenReady(AsyncOperationHandle<Sprite[]> handle) {
        if (handle.Status == AsyncOperationStatus.Succeeded) {
            gemSprites = handle.Result;

            setUpGems();
        }
    }

    private void setUpGems() {
        foreach (var gemPair in gemDictionary) {
            var gemTextObject = gemPair.Key.transform.Find("GemText");

            var gemLabel = gemTextObject.GetComponent<TMP_Text>();
            gemLabel.text = gemPair.Value.symbol.displayName;

            gemPair.Key.GetComponent<SpriteRenderer>().sprite = gemSprites[((int)gemPair.Value.gemColor)];
        }
    }

    private InputControl getUniqueRandomAlphaInput() {
        var exclusiveSymbols = RelicSymbols.alphaInputs().Where(x => !currentSymbols.Contains(x));
        var index = Random.Range(0, exclusiveSymbols.Count());

        return exclusiveSymbols.ToArray()[index];
    }
}
