using UnityEngine;

public class MoleAnimationController : MonoBehaviour
{
    [Header("Animation FBX")]
    public GameObject appearAnimationFBX;  // Appear from ground animation
    public GameObject idleAnimationFBX;    // Idle above ground animation

    [Header("Animation Timing")]
    public float appearAnimationLength = 1.0f;

    private GameObject currentModel;
    private Animation currentAnimation;
    private Mole mole;

    void Start()
    {
        mole = GetComponent<Mole>();
        PlayAppearAnimation();
    }

    public void PlayAppearAnimation()
    {
        SwitchAnimation("appear", appearAnimationFBX, false);

        // After appear animation, switch to idle
        Invoke("PlayIdleAnimation", appearAnimationLength);
    }

    public void PlayIdleAnimation()
    {
        SwitchAnimation("idle", idleAnimationFBX, true);
    }

    void SwitchAnimation(string animationName, GameObject animationFBX, bool loop)
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

            // Set loop mode
            foreach (AnimationState state in currentAnimation)
            {
                state.wrapMode = loop ? WrapMode.Loop : WrapMode.Once;
            }

            // Play animation
            currentAnimation.Play();
        }
        else
        {
            Debug.LogError($"No Animation component on {animationFBX.name}");
        }
    }

    // Get the renderer from the current animated model
    public Renderer GetModelRenderer()
    {
        if (currentModel != null)
        {
            return currentModel.GetComponentInChildren<Renderer>();
        }
        return null;
    }
}