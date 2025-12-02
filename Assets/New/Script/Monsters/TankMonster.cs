using UnityEngine;
using System.Collections;

public class TankMonster : Monster
{
    [Header("Tank Settings")]
    public bool onlyDamagedByExplosives = true;

    [Header("Tank Specific Sounds")]
    public AudioClip[] heavyFootstepSounds;

    [Header("Tank Animation")]
    private MonsterAnimationController animationController; // Reusing same controller as Walker
    private bool isAttacking = false;

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

        // Initialize animation controller
        animationController = GetComponent<MonsterAnimationController>();
        if (animationController == null)
        {
            animationController = gameObject.AddComponent<MonsterAnimationController>();
        }

        // Make tank bigger
        transform.localScale = Vector3.one * 1.5f; // 50% bigger
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

    void Update()
    {
        if (hp <= 0 || isAttacking) return;

        HandleMovement();
        CheckForRingBreach();
    }

    protected override void HandleMovement()
    {
        if (playerCenter == null) return;

        Vector3 direction = (playerCenter.position - transform.position).normalized;
        direction.y = 0;

        transform.position += direction * moveSpeed * Time.deltaTime;

        FacePlayer();
    }

    protected override void CheckForRingBreach()
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

    protected override void BreachRing()
    {
        hasBreached = true;
        isAttacking = true;

        Debug.Log($"{monsterType} breached the ring! Playing attack animation...");

        // Stop movement
        moveSpeed = 0;

        // Play attack animation
        if (animationController != null)
        {
            animationController.PlayAttackAnimation();
        }
        else
        {
            // Fallback: immediate damage and destroy
            ApplyBreachDamage();
            Destroy(gameObject);
        }

        // Damage player immediately (or you can time it with animation)
        ApplyBreachDamage();

        // Destroy after attack animation
        Invoke("DestroyAfterAttack", 1.5f);
    }

    void DestroyAfterAttack()
    {
        Destroy(gameObject);
    }

    void ApplyBreachDamage()
    {
        if (worldVariable != null)
        {
            worldVariable.playerHealth -= damageOnBreach;
            Debug.Log($"Player health (via WorldVariable): {worldVariable.playerHealth}");
        }
    }

    // Override OnCollisionEnter to handle MoleBall collisions
    void OnCollisionEnter(Collision collision)
    {
        MoleBall ball = collision.gameObject.GetComponent<MoleBall>();

        if (ball != null && ball.currentState == MoleBall.BallState.Yeeted)
        {
            HandleBallHit(ball);
        }
    }

    // Handle ball hits (separate from TutorialDummy's OnCollisionEnter)
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

    public override void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"Tank Hit! HP Left: {hp}");
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
}