using UnityEngine;
using UnityEngine.Audio;

public class Mole : MonoBehaviour
{
    public bool isExplosive;

    [Header("Mole Ball (Drop Item)")]
    public GameObject moleBall;

    [Header("References")]
    public WorldVariable worldVariable;
    // Added: need to get renderer for changing color
    public Renderer moleRenderer; 

    [Header("Audio")]
    public AudioClip Explosion;

    [Header("VFX")]
    public GameObject explodeEffect;

    [Header("Visual Settings (Blinking)")]
    public Color normalColor = Color.red;   // Red color for normal state
    public Color flashColor = Color.yellow; // Bright color for flashing (or white)
    public float explosionTime = 10.0f;     // Explosion time, aligned with logic below

    private float timer = 0f;
    private Color originalColor; // Store original color

    void Start()
    {
        // Auto-get renderer component
        if (moleRenderer == null) moleRenderer = GetComponent<Renderer>();
        
        // Record original color (prevent wrong color after blinking)
        if (moleRenderer != null)
        {
            // If it's explosive, use the assigned red; normal mole uses its material's color
            if (isExplosive) 
                originalColor = normalColor;
            else 
                originalColor = moleRenderer.material.color;
        }

        if (worldVariable == null)
        {
            worldVariable = FindAnyObjectByType<WorldVariable>();
        }

        if (worldVariable == null)
        {
            Debug.LogError("No WorldVariable found in the scene!");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        
        if (isExplosive)
        {
            // --- Added: handle blinking logic ---
            HandleBlinking();

            // original logic: explode after 10 seconds
            if (timer > explosionTime) Explode();
        } 
        else 
        {

            if (timer > 30.0f) Destroy(gameObject);
        }
    }

    // --- Added: core blinking algorithm ---
    void HandleBlinking()
    {
        if (moleRenderer == null) return;

        float baseSpeed = 2f;   // Base blinking speed
        float maxSpeed  = 8f;  // Ultimate max speed

        float blinkSpeed;

        // first 5 seconds: constant baseSpeed
        if (timer <= explosionTime - 5f)
        {
            blinkSpeed = baseSpeed;
        }
        else
        {
            // last 5 seconds: speed up linearly
            // timer:  explosionTime - 5  → explosionTime
            // factor: 0 → 1
            float factor = Mathf.InverseLerp(explosionTime - 5f, explosionTime, timer);
            blinkSpeed = Mathf.Lerp(baseSpeed, maxSpeed, factor);
        }

        // use PingPong to create a smooth flashing effect
        float t = Mathf.PingPong(timer * blinkSpeed, 1f);

        if (t > 0.5f)
        {
            moleRenderer.material.color = flashColor;
            moleRenderer.material.SetColor("_EmissionColor", flashColor * 2f); 
        }
        else
        {
            moleRenderer.material.color = normalColor;
            moleRenderer.material.SetColor("_EmissionColor", normalColor);
        }
    }


    public void OnHit()
    {
        if (moleBall != null) Instantiate(moleBall, transform.position, transform.rotation);

        Destroy(gameObject);
    }

    public void Explode()
    {
        if (worldVariable != null) worldVariable.playerHealth -= 1;
        if (Explosion != null) AudioSource.PlayClipAtPoint(Explosion, transform.position, 1f);
        if (explodeEffect != null) Instantiate(explodeEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
