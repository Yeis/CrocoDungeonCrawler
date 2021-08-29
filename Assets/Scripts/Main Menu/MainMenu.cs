using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Sprite newButtonImg;
    public Button button;

    public string GameScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeButtonImage()
    {
        button.image.sprite = newButtonImg;
    }

    public void NewGameScene()
    {
        SceneManager.LoadScene(GameScene);
    }
}
