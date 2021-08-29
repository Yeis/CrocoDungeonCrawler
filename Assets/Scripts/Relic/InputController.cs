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
    // public GameObject enemy;

    private RelicController relicController;
    private DifficultyController difficultyController;
    private Player player;
    // private Enemy enem;
    private bool isInCombat = false;

    void Awake() {
        controls = new RelicInputs();
        controls.Relic.SymbolSelect.performed += ctx => SymbolInteraction(ctx.control, true);
        controls.Relic.SymbolSelect.canceled += ctx => SymbolInteraction(ctx.control, false);

        relicController = relic.GetComponent<RelicController>();
        difficultyController = difficulty.GetComponent<DifficultyController>();
        player = playerObject.GetComponent<Player>();
        // enem = enemy.GetComponent<Enemy>();
    }

    void SymbolInteraction(InputControl control, bool isPressing) {
        if (isInCombat) {
            try {
                var gemPair = relicController.gemDictionary.Where(x => control == x.Value.symbol).First();
                var lockedEnemy = player.lockedEnemy.GetComponent<Enemy>();
                var isMistake = false;

                if (isPressing && lockedEnemy.color == gemPair.Value.gemColor) {
                    player.Attack(gemPair.Value.gemColor);
                    difficultyController.killCroco();
                } else if (isPressing) {
                    isMistake = true;
                    print("horaDeLaPenalizacion");
                }

                relicController.gemInteraction(gemPair, isPressing, isMistake);
            } catch {
                if (isPressing) {
                    print("horaDeLaPenalizacion");
                }
            }

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
