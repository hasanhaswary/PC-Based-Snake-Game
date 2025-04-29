using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour

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


    private GameController gameController = GameController.Instance;


    // Start is called before the first frame update
    void Start()
    {
        //Start State
        gameScreen.SetActive(true);
        restartScreen.SetActive(false);


        //Buttons Functionality
        exitButton.onClick.AddListener(gameController.LoadMenu);
        restartButton.onClick.AddListener(gameController.LoadGame);

        muteButton.onClick.AddListener(() =>
        {
            gameController.MuteAudio(audioSystem, unmuteButtonObj, muteButtonObj);
        });

        unmuteButton.onClick.AddListener(() =>
        {
            gameController.UnmuteAudio(audioSystem, unmuteButtonObj, muteButtonObj);
        });

        mainMenuButton.onClick.AddListener(gameController.LoadMenu);

    }

    public void GameEnd()
    {
        gameScreen.SetActive(true);
        restartScreen.SetActive(true);
    }
}
