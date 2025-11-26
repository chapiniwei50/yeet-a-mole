using UnityEngine;
using UnityEngine.Audio;

public class Mole : MonoBehaviour
{
    public bool isExplosive;

    [Header("Mole Ball")]
    public GameObject moleBall;

    public WorldVariable worldVariable;

    [Header("Audio")]
    public AudioClip Explosion;

    [Header("VFX")]
    public GameObject explodeEffect;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        
        if (isExplosive)
        {
            if (timer > 10.0f) Explode();
        } else {
            if (timer > 30.0f) Destroy(gameObject);
        }
    }

    public void OnHit()
    {
        if (moleBall != null) Instantiate(moleBall, transform.position, transform.rotation);

        Destroy(gameObject);
    }

    public void Explode()
    {
        if (worldVariable != null) worldVariable.playerHealth -= 1;
        if (Explosion != null) AudioSource.PlayClipAtPoint(Explosion, transform.position, 1f);
        if (explodeEffect != null) Instantiate(explodeEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
