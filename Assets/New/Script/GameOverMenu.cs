using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [Header("Refs")]
    public GameManager gameManager;   // <- make this PUBLIC so you can drag it in

    // You don't actually need Init(...) anymore unless you want to use it elsewhere

    public void OnRetryPressed()
    {
        if (gameManager != null) gameManager.Retry();
        else Debug.LogWarning("GameOverMenu: gameManager not assigned");
    }

    public void OnTitlePressed()
    {
        if (gameManager != null) gameManager.ReturnToTitle();
        else Debug.LogWarning("GameOverMenu: gameManager not assigned");
    }

    public void OnQuitPressed()
    {
        if (gameManager != null) gameManager.QuitGame();
        else Debug.LogWarning("GameOverMenu: gameManager not assigned");
    }
}
