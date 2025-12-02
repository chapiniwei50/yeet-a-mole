using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Text gameOverText;
    public Button restartButton;
    public Button quitButton;

    [Header("Settings")]
    public string levelSceneName = "LevelScene";

    void Start()
    {
        SetupUI();
        SetupButtons();

        // Position UI for VR (optional)
        PositionUIForVR();
    }

    void SetupUI()
    {
        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER\nHealth Depleted!";
            gameOverText.color = Color.red;
        }
    }

    void SetupButtons()
    {
        // Restart Button
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartClicked);

            // Make button text clear
            Text buttonText = restartButton.GetComponentInChildren<Text>();
            if (buttonText != null)
                buttonText.text = "PLAY AGAIN";
        }
        else
        {
            Debug.LogWarning("Restart button not assigned!");
        }

        // Quit Button
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitClicked);

            // Make button text clear
            Text buttonText = quitButton.GetComponentInChildren<Text>();
            if (buttonText != null)
                buttonText.text = "QUIT";
        }
        else
        {
            Debug.LogWarning("Quit button not assigned!");
        }
    }

    void OnRestartClicked()
    {
        Debug.Log("Restart button clicked - Loading Level Scene");
        SceneController.Instance.LoadLevel();
    }

    void OnQuitClicked()
    {
        Debug.Log("Quit button clicked");
        SceneController.Instance.QuitGame();
    }

    void PositionUIForVR()
    {
        // Position canvas in front of player for VR
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
        {
            // Position 3 meters in front, 1.5 meters up (adjust as needed)
            canvas.transform.position = new Vector3(0, 1.5f, 3f);
            canvas.transform.rotation = Quaternion.Euler(0, 180, 0); // Face player
        }
    }

    // For testing without VR
    void Update()
    {
        // Debug controls
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnRestartClicked();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnQuitClicked();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            // Test button by simulating click
            if (restartButton != null)
                restartButton.onClick.Invoke();
        }
    }
}