using UnityEngine;

public class ButtonProxy : MonoBehaviour
{
    
    //Had to create a proxy for the button clicks because something was break but functions was working
    public void OnRestartClick()
    {
        Debug.Log("Proxy: Restart button clicked!");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
        else
        {
            Debug.LogError("GameManager reference is null in proxy!");
        }
    }

    public void OnExitClick()
    {
        Debug.Log("Proxy: Exit button clicked!");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadMainMenu();
        }
        else
        {
            Debug.LogError("GameManager reference is null in proxy!");
        }
    }
}