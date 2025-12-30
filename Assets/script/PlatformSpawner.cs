using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{

    [Header("Final Platform")]
    public GameObject finalPlatformPrefab;
    [Header("Final Platform Settings")]
    public float finalPlatformY = 2.0f;


    private bool finalPlatformSpawned = false;

    [Header("Win Condition")]
    public int maxPlatforms = 30;

    public float winOffset = 2f;

    [Header("Final Platform")]



    private int spawnedCount = 0;
    private bool gameWon = false;


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

        for (int i = 0; i < 5 && spawnedCount < maxPlatforms; i++)
        {
            SpawnPlatform();
        }
    }


    void Update()
    {
        if (spawnedCount < maxPlatforms &&
            player.position.x + spawnDistanceAhead > lastSpawnX)
        {
            SpawnPlatform();
        }
    }



    void SpawnPlatform()
    {
        // Spawn pendulums first
        if (spawnedCount < maxPlatforms)
        {
            lastSpawnX += distanceBetweenPlatforms;

            Vector3 spawnPos = new Vector3(
                lastSpawnX,
                platformY,
                baseZ
            );

            Instantiate(pendulumPrefab, spawnPos, Quaternion.identity);
            spawnedCount++;

            // If this was the LAST pendulum, immediately spawn final platform
            if (spawnedCount == maxPlatforms)
            {
                SpawnFinalPlatform();
            }

            return;
        }
    }


    void SpawnFinalPlatform()
{
    if (finalPlatformSpawned)
        return;

    lastSpawnX += distanceBetweenPlatforms;

    Vector3 finalPos = new Vector3(
        lastSpawnX,
        finalPlatformY, // ABSOLUTE Y, not relative
        baseZ
    );

    Instantiate(finalPlatformPrefab, finalPos, Quaternion.identity);
    finalPlatformSpawned = true;

    Debug.Log("Final platform spawned at Y = " + finalPlatformY);
}



    void CheckWinCondition()
    {
        if (spawnedCount < maxPlatforms)
            return;

        float lastPlatformX = lastSpawnX;

        if (player.position.x > lastPlatformX + winOffset)
        {
            gameWon = true;
            OnWin();
        }
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


    void OnWin()
    {
        Debug.Log("YOU WIN!");
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

}
