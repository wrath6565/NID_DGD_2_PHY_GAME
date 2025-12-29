using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform spawnStartPoint;
    public GameObject pendulumPrefab;

    [Header("Spawn Settings")]
    public float spawnDistanceAhead = 15f;
    public float distanceBetweenPlatforms = 6f;

    [Tooltip("Fixed Y height for all pendulums")]
    public float platformY = 2.5f;

    [Tooltip("Base Z position for pendulums")]
    public float baseZ = 0f;

    [Tooltip("Optional small Z variation (set to 0 for none)")]
    public float zVariation = 0f;

    [Header("Cleanup")]
    public float destroyDistanceBehind = 10f;

    private float lastSpawnX;
    private List<GameObject> spawnedPlatforms = new List<GameObject>();

    void Start()
    {
        lastSpawnX = spawnStartPoint.position.x;

        // Initial spawn
        for (int i = 0; i < 5; i++)
        {
            SpawnPlatform();
        }
    }

    void Update()
    {
        if (player.position.x + spawnDistanceAhead > lastSpawnX)
        {
            SpawnPlatform();
        }

        CleanupPlatforms();
    }

    void SpawnPlatform()
    {
        lastSpawnX += distanceBetweenPlatforms;

        float zOffset = Random.Range(-zVariation, zVariation);

        Vector3 spawnPos = new Vector3(
            lastSpawnX,
            platformY,
            baseZ + zOffset
        );

        GameObject platform = Instantiate(pendulumPrefab, spawnPos, Quaternion.identity);
        spawnedPlatforms.Add(platform);
    }

    void CleanupPlatforms()
    {
        for (int i = spawnedPlatforms.Count - 1; i >= 0; i--)
        {
            if (spawnedPlatforms[i].transform.position.x < player.position.x - destroyDistanceBehind)
            {
                Destroy(spawnedPlatforms[i]);
                spawnedPlatforms.RemoveAt(i);
            }
        }
    }
}
