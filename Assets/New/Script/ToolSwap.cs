using UnityEngine;
using UnityEngine.InputSystem;

public class ToolSwap : MonoBehaviour
{
    [Header("Tools")]
    public GameObject bat;
    public GameObject shovel;
    public GameObject racket;

    [Header("Input")]
    // B button  -> bat
    public InputActionReference batAction;
    // A button  -> shovel
    public InputActionReference shovelAction;
    // Side trigger -> racket
    public InputActionReference racketAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetTool(bat);
    }

    void OnEnable()
    {
        if (batAction != null)
        {
            batAction.action.performed += OnBatPressed;
            batAction.action.Enable();
        }

        if (shovelAction != null)
        {
            shovelAction.action.performed += OnShovelPressed;
            shovelAction.action.Enable();
        }

        if (racketAction != null)
        {
            racketAction.action.performed += OnRacketPressed;
            racketAction.action.Enable();
        }
    }

    void OnDisable()
    {
        if (batAction != null)
            batAction.action.performed -= OnBatPressed;

        if (shovelAction != null)
            shovelAction.action.performed -= OnShovelPressed;

        if (racketAction != null)
            racketAction.action.performed -= OnRacketPressed;
    }

    void SetTool(GameObject tool)
    {
        if (bat != null) bat.SetActive(false);
        if (shovel != null) shovel.SetActive(false);
        if (racket != null) racket.SetActive(false);

        if (tool != null) tool.SetActive(true);
    }

    void OnBatPressed(InputAction.CallbackContext ctx)
    {
        SetTool(bat);
    }

    void OnShovelPressed(InputAction.CallbackContext ctx)
    {
        SetTool(shovel);
    }

    void OnRacketPressed(InputAction.CallbackContext ctx)
    {
        SetTool(racket);
    }
}
