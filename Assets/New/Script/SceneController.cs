using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    // Singleton for easy access
    private static SceneController instance;

    [Header("Scene Transition Sounds")]
    public AudioClip gameOverSound;
    public float soundVolume = 1.0f;

    private AudioSource audioSource;

    public static SceneController Instance
    {
        get
        {
            if (instance == null)
            {
                // Create if doesn't exist
                GameObject obj = new GameObject("SceneController");
                instance = obj.AddComponent<SceneController>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Setup AudioSource
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.spatialBlend = 0f; // 2D sound
                audioSource.volume = soundVolume;
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Load level scene (gameplay)
    public void LoadLevel()
    {
        SceneManager.LoadScene("LevelScene");
    }

    // Load game over scene with sound
    public void LoadGameOver()
    {
        StartCoroutine(LoadGameOverWithSound());
    }

    // Coroutine to play sound then load scene
    private IEnumerator LoadGameOverWithSound()
    {
        // Play game over sound if available
        if (gameOverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(gameOverSound);
            Debug.Log("Playing game over sound");

            // Wait for sound to finish (or at least most of it)
            yield return new WaitForSeconds(gameOverSound.length * 0.8f);
        }
        else
        {
            // Small delay even if no sound
            yield return new WaitForSeconds(0.5f);
        }

        // Load the game over scene
        SceneManager.LoadScene("LevelScene"); 
    }

    // Alternative: Load any scene with optional sound
    public void LoadSceneWithSound(string sceneName, AudioClip sound = null)
    {
        StartCoroutine(LoadSceneWithSoundCoroutine(sceneName, sound));
    }

    private IEnumerator LoadSceneWithSoundCoroutine(string sceneName, AudioClip sound)
    {
        // Play sound if provided
        if (sound != null && audioSource != null)
        {
            audioSource.PlayOneShot(sound);
            yield return new WaitForSeconds(sound.length);
        }

        SceneManager.LoadScene(sceneName);
    }

    // Quit game
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}