using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class Mole : MonoBehaviour
{
    public bool isExplosive;

    [Header("Mole Ball (Drop Item)")]
    public GameObject moleBall;

    [Header("References")]
    public WorldVariable worldVariable;

    [Header("Animation")]
    public GameObject moleAppearAnimationFBX;
    public GameObject moleIdleAnimationFBX;

    [Header("Glow Settings - For Blinking")]
    public GameObject glowEffectPrefab;  // Particle system or glow sphere
    public Color normalGlowColor = Color.red;
    public Color warningGlowColor = Color.yellow;
    public float explosionTime = 10.0f;

    [Header("Audio")]
    public AudioClip Explosion;
    public AudioClip HitSound;

    [Header("VFX")]
    public GameObject explodeEffect;

    // Private references
    private MoleAnimationController animationController;
    private GameObject glowEffect;
    private Renderer glowRenderer;
    private float timer = 0f;
    private bool isBlinking = false;

    void Start()
    {
        // Get or add animation controller
        animationController = GetComponent<MoleAnimationController>();
        if (animationController == null)
        {
            animationController = gameObject.AddComponent<MoleAnimationController>();
            animationController.appearAnimationFBX = moleAppearAnimationFBX;
            animationController.idleAnimationFBX = moleIdleAnimationFBX;
        }

        // Create glow effect
        if (glowEffectPrefab != null && isExplosive)
        {
            glowEffect = Instantiate(glowEffectPrefab, transform);
            glowEffect.transform.localPosition = Vector3.zero;

            // Get renderer for color changes
            glowRenderer = glowEffect.GetComponent<Renderer>();
            if (glowRenderer != null)
            {
                glowRenderer.material.color = normalGlowColor;
            }
        }

        if (worldVariable == null)
        {
            worldVariable = FindAnyObjectByType<WorldVariable>();
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isExplosive)
        {
            // Update blinking speed based on timer
            if (timer >= explosionTime - 5f && !isBlinking)
            {
                StartBlinking();
            }

            if (timer > explosionTime)
            {
                Explode();
            }
        }
        else
        {
            if (timer > 30.0f) Destroy(gameObject);
        }
    }

    void StartBlinking()
    {
        isBlinking = true;
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        float baseSpeed = 2f;
        float maxSpeed = 8f;

        while (isBlinking && glowRenderer != null)
        {
            // Calculate current blink speed (faster as explosion approaches)
            float blinkSpeed;
            if (timer <= explosionTime - 5f)
            {
                blinkSpeed = baseSpeed;
            }
            else
            {
                float factor = Mathf.InverseLerp(explosionTime - 5f, explosionTime, timer);
                blinkSpeed = Mathf.Lerp(baseSpeed, maxSpeed, factor);
            }

            // Switch between colors
            if (glowRenderer != null)
            {
                if (glowRenderer.material.color == normalGlowColor)
                {
                    glowRenderer.material.color = warningGlowColor;

                    // Make emission glow brighter
                    if (glowRenderer.material.HasProperty("_EmissionColor"))
                    {
                        glowRenderer.material.SetColor("_EmissionColor", warningGlowColor * 3f);
                    }
                }
                else
                {
                    glowRenderer.material.color = normalGlowColor;
                    if (glowRenderer.material.HasProperty("_EmissionColor"))
                    {
                        glowRenderer.material.SetColor("_EmissionColor", normalGlowColor);
                    }
                }
            }

            yield return new WaitForSeconds(0.5f / blinkSpeed);
        }
    }

    public void OnHit()
    {
        // Play hit sound
        if (HitSound != null)
        {
            AudioSource.PlayClipAtPoint(HitSound, transform.position, 1f);
        }

        // Drop mole ball
        if (moleBall != null)
        {
            Instantiate(moleBall, transform.position + Vector3.up, transform.rotation);
        }

        Destroy(gameObject);
    }

    public void Explode()
    {
        isBlinking = false;

        // Apply damage
        if (worldVariable != null)
        {
            worldVariable.playerHealth -= 1;
        }

        // Play explosion sound
        if (Explosion != null)
        {
            AudioSource.PlayClipAtPoint(Explosion, transform.position, 1f);
        }

        // Spawn explosion effect
        if (explodeEffect != null)
        {
            Instantiate(explodeEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}