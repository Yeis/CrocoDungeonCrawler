using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

public enum RoomDifficulty {
    veryEasy = 1, easy = 2, medium = 3, hard = 4
}

public static class RelicSymbols {
    private static Keyboard keyboard = Keyboard.current;

    public static List<InputControl> alphaInputs() {
        return new List<InputControl> {
            keyboard.qKey,
            keyboard.wKey,
            keyboard.eKey,
            keyboard.rKey,
            keyboard.tKey,
            keyboard.yKey,
            keyboard.uKey,
            keyboard.iKey,
            keyboard.oKey,
            keyboard.pKey,
            keyboard.aKey,
            keyboard.sKey,
            keyboard.dKey,
            keyboard.fKey,
            keyboard.gKey,
            keyboard.hKey,
            keyboard.jKey,
            keyboard.kKey,
            keyboard.lKey,
            keyboard.zKey,
            keyboard.xKey,
            keyboard.cKey,
            keyboard.vKey,
            keyboard.bKey,
            keyboard.nKey,
            keyboard.mKey,
        };
    }

    public static List<InputControl> numericInputs() {
        return new List<InputControl> {
            keyboard.digit0Key,
            keyboard.digit1Key,
            keyboard.digit2Key,
            keyboard.digit3Key,
            keyboard.digit4Key,
            keyboard.digit5Key,
            keyboard.digit6Key,
            keyboard.digit7Key,
            keyboard.digit8Key,
            keyboard.digit9Key,
        };
    }

    public static List<InputControl> symbolInputs() {
        return new List<InputControl> {
            keyboard.semicolonKey,
            keyboard.minusKey,
            keyboard.quoteKey,
            keyboard.periodKey,
            keyboard.commaKey,
        };
    }

}
