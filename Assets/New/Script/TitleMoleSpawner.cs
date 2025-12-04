using UnityEngine;
using System.Collections.Generic;

public class TitleMoleSpawner : MonoBehaviour
{
    [Header("Mole Prefab (decorative)")]
    public GameObject titleMolePrefab;

    [Header("Spawn Area")]
    public Transform center;          // center of arena (usually same as player/XR Origin)
    public float minRadius = 3f;      // minimum distance from center/player
    public float maxRadius = 6f;      // maximum distance from center
    public float groundY = 0f;        // Y height of your ground

    [Header("Facing")]
    public Transform lookAtTarget;    // usually XR Origin; if null, falls back to center

    [Header("Spawn Timing")]
    public float minSpawnInterval = 1.5f;
    public float maxSpawnInterval = 4f;
    public int maxMoles = 10;

    private List<GameObject> spawnedMoles = new List<GameObject>();
    private float nextSpawnTime;

    void Start()
    {
        if (lookAtTarget == null)
            lookAtTarget = center;

        ScheduleNextSpawn();
    }

    void Update()
    {
        if (titleMolePrefab == null || center == null) return;
        if (spawnedMoles.Count >= maxMoles) return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnOneMole();
            ScheduleNextSpawn();
        }
    }

    void ScheduleNextSpawn()
    {
        float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
        nextSpawnTime = Time.time + interval;
    }

    void SpawnOneMole()
    {
        // random angle and radius BETWEEN min & max
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float radius = Random.Range(minRadius, maxRadius);

        Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
        Vector3 pos = center.position + offset;
        pos.y = groundY;

        // make mole face the player (or center)
        Quaternion rot = Quaternion.identity;
        if (lookAtTarget != null)
        {
            Vector3 dir = lookAtTarget.position - pos;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.001f)
                dir = Vector3.forward;   // fallback

            rot = Quaternion.LookRotation(dir);
        }
        else
        {
            rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        }

        GameObject mole = Instantiate(titleMolePrefab, pos, rot);
        spawnedMoles.Add(mole);
    }
}
