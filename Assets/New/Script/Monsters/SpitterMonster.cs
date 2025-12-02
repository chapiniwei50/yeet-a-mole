

// SpitterMonster.cs
using UnityEngine;
using System.Collections;

public class SpitterMonster : Monster
{
    [Header("Spitter Settings")]
    public float stoppingDistance = 10f;

    [Header("Spitter Specific Sounds")]
    public AudioClip[] spitterIdleSounds;  // Hissing, bubbling sounds
    public AudioClip[] spitterMovementSounds;

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

        // Stop at shooting distance
        if (distanceToPlayer > stoppingDistance)
        {
            // Move toward player
            Vector3 direction = (playerCenter.position - transform.position).normalized;
            direction.y = 0;
            transform.position += direction * moveSpeed * Time.deltaTime;
            isMoving = true;
        }
        else
        {
            // Close enough to shoot, stop moving
            isMoving = false;
        }

        // Always face the player
        FacePlayer();
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
}