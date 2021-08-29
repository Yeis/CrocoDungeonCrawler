using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Sprite newButtonImg;
    public Button button;


    public void changeButtonImage()
    {
        button.image.sprite = newButtonImg;
    }
}
