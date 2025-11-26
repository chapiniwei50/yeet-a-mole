using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class ToolHitDetector : MonoBehaviour
{
    public enum SwingType
    {
        Any,
        DownOnly,
        UpOnly,
        ForwardOnly
    }

    [Header("Target")]
    public string targetTag;

    [Header("Haptic target")]
    public XRInputHapticImpulseProvider rightControllerHaptic;

    [Header("Input")]
    // Right trigger action (pressed while swinging)
    public InputActionReference triggerAction;

    [Header("Swing")]
    public SwingType swingType = SwingType.Any;
    public float minSwingSpeed = 0.5f;

    [Header("Audio")]
    public AudioClip hitClip;

    private AudioSource audioSource;
    private Vector3 lastPosition;
    private Vector3 velocity;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (triggerAction != null)
        {
            triggerAction.action.Enable();
        }

        lastPosition = transform.position;
    }

    private void OnDisable()
    {
        if (triggerAction != null)
        {
            triggerAction.action.Disable();
        }
    }

    private void Update()
    {
        // Estimate swing velocity based on tool position
        Vector3 currentPos = transform.position;
        velocity = (currentPos - lastPosition) / Mathf.Max(Time.deltaTime, 0.0001f);
        lastPosition = currentPos;
    }

    private bool IsTriggerHeld()
    {
        if (triggerAction == null)
            return false;

        return triggerAction.action.IsPressed();
    }

    private bool IsSwingDirectionValid()
    {
        switch (swingType)
        {
            case SwingType.DownOnly:
                // Negative Y = moving downward
                return velocity.y <= -minSwingSpeed;

            case SwingType.UpOnly:
                // Positive Y = moving upward
                return velocity.y >= minSwingSpeed;

            case SwingType.ForwardOnly:
                // Negative X = moving forward
                return velocity.x <= -minSwingSpeed;

            case SwingType.Any:
            default:
                return true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag))
            return;

        // Must be holding trigger
        if (!IsTriggerHeld())
            return;

        // Must be swinging in the correct direction
        if (!IsSwingDirectionValid() && swingType!= SwingType.ForwardOnly)
            return;

        // Haptics
        if (rightControllerHaptic != null)
        {
            rightControllerHaptic.GetChannelGroup()?.GetChannel()?.SendHapticImpulse(0.7f, 0.15f, 0.0f);
        }

        // Sound
        if (audioSource != null && hitClip != null)
        {
            audioSource.PlayOneShot(hitClip);
        }

        if (swingType != SwingType.ForwardOnly)
        {
            Mole m = other.GetComponent<Mole>();
            if (m != null)
            {
                if (m.isExplosive && swingType == SwingType.DownOnly)
                {
                    m.Explode();
                }
                else
                {
                    m.OnHit();
                }
            }
        } else {
            // get ball rigidbody
            Rigidbody ballRb = other.attachedRigidbody;
            if (ballRb == null)
                return;

            // launch ball in direction of racket swing
            Vector3 dir = velocity.normalized;
            float launchSpeed = velocity.magnitude * 2.0f;
            ballRb.linearVelocity = dir * launchSpeed;

            // tell launched ball to set its state to "Yeeted"
            MoleBall b = other.GetComponent<MoleBall>();
            if (b != null) b.yeeted();
        }
        
    }
}
