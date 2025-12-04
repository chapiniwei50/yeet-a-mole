using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Refs")]
    public WorldVariable worldVariable;
    public GameObject gameOverCanvasRoot;   // assign in Inspector

    [Header("Scene Names")]
    public string levelSceneName = "LevelScene";
    public string titleSceneName = "TitleScene";

    [Header("Audio")]
    public AudioClip gameOverClip;      // <- assign in Inspector
    [Range(0f, 1f)]
    public float gameOverVolume = 1f;   // <- tweak volume

    private bool isGameOver = false;

    void Start()
    {
        if (worldVariable == null)
            worldVariable = FindAnyObjectByType<WorldVariable>();

        // Hide Game Over UI at start
        if (gameOverCanvasRoot != null)
            gameOverCanvasRoot.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (isGameOver || worldVariable == null)
            return;

        if (worldVariable.playerHealth <= 0)
        {
            HandleGameOver();
        }
    }

    void HandleGameOver()
    {
        isGameOver = true;

        // Pause gameplay
        Time.timeScale = 0f;

        // Play game over sound
        PlayGameOverSound();

        // Show the Game Over canvas
        if (gameOverCanvasRoot != null)
            gameOverCanvasRoot.SetActive(true);
    }

    void PlayGameOverSound()
    {
        if (gameOverClip == null) return;

        // Try to play at the player's head position
        Transform cam = Camera.main != null
            ? Camera.main.transform
            : null;

        Vector3 pos = cam != null ? cam.position : Vector3.zero;

        AudioSource.PlayClipAtPoint(gameOverClip, pos, gameOverVolume);
    }

    // These are used by GameOverMenu:

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelSceneName);
    }

    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(titleSceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
