using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class RelicController : MonoBehaviour {
    private Keyboard keyboard;

    /// Control order - [0]North, [1]West, [2]South, [3]East 
    private List<Gem> gems;
    public Button symbolSwitch, colorSwitch;

    private GameObject gemNorth;
    private GameObject gemWest;
    private GameObject gemSouth;
    private GameObject gemEast;

    private Sprite[] gemSprites;

    public Dictionary<GameObject, Gem> gemDictionary = new Dictionary<GameObject, Gem>();

    void Awake() {
        keyboard = Keyboard.current;

        AsyncOperationHandle<Sprite[]> spriteHandle = Addressables.LoadAssetAsync<Sprite[]>("Assets/Assets/Images/Relic/Gems_sprite.png");
        spriteHandle.Completed += LoadSpritesWhenReady;

        // Default keys WASD
        gems = new List<Gem> {
            new Gem(keyboard.wKey, GemColor.blue, GemDirection.north),
            new Gem(keyboard.aKey, GemColor.pink, GemDirection.west),
            new Gem(keyboard.sKey, GemColor.orange, GemDirection.south),
            new Gem(keyboard.dKey, GemColor.green, GemDirection.east),
        };

        gemNorth = transform.Find("IndicatorGemNorth").gameObject;
        gemWest = transform.Find("IndicatorGemWest").gameObject;
        gemSouth = transform.Find("IndicatorGemSouth").gameObject;
        gemEast = transform.Find("IndicatorGemEast").gameObject;

        gemDictionary.Add(gemNorth, gems[0]);
        gemDictionary.Add(gemWest, gems[1]);
        gemDictionary.Add(gemSouth, gems[2]);
        gemDictionary.Add(gemEast, gems[3]);

        symbolSwitch.onClick.AddListener(OnSymbolSwitchClick);
        colorSwitch.onClick.AddListener(OnColorSwitchClick);
    }

    void Start() { }

    void OnSymbolSwitchClick() {
        gemDictionary[gemNorth].symbol = keyboard.jKey;
        gemDictionary[gemWest].symbol = keyboard.kKey;
        gemDictionary[gemSouth].symbol = keyboard.lKey;
        gemDictionary[gemEast].symbol = keyboard.semicolonKey;

        setUpGems();
    }

    void OnColorSwitchClick() {
        gemDictionary[gemNorth].gemColor = GemColor.pink;
        gemDictionary[gemWest].gemColor = GemColor.orange;
        gemDictionary[gemSouth].gemColor = GemColor.green;
        gemDictionary[gemEast].gemColor = GemColor.blue;

        setUpGems();
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
}

public class Gem {
    public InputControl symbol;
    public GemColor gemColor;
    public GemDirection direction;

    public Gem(InputControl symbol, GemColor gemColor, GemDirection direction) {
        this.symbol = symbol;
        this.gemColor = gemColor;
        this.direction = direction;
    }
}

public enum GemColor {
    blue = 0,
    blueSelected = 1,
    disabled = 2,
    green = 3,
    greenSelected = 4,
    orange = 5,
    orangeSelected = 6,
    pink = 7,
    pinkSelected = 8
}

public enum GemDirection {
    north = 0, west = 1, south = 2, east = 3
}