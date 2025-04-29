using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //So that it can be called across Scripts in other Scenes
    public static GameController Instance { get; private set; }

    //Game Variables
    public GameObject instructionScreen;
    public GameObject mainMenuScreen;
    public GameObject gameScreen;
    public GameObject restartScreen;
    public AudioSource mainMenuAudio;
    public Text displayedScore;
    


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    //Functions being used globally

    public void LoadGame()
    {
        SceneManager.LoadScene("SnakeGame");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void MuteAudio(GameObject audio,GameObject unmuteButton, GameObject muteButton)
    {
        if (audio != null) audio.SetActive(false);
        if (unmuteButton != null) unmuteButton.SetActive(false);
        if (muteButton != null) muteButton.SetActive(true);
    }

    public void UnmuteAudio(GameObject audio, GameObject unmuteButton, GameObject muteButton)
    {
        if (audio != null) audio.SetActive(true);
        if (unmuteButton != null) unmuteButton.SetActive(true);
        if (muteButton != null) muteButton.SetActive(false);
    }
}
