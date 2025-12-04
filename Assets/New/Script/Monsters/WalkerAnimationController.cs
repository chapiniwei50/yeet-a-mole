using UnityEngine;
using System.Collections;

public class WalkerAnimationController : MonoBehaviour
{
    [Header("Animation FBX Files")]
    public GameObject spawnAnimationFBX;   // NEW: spawn (rise from ground)
    public GameObject walkAnimationFBX;
    public GameObject attackAnimationFBX;
    public GameObject dieAnimationFBX;

    [Header("Animation Settings")]
    public float spawnAnimationLength = 1.0f;   // NEW: how long the spawn clip is
    public float attackAnimationLength = 1.5f;
    public float deathAnimationLength = 2.0f;

    private GameObject currentModel;
    private Animation currentAnimation;
    private string currentState = "idle";
    private Monster monster;
    private bool isSpawning = false;

    void Start()
    {
        monster = GetComponent<Monster>();

        // If we have a spawn animation, play that first
        if (spawnAnimationFBX != null && spawnAnimationLength > 0f)
        {
            PlaySpawnAnimation();
        }
        else
        {
            // Fallback: go straight to walk if no spawn animation assigned
            PlayWalkAnimation();
        }
    }

    // ---------------- SPAWN ----------------

    public void PlaySpawnAnimation()
    {
        if (isSpawning) return;

        isSpawning = true;
        SwitchAnimation("spawn", spawnAnimationFBX, false);

        // After spawn animation finishes, go into walk loop
        StartCoroutine(SpawnThenWalk());
    }

    private IEnumerator SpawnThenWalk()
    {
        yield return new WaitForSeconds(spawnAnimationLength);

        isSpawning = false;
        PlayWalkAnimation();
    }

    // ---------------- WALK ----------------

    public void PlayWalkAnimation()
    {
        if (currentState == "walk" || isSpawning) return;

        SwitchAnimation("walk", walkAnimationFBX, true);
    }

    // ---------------- ATTACK ----------------

    public void PlayAttackAnimation()
    {
        if (currentState == "attack" || isSpawning) return;

        SwitchAnimation("attack", attackAnimationFBX, false);

        // After attack animation, the monster will be destroyed
        Invoke(nameof(DestroyAfterAttack), attackAnimationLength);
    }

    // ---------------- DEATH ----------------

    public void PlayDeathAnimation()
    {
        if (currentState == "die") return;

        // If we're in the middle of spawning, stop that state
        isSpawning = false;

        SwitchAnimation("die", dieAnimationFBX, false);

        // Destroy after death animation
        Invoke(nameof(DestroyAfterDeath), deathAnimationLength);
    }

    // ---------------- CORE SWITCH ----------------

    void SwitchAnimation(string newState, GameObject animationFBX, bool loop)
    {
        if (animationFBX == null)
        {
            Debug.LogWarning($"WalkerAnimationController: No FBX assigned for state '{newState}'");
            return;
        }

        // Clean up current animation
        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        // Instantiate new animation model
        currentModel = Instantiate(animationFBX, transform);
        currentModel.transform.localPosition = Vector3.zero;
        currentModel.transform.localRotation = Quaternion.identity;

        // Get Animation component
        currentAnimation = currentModel.GetComponent<Animation>();
        if (currentAnimation == null)
        {
            currentAnimation = currentModel.GetComponentInChildren<Animation>();
        }

        if (currentAnimation != null)
        {
            currentAnimation.playAutomatically = true;
            currentAnimation.cullingType = AnimationCullingType.AlwaysAnimate;

            foreach (AnimationState state in currentAnimation)
            {
                state.wrapMode = loop ? WrapMode.Loop : WrapMode.Once;
            }

            currentAnimation.Play();
            currentState = newState;

            Debug.Log($"Walker playing {newState} animation");
        }
        else
        {
            Debug.LogError($"No Animation component on {animationFBX.name}");
        }
    }

    void DestroyAfterAttack()
    {
        Destroy(gameObject);
    }

    void DestroyAfterDeath()
    {
        // Disable the monster but don't destroy immediately
        if (monster != null)
        {
            monster.gameObject.SetActive(false);
        }

        Destroy(gameObject, 0.5f);
    }

    public bool IsPlaying(string state)
    {
        return currentState == state;
    }
}
