using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    
    public GameObject gameScreen;
    public GameObject restartButton;

    void Awake()
    {
        // Check if GameManager exists and register the GameObjects
        if (GameManager.Instance != null)
        {
            if (gameScreen != null)
            {
                GameManager.Instance.gameScreen = gameScreen;
                Debug.Log("Registered gameScreen with GameManager.");
            }
            if (restartButton != null)
            {
                GameManager.Instance.restartButton = restartButton;
                Debug.Log("Registered restartActionsPanel with GameManager.");
            }
        }
        else
        {
            Debug.LogWarning("GameManager instance not found!");
        }
    }
}
