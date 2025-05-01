using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    
    public GameObject mainMenuPanel;         
    public GameObject gameScreen;            
    public GameObject restartButton;   
    public GameObject gamePlay;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager initialized.");
        }
        else
        {
            
            Debug.Log("Duplicate GameManager destroyed.");
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadMainMenu();
        }
        if (Input.GetKeyDown(KeyCode.Space)){
            StartGame();
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {      
        if (scene.name == "MainMenu")
        {
            gameScreen = null;
            restartButton = null;

        }
        UpdateUI();
    }

    void UpdateUI()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        
        if (sceneName == "MainMenu")
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(true); 
        }
        else if (sceneName == "SnakeGame")
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (gameScreen != null) gameScreen.SetActive(true);    
            if (restartButton != null) restartButton.SetActive(false);           
            if (gamePlay != null) gamePlay.SetActive(true);            
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SnakeGame");
        Debug.Log("StartGame working");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("LoadMainMenu working");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SnakeGame");
        Debug.Log("RestartGame working");
    }

    public void ShowGameOver()
    {
        if (restartButton != null) restartButton.SetActive(true);
    }
}

//not sure what this does but if i delete it it breaks the SCene transitions
public static class TransformExtensions
{
    public static string GetPath(this Transform transform)
    {
        string path = transform.name;
        while (transform.parent != null)
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }
        return path;
    }
}