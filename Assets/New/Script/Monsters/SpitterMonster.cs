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

    // NEW: track dead state so we can block shooting / movement cleanly
    private bool isDead = false;

    protected override void Start()
    {
        // Set spitter properties
        monsterType = MonsterType.Spitter;

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
        if (playerCenter == null || isDead || hp <= 0) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerCenter.position);

        // Check if we're in shooting range
        bool nowInShootingRange = distanceToPlayer <= stoppingDistance;

        if (nowInShootingRange != isInShootingRange)
        {
            isInShootingRange = nowInShootingRange;
            // (animation choice handled below each frame)
        }

        if (distanceToPlayer > stoppingDistance)
        {
            // Move toward player
            Vector3 direction = (playerCenter.position - transform.position).normalized;
            direction.y = 0;
            transform.position += direction * moveSpeed * Time.deltaTime;
            isMoving = true;

            // Moving  play WALK (if not shooting)
            if (animationController != null && !animationController.IsShooting())
            {
                animationController.PlayWalkAnimation();
            }
        }
        else
        {
            // In range, stop moving
            isMoving = false;

            // Stopped and not shooting  play IDLE
            if (animationController != null && !animationController.IsShooting())
            {
                animationController.PlayIdleAnimation();
            }
        }

        // Always face the player
        FacePlayer();
    }


    // Override the shooting method to add animation
    protected override void Shoot()
    {
        // Hard guard: no shooting if dead or HP gone
        if (isDead || hp <= 0) return;

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

        // Check again in case we died while waiting
        if (isDead || hp <= 0) yield break;

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
            Invoke(nameof(PlayDelayedHiss), 0.5f);
        }
    }

    private void PlayDelayedHiss()
    {
        if (hp > 0 && !isDead && idleSounds != null && idleSounds.Length > 0)
        {
            PlayRandomSound(idleSounds);
        }
    }

    public override void TakeDamage(int damage)
    {
        if (isDead || hp <= 0) return;

        // Let Monster handle hit flash + hit/death sounds + hp logic
        Debug.Log($"Spitter took {damage} damage (HP before: {hp})");
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        if (isDead) return;
        isDead = true;

        // Stop all coroutines (including ShootRoutine, ShootAfterAnimationDelay, idle/movement sounds)
        StopAllCoroutines();

        // Stop movement
        moveSpeed = 0;
        isMoving = false;

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
        if (isDead) return;

        hasBreached = true;
        Debug.Log($"{monsterType} breached the ring!");

        // Stop everything
        isDead = true;
        moveSpeed = 0;
        isMoving = false;
        StopAllCoroutines();

        // Apply damage immediately
        if (worldVariable != null)
        {
            worldVariable.playerHealth -= damageOnBreach;
            Debug.Log($"Player health (via WorldVariable): {worldVariable.playerHealth}");
        }

        // Destroy immediately on breach (or you could play a special animation)
        Destroy(gameObject);
    }
}
