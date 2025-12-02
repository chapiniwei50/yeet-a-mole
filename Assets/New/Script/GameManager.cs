using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public WorldVariable worldVariable;
    public LevelWaveManager waveManager;

    private bool gameEnded = false;

    void Start()
    {
        // Find references if not set
        if (worldVariable == null)
            worldVariable = FindAnyObjectByType<WorldVariable>();

        if (waveManager == null)
            waveManager = FindAnyObjectByType<LevelWaveManager>();

        // Initialize health
        if (worldVariable != null)
        {
            worldVariable.playerHealth = 5; // Reset health
            Debug.Log("Game Started with 5 health");
        }
    }

    void Update()
    {
        if (gameEnded) return;

        CheckGameState();
    }

    void CheckGameState()
    {
        // Check for game over (player health)
        if (worldVariable != null && worldVariable.playerHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        if (gameEnded) return;

        gameEnded = true;
        Debug.Log("GAME OVER - Loading Game Over Scene");

        // Stop everything
        if (waveManager != null)
        {
            waveManager.StopAllCoroutines();
            waveManager.enabled = false;
        }

        // Load game over scene
        SceneController.Instance.LoadGameOver();
    }
}