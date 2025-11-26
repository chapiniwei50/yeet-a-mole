using UnityEngine;

public class Room1 : MonoBehaviour
{
    public GameObject mole;
    public Transform spawnPoint;
    public WorldVariable worldVariable;

    private float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f && worldVariable.tutorialStage == 1)
        {
            SpawnMole();
            timer = 15f;
        }
    }
    private void SpawnMole()
    {
        if (mole != null && spawnPoint != null)
        {
            Instantiate(mole, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
