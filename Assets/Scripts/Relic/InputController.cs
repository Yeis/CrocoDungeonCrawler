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
    private RelicController relicController;
    private DifficultyController difficultyController;

    void Awake() {
        controls = new RelicInputs();
        controls.Relic.SymbolSelect.performed += ctx => SymbolInteraction(ctx.control, true);
        controls.Relic.SymbolSelect.canceled += ctx => SymbolInteraction(ctx.control, false);

        relicController = relic.GetComponent<RelicController>();
        difficultyController = difficulty.GetComponent<DifficultyController>();
    }

    void SymbolInteraction(InputControl control, bool isPressing) {
        try {
            var gemPair = relicController.gemDictionary.Where(x => control == x.Value.symbol).First();
            if (isPressing) {
                difficultyController.killCroco();
            }

            relicController.gemInteraction(gemPair, isPressing);
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
