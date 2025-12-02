using UnityEngine;
using UnityEngine.UI;
using TMPro; // Reference TextMeshPro
using System.Collections;

public class HealthBarPro : MonoBehaviour
{
    [Header("References")]
    public WorldVariable worldVariable;
    public Image fillImage;
    public TextMeshProUGUI hpText; // Reference to TMP text component

    [Header("Settings")]
    public int maxHealth = 5;
    public float smoothSpeed = 5f; // Smooth animation speed for HP bar

    [Header("Animation")]
    public Transform container; // Object to apply shake animation (usually Canvas or BG)

    [Header("Sound")]
    public AudioClip hurtSound; // Sound to play when player takes damage

    private float targetFillAmount;
    private int currentDisplayHealth;
    private AudioSource audioSource; // Audio source for hurt sounds

    void Start()
    {
        if (worldVariable == null)
            worldVariable = FindAnyObjectByType<WorldVariable>();

        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Initialization
        currentDisplayHealth = worldVariable.playerHealth;
        targetFillAmount = (float)currentDisplayHealth / maxHealth;
        fillImage.fillAmount = targetFillAmount;
        UpdateText();
    }

    void Update()
    {
        if (worldVariable == null) return;

        // 1. Monitor HP changes
        int realHealth = worldVariable.playerHealth;

        // When actual HP changes (damage or healing)
        if (realHealth != currentDisplayHealth)
        {
            // If took damage (HP decreased)
            if (realHealth < currentDisplayHealth)
            {
                StartCoroutine(ShakeEffect()); // Trigger hit animation
                PlayHurtSound(); // Play hurt sound
            }

            currentDisplayHealth = realHealth;
            targetFillAmount = (float)currentDisplayHealth / maxHealth;
            UpdateText();
        }

        // 2. Smoothly move the HP bar (Lerp animation)
        // This line makes fillAmount gradually approach targetFillAmount
        fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFillAmount, Time.deltaTime * smoothSpeed);
    }

    void UpdateText()
    {
        if (hpText != null)
        {
            // Display format: "HP 3/5"
            hpText.text = $"HP {currentDisplayHealth}/{maxHealth}";

            // Only cartoon fonts with outline look good
            // if (currentDisplayHealth <= 1) hpText.color = Color.red; // Low HP turns red
            // else hpText.color = Color.white;
        }
    }

    // Play hurt sound when player takes damage
    void PlayHurtSound()
    {
        if (hurtSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }
    }

    // Simple damage shake animation
    IEnumerator ShakeEffect()
    {
        if (container == null) yield break;

        Vector3 originalPos = container.localPosition;
        float elapsed = 0.0f;
        float duration = 0.3f;
        float magnitude = 0.05f; // Shake intensity (small because Canvas scale is tiny)

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            container.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        container.localPosition = originalPos; // Reset position
    }
}