using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    public float speed = 4f; // Projectile speed
    public float lifetime = 5f; // Lifetime (prevents the projectile from flying forever)

    void Start()
    {
        // Automatically destroy after 5 seconds to save performance
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move "forward" every frame
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // Collision detection
    void OnTriggerEnter(Collider other)
    {
        // If it hits the Player or the Ground
        // Note: The player's head or hands usually have the "Player" Tag; if not, ignore the Tag check for now
        if (other.CompareTag("Player") || other.CompareTag("Ground")) 
        {
            Debug.Log("Projectile hit the target!");
            Destroy(gameObject); // Destroy the projectile itself
        }
    }
}