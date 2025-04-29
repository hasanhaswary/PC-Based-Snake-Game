using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUImanger : MonoBehaviour

{
    public Button exitButton;
    public Button restartButton;
    public Button muteButton;
    public Button unmuteButton;
    public Button mainMenuButton;
    public GameObject audioSystem;
    public GameObject unmuteButtonObj;
    public GameObject muteButtonObj;
    public GameObject gameScreen;
    public GameObject restartScreen;


    // Start is called before the first frame update
    void Start()
    {
        //Start State
        gameScreen.SetActive(true);
        restartScreen.SetActive(false);

        //Buttons Functionality
        exitButton.onClick.AddListener(GameController.Instance.LoadMenu);
        restartButton.onClick.AddListener(GameController.Instance.LoadGame);

        muteButton.onClick.AddListener(() =>
        {
            GameController.Instance.MuteAudio(audioSystem, unmuteButtonObj, muteButtonObj);
        });

        unmuteButton.onClick.AddListener(() =>
        {
            GameController.Instance.UnmuteAudio(audioSystem, unmuteButtonObj, muteButtonObj);
        });

        mainMenuButton.onClick.AddListener(GameController.Instance.LoadMenu);

    }

    public void GameEnd()
    {
        gameScreen.SetActive(true);
        restartScreen.SetActive(true);
    }
}
