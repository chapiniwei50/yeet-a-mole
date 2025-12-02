

// TankMonster.cs
using UnityEngine;
using System.Collections;

public class TankMonster : Monster
{
    [Header("Tank Settings")]
    public bool onlyDamagedByExplosives = true;

    [Header("Tank Specific Sounds")]
    public AudioClip[] heavyFootstepSounds;

    protected override void Start()
    {
        monsterType = MonsterType.Tank;
        moveSpeed = 1.0f; // Slower movement
        hp = 3;
        damageOnBreach = 2;

        // Tank specific settings
        playMovementSounds = true;
        movementSoundInterval = 0.7f; // Slow, heavy footsteps
        minIdleSoundInterval = 4f;    // Deep, less frequent growls
        maxIdleSoundInterval = 10f;
        soundVolume = 1.0f; // Louder

        base.Start();
    }

    protected override void StartRepeatedSounds()
    {
        // Use tank-specific footstep sounds if provided
        if (heavyFootstepSounds != null && heavyFootstepSounds.Length > 0)
        {
            movementSounds = heavyFootstepSounds;
        }

        base.StartRepeatedSounds();
    }

    void OnCollisionEnter(Collision collision)
    {
        MoleBall ball = collision.gameObject.GetComponent<MoleBall>();

        if (ball != null && ball.currentState == MoleBall.BallState.Yeeted)
        {
            HandleBallHit(ball);
        }
    }

    public void HandleBallHit(MoleBall ball)
    {
        if (onlyDamagedByExplosives && !ball.isExplosive)
        {
            StartCoroutine(FlashResist());
            Debug.Log("Tank resisted non-explosive damage!");
        }
        else
        {
            // Apply damage
            TakeDamage(ball.damage);
            Debug.Log($"Tank took {ball.damage} damage (Explosive: {ball.isExplosive})");
        }

        // Destroy the ball after hitting
        Destroy(ball.gameObject);
    }

    private IEnumerator FlashResist()
    {
        if (rend)
        {
            Color original = rend.material.color;
            rend.material.color = Color.gray;
            yield return new WaitForSeconds(0.3f);
            if (rend) rend.material.color = original;
        }
    }
}

