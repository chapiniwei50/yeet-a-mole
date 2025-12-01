using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 5f;
    public int damage = 1;

    private WorldVariable worldVariable; // Reference to global variable

    void Start()
    {
        // 1. Auto-destroy
        Destroy(gameObject, lifetime);

        // 2. Find global variable (for dealing damage)
        worldVariable = FindAnyObjectByType<WorldVariable>();

        // 3. Find player and aim (assume the player's head is Main Camera)
        // Remember to set the Main Camera under XR Origin to Tag "MainCamera" (Unity's default)
        if (Camera.main != null)
        {
            // Make the projectile face the camera (player's head)
            transform.LookAt(Camera.main.transform);
        }
    }

    void Update()
    {
        // Since LookAt was already done in Start, just move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if it hit the player
        // For safety, detect "Player" Tag or "MainCamera" Tag
        if (other.CompareTag("Player") || other.CompareTag("MainCamera"))
        {
            Debug.Log("Player hit!");
            
            // Damage logic
            if (worldVariable != null)
            {
                worldVariable.playerHealth -= damage;
                Debug.Log($"Current health: {worldVariable.playerHealth}");
            }

            Destroy(gameObject);
        }
        // Destroy when hitting wall/floor
        else if (other.CompareTag("Ground") || other.CompareTag("Untagged")) 
        {
            Destroy(gameObject);
        }
    }
}
