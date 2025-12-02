using UnityEngine;
using System.Collections;

public class SpitterAnimationController : MonoBehaviour
{
    [Header("Animation FBX Files")]
    public GameObject walkAnimationFBX;
    public GameObject shootAnimationFBX;
    public GameObject dieAnimationFBX;

    [Header("Animation Timing")]
    public float shootAnimationLength = 1.0f;
    public float deathAnimationLength = 2.0f;
    public float returnToWalkDelay = 0.5f; // Delay after shooting before returning to walk

    private GameObject currentModel;
    private Animation currentAnimation;
    private string currentState = "idle";
    private Monster monster;
    private Coroutine shootCoroutine;
    private bool isShooting = false;

    void Start()
    {
        monster = GetComponent<Monster>();
        PlayWalkAnimation();
    }

    public void PlayWalkAnimation()
    {
        if (currentState == "walk" || isShooting) return;

        SwitchAnimation("walk", walkAnimationFBX, true);
    }

    public void PlayShootAnimation()
    {
        if (currentState == "shoot" || isShooting) return;

        isShooting = true;

        // Stop walk animation
        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        // Play shoot animation
        SwitchAnimation("shoot", shootAnimationFBX, false);

        // Return to walk after shooting
        if (shootCoroutine != null)
            StopCoroutine(shootCoroutine);

        shootCoroutine = StartCoroutine(ReturnToWalkAfterShoot());
    }

    public void PlayDeathAnimation()
    {
        if (currentState == "die") return;

        // Stop any shooting coroutine
        if (shootCoroutine != null)
            StopCoroutine(shootCoroutine);

        SwitchAnimation("die", dieAnimationFBX, false);

        // Destroy after death animation
        Invoke("DestroyAfterDeath", deathAnimationLength);
    }

    void SwitchAnimation(string newState, GameObject animationFBX, bool loop)
    {
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
            // Configure animation
            currentAnimation.playAutomatically = true;
            currentAnimation.cullingType = AnimationCullingType.AlwaysAnimate;

            // Set loop mode
            foreach (AnimationState state in currentAnimation)
            {
                state.wrapMode = loop ? WrapMode.Loop : WrapMode.Once;
            }

            // Play animation
            currentAnimation.Play();
            currentState = newState;

            Debug.Log($"Spitter playing {newState} animation");
        }
        else
        {
            Debug.LogError($"No Animation component on {animationFBX.name}");
        }
    }

    IEnumerator ReturnToWalkAfterShoot()
    {
        // Wait for shoot animation to complete
        yield return new WaitForSeconds(shootAnimationLength);

        // Add a small delay before returning to walk
        yield return new WaitForSeconds(returnToWalkDelay);

        // Return to walk animation
        isShooting = false;
        PlayWalkAnimation();
    }

    void DestroyAfterDeath()
    {
        // Disable the monster
        monster.gameObject.SetActive(false);

        // Destroy after a delay
        Destroy(gameObject, 0.5f);
    }

    public bool IsPlaying(string state)
    {
        return currentState == state;
    }

    public bool IsShooting()
    {
        return isShooting;
    }
}