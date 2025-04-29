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


    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(GameController.Instance.LoadMenu);
        restartButton.onClick.AddListener(GameController.Instance.RestartGame);
        muteButton.onClick.AddListener(() => GameController.Instance.MuteAudio(audioSystem, unmuteButtonObj, muteButtonObj));
        unmuteButton.onClick.AddListener(() => GameController.Instance.UnmuteAudio(audioSystem, unmuteButtonObj, muteButtonObj));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
