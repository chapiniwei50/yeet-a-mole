using UnityEngine;
using System.Collections;

public class SpitterAnimationController : MonoBehaviour
{
    [Header("Animation FBX Files")]
    public GameObject spawnAnimationFBX;      // NEW: spawn (rise from ground)
    public GameObject walkAnimationFBX;
    public GameObject idleAnimationFBX;       // Idle animation when stopped
    public GameObject shootAnimationFBX;
    public GameObject dieAnimationFBX;

    [Header("Animation Timing")]
    public float spawnAnimationLength = 1.0f; // NEW
    public float shootAnimationLength = 1.0f;
    public float deathAnimationLength = 2.0f;
    public float returnToIdleDelay = 0.3f;    // Delay after shooting before returning to idle

    private GameObject currentModel;
    private Animation currentAnimation;
    private string currentState = "idle";
    private Monster monster;
    private Coroutine shootCoroutine;
    private bool isShooting = false;
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
            // Fallback: start in walk
            PlayWalkAnimation();
        }
    }

    // ---------------- SPAWN ----------------

    public void PlaySpawnAnimation()
    {
        if (isSpawning) return;

        isSpawning = true;
        SwitchAnimation("spawn", spawnAnimationFBX, false);
        StartCoroutine(SpawnThenWalk());
    }

    private IEnumerator SpawnThenWalk()
    {
        yield return new WaitForSeconds(spawnAnimationLength);

        isSpawning = false;

        // After spawn, by default we start walking (movement script may later switch to idle)
        PlayWalkAnimation();
    }

    // ---------------- WALK / IDLE ----------------

    public void PlayWalkAnimation()
    {
        if (currentState == "walk" || isShooting || isSpawning) return;

        SwitchAnimation("walk", walkAnimationFBX, true);
    }

    public void PlayIdleAnimation()
    {
        if (currentState == "idle" || isShooting || isSpawning) return;

        GameObject idleFBX = idleAnimationFBX != null ? idleAnimationFBX : walkAnimationFBX;
        SwitchAnimation("idle", idleFBX, true);
    }

    // ---------------- SHOOT ----------------

    public void PlayShootAnimation()
    {
        if (currentState == "shoot" || isShooting || isSpawning) return;

        isShooting = true;

        SwitchAnimation("shoot", shootAnimationFBX, false);

        if (shootCoroutine != null)
            StopCoroutine(shootCoroutine);

        shootCoroutine = StartCoroutine(ReturnToIdleAfterShoot());
    }

    IEnumerator ReturnToIdleAfterShoot()
    {
        yield return new WaitForSeconds(shootAnimationLength + returnToIdleDelay);

        isShooting = false;

        // When done shooting, default to idle (movement script may switch to walk if moving)
        PlayIdleAnimation();
    }

    // ---------------- DEATH ----------------

    public void PlayDeathAnimation()
    {
        if (currentState == "die") return;

        // Stop any shooting coroutine
        if (shootCoroutine != null)
            StopCoroutine(shootCoroutine);

        isSpawning = false;
        isShooting = false;

        SwitchAnimation("die", dieAnimationFBX, false);

        Invoke(nameof(DestroyAfterDeath), deathAnimationLength);
    }

    void DestroyAfterDeath()
    {
        if (monster != null)
        {
            monster.gameObject.SetActive(false);
        }

        Destroy(gameObject, 0.5f);
    }

    // ---------------- CORE SWITCH ----------------

    void SwitchAnimation(string newState, GameObject animationFBX, bool loop)
    {
        if (animationFBX == null)
        {
            Debug.LogWarning($"SpitterAnimationController: No FBX assigned for state {newState}");
            return;
        }

        // Clean up current animation model
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

            Debug.Log($"Spitter playing {newState} animation");
        }
        else
        {
            Debug.LogError($"No Animation component on {animationFBX.name}");
        }
    }

    // ---------------- HELPERS ----------------

    public bool IsPlaying(string state)
    {
        return currentState == state;
    }

    public bool IsShooting()
    {
        return isShooting;
    }
}
