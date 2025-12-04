using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    [Header("Scene Names")]
    public string tutorialSceneName = "SampleScene";
    public string gameSceneName = "LevelScene";

    public void PlayTutorial()
    {
        SceneManager.LoadScene(tutorialSceneName);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        // So it does *something* in editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
