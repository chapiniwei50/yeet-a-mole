using UnityEngine;
using System.Collections;

public class SpitterMonster : Monster
{
    [Header("Spitter Settings")]
    public float stoppingDistance = 10f;

    [Header("Spitter Specific Sounds")]
    public AudioClip[] spitterIdleSounds;
    public AudioClip[] spitterMovementSounds;

    [Header("Animation")]
    private SpitterAnimationController animationController;
    private bool isInShootingRange = false;

    protected override void Start()
    {
        // Set spitter properties
        monsterType = MonsterType.Spitter;
        moveSpeed = 1.5f;
        hp = 1;
        damageOnBreach = 1;

        // Enable shooting with TutorialDummy system
        isShooter = true;
        shootInterval = 2.5f; // Custom interval

        // Spitter specific settings
        playMovementSounds = true;
        movementSoundInterval = 0.4f; // Slick, slimy movement
        minIdleSoundInterval = 1.5f;  // Frequent hissing
        maxIdleSoundInterval = 4f;

        base.Start();

        // Initialize animation controller
        animationController = GetComponent<SpitterAnimationController>();
        if (animationController == null)
        {
            animationController = gameObject.AddComponent<SpitterAnimationController>();
        }
    }

    protected override void StartRepeatedSounds()
    {
        // Use spitter-specific sounds if provided
        if (spitterIdleSounds != null && spitterIdleSounds.Length > 0)
        {
            idleSounds = spitterIdleSounds;
        }

        if (spitterMovementSounds != null && spitterMovementSounds.Length > 0)
        {
            movementSounds = spitterMovementSounds;
        }

        base.StartRepeatedSounds();
    }

    protected override void HandleMovement()
    {
        if (playerCenter == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerCenter.position);

        // Check if we're in shooting range
        bool nowInShootingRange = distanceToPlayer <= stoppingDistance;

        // If we just entered or left shooting range, update animation
        if (nowInShootingRange != isInShootingRange)
        {
            isInShootingRange = nowInShootingRange;

            if (!isInShootingRange && animationController != null)
            {
                // Left shooting range, resume walking
                animationController.PlayWalkAnimation();
            }
        }

        // Stop at shooting distance
        if (distanceToPlayer > stoppingDistance)
        {
            // Move toward player
            Vector3 direction = (playerCenter.position - transform.position).normalized;
            direction.y = 0;
            transform.position += direction * moveSpeed * Time.deltaTime;
            isMoving = true;

            // Play walk animation if not already playing
            if (animationController != null && !animationController.IsShooting())
            {
                animationController.PlayWalkAnimation();
            }
        }
        else
        {
            // Close enough to shoot, stop moving
            isMoving = false;

            // Play walk animation (idle at shooting position)
            if (animationController != null && !animationController.IsShooting())
            {
                animationController.PlayWalkAnimation();
            }
        }

        // Always face the player
        FacePlayer();
    }

    // Override the shooting method to add animation
    protected override void Shoot()
    {
        // Play shoot animation
        if (animationController != null)
        {
            animationController.PlayShootAnimation();
        }

        // Wait for animation to reach shooting frame, then spawn projectile
        StartCoroutine(ShootAfterAnimationDelay(0.3f)); // Adjust timing based on your animation
    }

    IEnumerator ShootAfterAnimationDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Play shooting sound if available
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Spawn projectile
        if (projectilePrefab && firePoint)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
    }

    protected override void PlaySpawnSound()
    {
        // Spitter-specific spawn sound (optional override)
        base.PlaySpawnSound();

        // Add extra hiss on spawn
        if (audioSource != null && idleSounds != null && idleSounds.Length > 0)
        {
            // Play an extra idle sound immediately after spawn
            Invoke("PlayDelayedHiss", 0.5f);
        }
    }

    private void PlayDelayedHiss()
    {
        if (hp > 0 && idleSounds != null && idleSounds.Length > 0)
        {
            PlayRandomSound(idleSounds);
        }
    }

    public override void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"Spitter Hit! HP Left: {hp}");
        StartCoroutine(FlashRed());

        if (hp <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        // Stop movement
        moveSpeed = 0;

        // Play death animation
        if (animationController != null)
        {
            animationController.PlayDeathAnimation();
        }
        else
        {
            // Fallback if no animation controller
            base.Die();
        }
    }

    protected override void BreachRing()
    {
        hasBreached = true;
        Debug.Log($"{monsterType} breached the ring!");

        // Stop movement
        moveSpeed = 0;

        // Apply damage immediately
        if (worldVariable != null)
        {
            worldVariable.playerHealth -= damageOnBreach;
            Debug.Log($"Player health (via WorldVariable): {worldVariable.playerHealth}");
        }

        // Destroy immediately on breach (or play a breach animation if you have one)
        Destroy(gameObject);
    }
}