using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform spawnStartPoint;

    [Header("Platform Prefabs")]
    public GameObject pendulumPrefab;
    public GameObject rotatingPlatformPrefab;
    public GameObject finalPlatformPrefab;

    [Header("Difficulty")]
    [Range(0f, 1f)]
    public float rotatingPlatformChance = 0.3f;

    [Header("Spawn Settings")]
    public float spawnDistanceAhead = 15f;
    public float distanceBetweenPlatforms = 6f;
    public float platformY = 2.5f;
    public float baseZ = 0f;

    [Header("Final Platform Settings")]
    public float finalPlatformY = 2.0f;

    [Header("Win Condition")]
    public int maxPlatforms = 30;

    [Header("Cleanup")]
    public float destroyDistanceBehind = 10f;

    // Internal state
    private float lastSpawnX;
    private int spawnedCount = 0;
    private bool finalPlatformSpawned = false;
    private bool lastWasRotating = false;

    private List<GameObject> spawnedPlatforms = new List<GameObject>();

    void Start()
    {
        ResetSpawner();
    }

    void Update()
    {
        if (spawnedCount < maxPlatforms &&
            player.position.x + spawnDistanceAhead > lastSpawnX)
        {
            SpawnPlatform();
        }

        CleanupPlatforms();
    }

    void SpawnPlatform()
    {
        if (spawnedCount >= maxPlatforms)
            return;

        lastSpawnX += distanceBetweenPlatforms;

        Vector3 spawnPos = new Vector3(
            lastSpawnX,
            platformY,
            baseZ
        );

        GameObject platformToSpawn;

        // Balancing rule: never spawn two rotating platforms in a row
        if (lastWasRotating)
        {
            platformToSpawn = pendulumPrefab;
            lastWasRotating = false;
        }
        else
        {
            if (Random.value < rotatingPlatformChance)
            {
                platformToSpawn = rotatingPlatformPrefab;
                lastWasRotating = true;
            }
            else
            {
                platformToSpawn = pendulumPrefab;
                lastWasRotating = false;
            }
        }

        GameObject platform = Instantiate(platformToSpawn, spawnPos, Quaternion.identity);
        spawnedPlatforms.Add(platform);
        spawnedCount++;

        // Spawn final platform immediately after last normal platform
        if (spawnedCount == maxPlatforms)
        {
            SpawnFinalPlatform();
        }
    }

    void SpawnFinalPlatform()
    {
        if (finalPlatformSpawned)
            return;

        lastSpawnX += distanceBetweenPlatforms;

        Vector3 finalPos = new Vector3(
            lastSpawnX,
            finalPlatformY,
            baseZ
        );

        Instantiate(finalPlatformPrefab, finalPos, Quaternion.identity);
        finalPlatformSpawned = true;
    }

    void CleanupPlatforms()
    {
        for (int i = spawnedPlatforms.Count - 1; i >= 0; i--)
        {
            if (spawnedPlatforms[i] != null &&
                spawnedPlatforms[i].transform.position.x <
                player.position.x - destroyDistanceBehind)
            {
                Destroy(spawnedPlatforms[i]);
                spawnedPlatforms.RemoveAt(i);
            }
        }
    }

    // Called by PlayerController when the player respawns
    public void ResetSpawner()
    {
        // Destroy existing platforms
        for (int i = 0; i < spawnedPlatforms.Count; i++)
        {
            if (spawnedPlatforms[i] != null)
            {
                Destroy(spawnedPlatforms[i]);
            }
        }

        spawnedPlatforms.Clear();

        // Reset state
        spawnedCount = 0;
        lastSpawnX = spawnStartPoint.position.x;
        finalPlatformSpawned = false;
        lastWasRotating = false;

        // Spawn initial platforms
        for (int i = 0; i < 5 && spawnedCount < maxPlatforms; i++)
        {
            SpawnPlatform();
        }
    }
}
