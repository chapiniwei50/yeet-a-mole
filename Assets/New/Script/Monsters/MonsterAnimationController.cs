using UnityEngine;
using System.Collections;

public class MonsterAnimationController : MonoBehaviour
{
    [Header("Animation FBX Files")]
    public GameObject walkAnimationFBX;
    public GameObject attackAnimationFBX;
    public GameObject dieAnimationFBX;

    [Header("Animation Settings")]
    public float attackAnimationLength = 1.5f;
    public float deathAnimationLength = 2.0f;

    private GameObject currentModel;
    private Animation currentAnimation;
    private string currentState = "idle";
    private Monster monster;

    void Start()
    {
        monster = GetComponent<Monster>();
        PlayWalkAnimation();
    }

    public void PlayWalkAnimation()
    {
        if (currentState == "walk") return;

        SwitchAnimation("walk", walkAnimationFBX, true);
    }

    public void PlayAttackAnimation()
    {
        if (currentState == "attack") return;

        SwitchAnimation("attack", attackAnimationFBX, false);

        // After attack animation, the monster will be destroyed
        // So no need to return to walk
    }

    public void PlayDeathAnimation()
    {
        if (currentState == "die") return;

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

            Debug.Log($"Playing {newState} animation");
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
        // This allows for cleanup or effects
        monster.gameObject.SetActive(false);

        // Or destroy after a delay
        Destroy(gameObject, 0.5f);
    }

    public bool IsPlaying(string state)
    {
        return currentState == state;
    }
}