using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject applePrefab;
    public GameObject bombPrefab; 
    public float minX = -740f, maxX = 740f; 
    public float minZ = -990f, maxZ = 990f; 
    public float spawnY = 0.5f; 
    public float bombSpawnInterval = 5f; 
    public int maxBombs = 1; 
    public float bombLifetime = 10f; 
    private GameObject currentApple;
    private List<BombInfo> activeBombs = new List<BombInfo>();
    private float lastBombSpawnTime = 0f;
    private int playerScore;
    private int lastScoreThreshold = 0;

    private class BombInfo
    {
        public GameObject bomb;
        public float spawnTime;

        public BombInfo(GameObject bomb, float spawnTime)
        {
            this.bomb = bomb;
            this.spawnTime = spawnTime;
        }
    }

    void Start()
    {
        SpawnApple();
        SpawnApple();
    }

    void Update()
    {
        playerScore = GameObject.FindWithTag("snakeHead")?.GetComponent<PlayerLogic>()?.score ?? 0;
        int newThreshold = lastScoreThreshold;
        if (playerScore > 75) newThreshold = 75;
        else if (playerScore > 65) newThreshold = 65;
        else if (playerScore > 55) newThreshold = 55;
        else if (playerScore > 45) newThreshold = 45;
        else if (playerScore > 35) newThreshold = 35;
        else if (playerScore > 25) newThreshold = 25;
        else if (playerScore > 10) newThreshold = 10;

        // Increment maxBombs only when a new threshold is crossed
        if (newThreshold > lastScoreThreshold)
        {
            maxBombs++;
            lastScoreThreshold = newThreshold;
        }

        if (currentApple == null)
        {
            SpawnApple();
        }

        if (Time.time - lastBombSpawnTime >= bombSpawnInterval && activeBombs.Count < maxBombs)
        {
            SpawnBomb();
            lastBombSpawnTime = Time.time;
        }


        for (int i = activeBombs.Count - 1; i >= 0; i--)
        {
            BombInfo bombInfo = activeBombs[i];
            if (bombInfo.bomb == null)
            {
                activeBombs.RemoveAt(i);
                continue;
            }

            if (Time.time >= bombInfo.spawnTime + bombLifetime)
            {
                Destroy(bombInfo.bomb);
                activeBombs.RemoveAt(i);
            }
        }
    }

    void SpawnApple()
    {
        Vector3 spawnPos;
        int maxTries = 100;
        int tries = 0;

        do
        {
            spawnPos = new Vector3(
                Random.Range(minX, maxX),
                spawnY,
                Random.Range(minZ, maxZ)
            );
            tries++;
        }
        while (!IsSpawnPositionClear(spawnPos) && tries < maxTries);

        currentApple = Instantiate(applePrefab, spawnPos, Quaternion.identity);
        currentApple.tag = "Apple";
    }


    void SpawnBomb()
    {
        Vector3 spawnPos;
        int maxTries = 100;
        int tries = 0;

        do
        {
            spawnPos = new Vector3(
                Random.Range(minX, maxX),
                spawnY,
                Random.Range(minZ, maxZ)
            );
            tries++;
        }
        while (!IsSpawnPositionClear(spawnPos) && tries < maxTries);

        GameObject bomb = Instantiate(bombPrefab, spawnPos, Quaternion.identity);
        bomb.tag = "Bomb";
        activeBombs.Add(new BombInfo(bomb, Time.time));
    }


    bool IsSpawnPositionClear(Vector3 position, float checkRadius = 0.5f)
    {
        Collider[] colliders = Physics.OverlapSphere(position, checkRadius);

        foreach (Collider col in colliders)
        {
            // Prevent spawn if touching snake, apple, or bomb
            if (col.CompareTag("snakeBody") || col.CompareTag("Player") || col.CompareTag("Apple") || col.CompareTag("Bomb"))
            {
                return false;
            }
        }
        return true;
    }

    //To set the spawn area properly
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 center = new Vector3((minX + maxX) / 2f, spawnY, (minZ + maxZ) / 2f);
        Vector3 size = new Vector3(Mathf.Abs(maxX - minX), 0.1f, Mathf.Abs(maxZ - minZ));

        Gizmos.DrawWireCube(center, size);
    }

}