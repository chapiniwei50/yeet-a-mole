using UnityEngine;
using System.Collections;

public class MonsterAnimationController : MonoBehaviour
{
    [Header("Animation FBX Files")]
    public GameObject spawnAnimationFBX;      // spawn (rise from ground)
    public GameObject walkAnimationFBX;
    public GameObject attackAnimationFBX;
    public GameObject dieAnimationFBX;

    [Header("Animation Settings")]
    public float spawnAnimationLength = 1.0f;
    public float attackAnimationLength = 1.5f;
    public float deathAnimationLength = 2.0f;

    [Header("Position Offset")]
    public Vector3 animationOffset = Vector3.zero;   // <-- NEW: tweak in Inspector

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
            // Fallback: go straight to walk
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

        // Tank: caller (TankMonster) decides when to destroy,
        // so we don't auto-destroy here.
    }

    // ---------------- DEATH ----------------

    public void PlayDeathAnimation()
    {
        if (currentState == "die") return;

        isSpawning = false;

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
            Debug.LogWarning($"MonsterAnimationController: No FBX assigned for state {newState}");
            return;
        }

        // Clean up current animation
        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        // Instantiate new animation model
        currentModel = Instantiate(animationFBX, transform);

        // Apply offset here so you can line up the mesh with collider/hole
        currentModel.transform.localPosition = animationOffset;
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

            Debug.Log($"Monster playing {newState} animation");
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
}
