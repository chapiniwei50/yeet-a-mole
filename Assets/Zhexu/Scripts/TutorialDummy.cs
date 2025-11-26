using UnityEngine;
using System.Collections;

public class TutorialDummy : MonoBehaviour
{
    public int hp = 3;
    public bool isShooter = false; // Is this the shooter enemy in Room 4?
    public GameObject projectilePrefab; // Projectile Prefab
    public Transform firePoint; 		 // Firing position

    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if(rend) originalColor = rend.material.color;

        if (isShooter) StartCoroutine(ShootRoutine());
    }

    void OnCollisionEnter(Collision collision)
    {
        MoleBall ball = collision.gameObject.GetComponent<MoleBall>();
        
        // Condition: Must be a MoleBall and must be in the Yeeted state
        if (ball != null && ball.currentState == MoleBall.BallState.Yeeted)
        {
            TakeDamage(ball.damage);
            // Destroy the ball after hitting to prevent secondary damage
            Destroy(ball.gameObject); 
        }
    }

    void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"Dummy Hit! HP Left: {hp}");
        StartCoroutine(FlashRed());

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Dummy Defeated!");
        gameObject.SetActive(false);
        // We can notify the GameManager to open the door here later
    }

    IEnumerator FlashRed()
    {
        if(rend) rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        if(rend) rend.material.color = originalColor;
    }

    IEnumerator ShootRoutine()
    {
        while (hp > 0)
        {
            yield return new WaitForSeconds(3f); // Shoot once every 3 seconds
            if (projectilePrefab && firePoint)
            {
                Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            }
        }
    }
}