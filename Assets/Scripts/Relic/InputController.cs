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

    private RelicController relicController;
    private DifficultyController difficultyController;
    private Player player;

    void Awake() {
        controls = new RelicInputs();
        controls.Relic.SymbolSelect.performed += ctx => SymbolInteraction(ctx.control, true);
        controls.Relic.SymbolSelect.canceled += ctx => SymbolInteraction(ctx.control, false);

        relicController = relic.GetComponent<RelicController>();
        difficultyController = difficulty.GetComponent<DifficultyController>();
        player = playerObject.GetComponent<Player>();
    }

    void SymbolInteraction(InputControl control, bool isPressing) {
        try {
            var gemPair = relicController.gemDictionary.Where(x => control == x.Value.symbol).First();
            var lockedEnemy = player.lockedEnemy.GetComponent<Enemy>();
            var isMistake = false;

            if (isPressing && lockedEnemy.color == gemPair.Value.gemColor) {
                difficultyController.killCroco();
                player.Attack();
            } else {
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

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();

    }
}
