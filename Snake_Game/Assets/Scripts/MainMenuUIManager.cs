using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.XR;

public class MainMenuUIManager : MonoBehaviour
{
    public GameObject mainMenuScreen;
    public GameObject instructionScreen;
    public GameObject muteButtonObj;
    public GameObject unmuteButtonObj;
    public GameObject mainMenuAudioObj;
    public Button startButton;
    public Button infoButton;
    public Button muteButton;
    public Button unmuteButton;
    public Button exitInstructionButton;
    public AudioSource mainMenuAudio;

    // Start is called before the first frame update
    void Start()
    {
        //Intial State
        mainMenuScreen.SetActive(true);
        instructionScreen.SetActive(false);

        //Button Functionality
        startButton.onClick.AddListener(() =>
        {
            GameController.Instance.LoadGame();
            mainMenuAudio.Stop();
            mainMenuScreen.SetActive(false);
        });

        infoButton.onClick.AddListener(() =>
        {
            instructionScreen.SetActive(true);
            mainMenuScreen.SetActive(false);
        });

        muteButton.onClick.AddListener(() => 
        {
            GameController.Instance.MuteAudio(mainMenuAudioObj, unmuteButtonObj, muteButtonObj);
        });

        unmuteButton.onClick.AddListener(() =>
        {
            GameController.Instance.UnmuteAudio(mainMenuAudioObj, unmuteButtonObj, muteButtonObj);
        });

        exitInstructionButton.onClick.AddListener(() =>
        {
            instructionScreen.SetActive(false);
        });
    }
}
