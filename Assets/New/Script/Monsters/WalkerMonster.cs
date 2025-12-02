using UnityEngine;

public class WalkerMonster : Monster
{
    private WalkerAnimationController animationController;
    private bool isAttacking = false;

    protected override void Start()
    {
        monsterType = MonsterType.Walker;
        moveSpeed = 3.0f;
        hp = 1;
        damageOnBreach = 1;

        // Add animation controller if not present
        animationController = GetComponent<WalkerAnimationController>();
        if (animationController == null)
        {
            animationController = gameObject.AddComponent<WalkerAnimationController>();
        }

        base.Start();
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

        // Apply damage immediately (or you can time it with animation)
        if (worldVariable != null)
        {
            worldVariable.playerHealth -= damageOnBreach;
            Debug.Log($"Player health (via WorldVariable): {worldVariable.playerHealth}");
        }

        // Destroy after attack animation
        Invoke("DestroyAfterAttack", 1.5f);
    }

    void DestroyAfterAttack()
    {
        Destroy(gameObject);
    }

    public override void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"Walker Hit! HP Left: {hp}");
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

    // This is called by the animation controller after death animation
    public void BaseDie()
    {
        base.Die();
    }
}