using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class MoleBall : MonoBehaviour
{
    public enum BallState { Spawning, Held, Thrown, ReadyToYeet }
    
    [Header("Status")]
    public BallState currentState = BallState.Spawning;
    public int damage = 1;

    [Header("Visuals")]
    public Material normalMat; // Normal brown color
    public Material readyMat;  // Glowing yellow color (indicates it's ready to Yeet)

    private Rigidbody rb;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactable;
    private Renderer rend;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rend = GetComponent<Renderer>();

        // Subscribe to grab and release events
        interactable.selectEntered.AddListener(OnGrab);
        interactable.selectExited.AddListener(OnThrow);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        currentState = BallState.Held;
        // When grabbed, reset the material
        if(normalMat != null) rend.material = normalMat;
    }

    void OnThrow(SelectExitEventArgs args)
    {
        currentState = BallState.Thrown;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Core Logic: Only balls that are thrown and hit the ground become "bullets"
        if (currentState == BallState.Thrown && collision.gameObject.CompareTag("Ground"))
        {
            ActivateYeetState();
        }
    }

    void ActivateYeetState()
    {
        currentState = BallState.ReadyToYeet;
        Debug.Log("Ball is READY TO YEET!");
        
        // Visual feedback: change color
        if (readyMat != null) rend.material = readyMat;
        
    }
}