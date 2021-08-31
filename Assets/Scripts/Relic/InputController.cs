using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using TMPro;


public class InputController : MonoBehaviour {
    public RelicInputs controls;
    public GameObject relic;
    public GameObject difficulty;
    public GameObject playerObject;
    public GameObject spawnerObject;

    private RelicController relicController;
    private DifficultyController difficultyController;
    private Player player;
    // private Enemy enem;
    private bool isInCombat = false;
    public bool isBossVulnerable = false;
    public KeyValuePair<GameObject, Gem> targetedVulnerableGem;
    private Spawner spawnerController;

    void Awake() {
        controls = new RelicInputs();
        controls.Relic.SymbolSelect.performed += ctx => SymbolInteraction(ctx.control, true);
        controls.Relic.SymbolSelect.canceled += ctx => SymbolInteraction(ctx.control, false);

        spawnerController = spawnerObject.GetComponent<Spawner>();
        relicController = relic.GetComponent<RelicController>();
        difficultyController = difficulty.GetComponent<DifficultyController>();
        player = playerObject.GetComponent<Player>();
    }

    void SymbolInteraction(InputControl control, bool isPressing) {
        if (isInCombat) {
            var gemPair = new KeyValuePair<GameObject, Gem>();
            try {
                gemPair = relicController.gemDictionary.Where(x => control == x.Value.symbol).First();
            } catch {
                if (isPressing && !isBossVulnerable) {
                    int randomIndex = Random.Range(0, relicController.gemDictionary.Count);

                    KeyValuePair<GameObject, Gem> randomGem = relicController.gemDictionary.ElementAt(randomIndex);
                    difficultyController.penalizeGem(randomGem);
                }
                return;
            }

            if (gemPair.Value.isPenalized) {
                return;
            }
            var isMistake = false;

            // Boss Vulnerable logic 
            if (isBossVulnerable && isPressing) {
                if (targetedVulnerableGem.Value.gemColor == gemPair.Value.gemColor) {
                    difficultyController.attackVulnerabilityGem();
                } else {
                    spawnerController.restartCurrentWave();
                    difficultyController.hideVulnerabilityCombo();
                }
            } else {
                if (player.lockedEnemy != null) {
                    var lockedEnemy = player.lockedEnemy.GetComponent<Enemy>();

                    if (isPressing && lockedEnemy.color == gemPair.Value.gemColor) {
                        player.Attack(gemPair.Value.gemColor);
                        difficultyController.killCroco(gemPair);
                    } else if (isPressing) {
                        isMistake = true;
                        difficultyController.penalizeGem(gemPair);
                    }
                }
            }
            relicController.gemInteraction(gemPair, isPressing, isMistake);

        }
    }
    public void setIsInCombat(bool startedCombat) {
        isInCombat = startedCombat;
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }
}
