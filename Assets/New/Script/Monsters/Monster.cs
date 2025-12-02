using UnityEngine;
using System.Collections;

public class Monster : TutorialDummy
{
    [Header("Monster Movement Settings")]
    public float moveSpeed = 2.0f;
    public int damageOnBreach = 1;
    public float defenseRingRadius = 5f;

    [Header("Monster Type")]
    public MonsterType monsterType;

    [Header("Monster Sounds")]
    public AudioClip spawnSound;

    [Header("Repeated Sounds")]
    public AudioClip[] idleSounds;           // Random idle sounds (growls, etc.)
    public AudioClip[] movementSounds;       // Footstep or movement sounds
    public float minIdleSoundInterval = 3f;
    public float maxIdleSoundInterval = 8f;
    public float movementSoundInterval = 0.5f; // For footsteps
    public bool playMovementSounds = false;

    [Header("Sound Settings")]
    public float soundVolume = 0.7f;
    public float spatialBlend = 1.0f;        // 0 = 2D, 1 = 3D

    protected bool hasBreached = false;
    protected Transform playerCenter;
    protected WorldVariable worldVariable;
    protected Coroutine idleSoundRoutine;
    protected Coroutine movementSoundRoutine;
    protected bool isMoving = true; // Monsters are always moving toward player

    public enum MonsterType
    {
        Walker,
        Spitter,
        Tank
    }

    protected override void Start()
    {
        // Find WorldVariable for player health
        worldVariable = FindAnyObjectByType<WorldVariable>();

        // Find player center (XR Origin)
        GameObject xrOrigin = GameObject.Find("XR Origin Hands (XR Rig)");
        if (xrOrigin != null)
        {
            playerCenter = xrOrigin.transform;
        }

        base.Start();

        // Configure AudioSource
        if (audioSource != null)
        {
            audioSource.spatialBlend = spatialBlend;
            audioSource.volume = soundVolume;
        }

        // Play spawn sound
        PlaySpawnSound();

        // Start repeated sound coroutines
        StartRepeatedSounds();

        Debug.Log($"{monsterType} monster spawned at {transform.position}");
    }

    protected virtual void StartRepeatedSounds()
    {
        // Start idle sounds (random intervals)
        if (idleSounds != null && idleSounds.Length > 0)
        {
            idleSoundRoutine = StartCoroutine(IdleSoundRoutine());
        }

        // Start movement sounds (regular intervals)
        if (playMovementSounds && movementSounds != null && movementSounds.Length > 0)
        {
            movementSoundRoutine = StartCoroutine(MovementSoundRoutine());
        }
    }

    protected virtual IEnumerator IdleSoundRoutine()
    {
        while (hp > 0)
        {
            // Wait random interval
            float waitTime = Random.Range(minIdleSoundInterval, maxIdleSoundInterval);
            yield return new WaitForSeconds(waitTime);

            // Play random idle sound
            PlayRandomSound(idleSounds);
        }
    }

    protected virtual IEnumerator MovementSoundRoutine()
    {
        while (hp > 0)
        {
            // Only play movement sound if actually moving
            if (isMoving && hp > 0)
            {
                PlayRandomSound(movementSounds);
            }

            // Wait for next movement sound
            yield return new WaitForSeconds(movementSoundInterval);
        }
    }

    protected virtual void PlayRandomSound(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0 || audioSource == null || hp <= 0)
            return;

    

        // Pick random sound
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        if (clip != null)
        {
            // Randomize pitch slightly for variety
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(clip);
            // Reset pitch
            audioSource.pitch = 1.0f;
            Debug.Log($"Playing sound: {clip.name}");
        }
    }

    protected virtual void PlaySpawnSound()
    {
        if (spawnSound != null && audioSource != null)
        {
            audioSource.pitch = 1.0f;
            audioSource.PlayOneShot(spawnSound);
        }
    }

    void Update()
    {
        if (hp <= 0) return;

        HandleMovement();
        CheckForRingBreach();
    }

    protected virtual void HandleMovement()
    {
        if (playerCenter == null) return;

        Vector3 direction = (playerCenter.position - transform.position).normalized;
        direction.y = 0;

        transform.position += direction * moveSpeed * Time.deltaTime;

        FacePlayer();

        // Update movement state (can be used to control sound)
        isMoving = direction.magnitude > 0.1f;
    }

    protected void FacePlayer()
    {
        if (playerCenter == null) return;

        Vector3 lookDirection = (playerCenter.position - transform.position).normalized;
        lookDirection.y = 0;

        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    protected virtual void CheckForRingBreach()
    {
        if (hasBreached || playerCenter == null) return;

        Vector2 monsterPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 centerPos = new Vector2(playerCenter.position.x, playerCenter.position.z);
        float distanceFromCenter = Vector2.Distance(monsterPos, centerPos);

        if (distanceFromCenter <= defenseRingRadius)
        {
            BreachRing();
        }
    }

    protected virtual void BreachRing()
    {
        hasBreached = true;
        Debug.Log($"{monsterType} breached the ring!");

        if (worldVariable != null)
        {
            worldVariable.playerHealth -= damageOnBreach;
            Debug.Log($"Player health (via WorldVariable): {worldVariable.playerHealth}");
        }
        else
        {
            Debug.LogWarning("WorldVariable not found to damage player!");
        }

        // Stop all sounds before destroying
        StopAllCoroutines();
        Destroy(gameObject);
    }

    protected override void Die()
    {
        // Stop sound coroutines
        if (idleSoundRoutine != null)
            StopCoroutine(idleSoundRoutine);
        if (movementSoundRoutine != null)
            StopCoroutine(movementSoundRoutine);

        base.Die();
    }
}