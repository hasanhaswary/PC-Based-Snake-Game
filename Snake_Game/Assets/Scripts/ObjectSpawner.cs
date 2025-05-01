using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject applePrefab;
    public GameObject bombPrefab;
    public float minX = -740f, maxX = 740f;
    public float minZ = -990f, maxZ = 990f;
    public float spawnY = 0.5f;
    public float bombSpawnInterval = 2f;
    public float bombLifetime = 10f; 
    private GameObject currentApple;
    private List<BombInfo> activeBombs = new List<BombInfo>();
    private int playerScore;
    private List<int> scoreThresholds = new List<int> { 5, 8, 11, 14, 17, 20, 23, 26, 29, 32, 35, 38, 41, 44, 47, 50, 53, 56, 59, 62, 65, 68, 71, 74 };
    private int lastThresholdCrossed;

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
        if (currentApple == null)
        {
            SpawnApple();
        }

        playerScore = GameObject.FindWithTag("snakeHead")?.GetComponent<PlayerLogic>()?.score ?? 0;

        if (scoreThresholds.Contains(playerScore) && playerScore != lastThresholdCrossed)
        {
            SpawnBomb();
            SpawnApple();
            lastThresholdCrossed = playerScore;
            Debug.Log($"Threshold = {lastThresholdCrossed} and playerScore = {playerScore}");
            
        }

        while (playerScore == lastThresholdCrossed)
        {
            SpawnApple();
            SpawnApple();
        }
    }

    void SpawnApple()
    {
        Vector3 spawnPos = GetValidSpawnPosition();
        if (spawnPos != Vector3.zero)
        {
            currentApple = Instantiate(applePrefab, spawnPos, Quaternion.identity);
            currentApple.tag = "Apple";
        }
        else
        {
            Debug.LogWarning("Failed to find a valid spawn position for apple. Forcing spawn.");
            spawnPos = new Vector3(Random.Range(minX, maxX), spawnY, Random.Range(minZ, maxZ));
            currentApple = Instantiate(applePrefab, spawnPos, Quaternion.identity);
            currentApple.tag = "Apple";
        }
    }

    void SpawnBomb()
    {
        Vector3 spawnPos = GetValidSpawnPosition();
        if (spawnPos != Vector3.zero)
        {
            GameObject bomb = Instantiate(bombPrefab, spawnPos, Quaternion.identity);
            bomb.tag = "Bomb";
            activeBombs.Add(new BombInfo(bomb, Time.time));
            Debug.Log($"Bomb Spawned at {spawnPos}, Active Bombs: {activeBombs.Count}");
        }
        else
        {
            Debug.LogWarning("Failed to find a valid spawn position for bomb. Forcing spawn.");
            spawnPos = new Vector3(Random.Range(minX, maxX), spawnY, Random.Range(minZ, maxZ));
            GameObject bomb = Instantiate(bombPrefab, spawnPos, Quaternion.identity);
            bomb.tag = "Bomb";
            activeBombs.Add(new BombInfo(bomb, Time.time));
        }
    }

    Vector3 GetValidSpawnPosition(float initialCheckRadius = 0.5f, int maxTries = 300) // Reduced initialCheckRadius, increased maxTries
    {
        Vector3 spawnPos = Vector3.zero;
        int tries = 0;
        float checkRadius = initialCheckRadius;

        do
        {
            spawnPos = new Vector3(
                Random.Range(minX, maxX),
                spawnY,
                Random.Range(minZ, maxZ)
            );
            tries++;
            if (tries > maxTries / 2 && checkRadius > 0.1f)
            {
                checkRadius *= 0.5f;
                Debug.Log($"Reduced checkRadius to {checkRadius} to find spawn position.");
            }
        }
        while (!IsSpawnPositionClear(spawnPos, checkRadius) && tries < maxTries);

        return tries < maxTries ? spawnPos : Vector3.zero;
    }

    bool IsSpawnPositionClear(Vector3 position, float checkRadius)
    {
        Collider[] colliders = Physics.OverlapSphere(position, checkRadius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("snakeBody") || col.CompareTag("snakeHead") || col.CompareTag("Apple") || col.CompareTag("Bomb"))
            {
                return false;
            }
        }
        return true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3((minX + maxX) / 2f, spawnY, (minZ + maxZ) / 2f);
        Vector3 size = new Vector3(Mathf.Abs(maxX - minX), 0.1f, Mathf.Abs(maxZ - minZ));
        Gizmos.DrawWireCube(center, size);
    }
}