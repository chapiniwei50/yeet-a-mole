// WalkerMonster.cs
using UnityEngine;

public class WalkerMonster : Monster
{
    [Header("Walker Specific Sounds")]
    public AudioClip[] footstepSounds;  // Faster footsteps

    protected override void Start()
    {
        monsterType = MonsterType.Walker;
        moveSpeed = 3.0f;
        hp = 1;
        damageOnBreach = 1;

        // Walker specific settings
        playMovementSounds = true;
        movementSoundInterval = 0.3f; // Fast footsteps
        minIdleSoundInterval = 2f;    // More frequent growls
        maxIdleSoundInterval = 5f;

        base.Start();
    }

    protected override void StartRepeatedSounds()
    {
        // Use walker-specific footstep sounds if provided
        if (footstepSounds != null && footstepSounds.Length > 0)
        {
            movementSounds = footstepSounds;
        }

        base.StartRepeatedSounds();
    }
}

