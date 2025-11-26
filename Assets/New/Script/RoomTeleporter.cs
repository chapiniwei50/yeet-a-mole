using UnityEngine;
using UnityEngine.InputSystem;

public class RoomTeleporter : MonoBehaviour
{
    [Header("XR Origin (Player Rig)")]
    public GameObject xrOrigin;

    [Header("Teleport Input")]
    public InputActionReference teleportAction;

    [Header("Destination")]
    public Transform destinationB;
    public Transform destinationC;
    public Transform destinationD;

    public WorldVariable worldVariable;

    private void OnEnable()
    {
        teleportAction.action.performed += OnTeleportPressed;
        teleportAction.action.Enable();
    }

    private void OnDisable()
    {
        teleportAction.action.performed -= OnTeleportPressed;
        teleportAction.action.Disable();
    }

    private void OnTeleportPressed(InputAction.CallbackContext ctx)
    {
        if (xrOrigin != null && worldVariable != null)
        {
            if (worldVariable.tutorialStage == 1)
            {
                xrOrigin.transform.position = destinationB.position;
                xrOrigin.transform.rotation = destinationB.rotation;
                worldVariable.tutorialStage += 1;
            } 
            else if (worldVariable.tutorialStage == 2)
            {
                xrOrigin.transform.position = destinationC.position;
                xrOrigin.transform.rotation = destinationC.rotation;
                worldVariable.tutorialStage += 1;
            }
            else if (worldVariable.tutorialStage == 3)
            {
                xrOrigin.transform.position = destinationD.position;
                xrOrigin.transform.rotation = destinationD.rotation;
                worldVariable.tutorialStage += 1;
            }
        }
    }
}
