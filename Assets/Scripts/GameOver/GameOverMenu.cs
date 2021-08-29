using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public Sprite newButtonImg;
    public Button button;
    public void changeButtonImage()
    {
        button.image.sprite = newButtonImg;
    }
}
