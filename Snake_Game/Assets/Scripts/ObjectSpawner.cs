using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject applePrefab;
    public GameObject bombPrefab; 
    public float minX = -10f, maxX = 10f; 
    public float minZ = -10f, maxZ = 10f; 
    public float spawnY = 0.5f; 
    public float bombSpawnInterval = 5f; 
    public int maxBombs = 2; 
    public float bombLifetime = 10f; 
    private GameObject currentApple; // Reference to the current apple
    private List<BombInfo> activeBombs = new List<BombInfo>();

    
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
    }

    void Update()
    {
        
        if (currentApple == null)
        {
            SpawnApple();
        }

        if (Time.time >= GetNextBombSpawnTime() && activeBombs.Count < maxBombs)
        {
            SpawnBomb();
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
        Vector3 spawnPos = new Vector3(
            Random.Range(minX, maxX),
            spawnY,
            Random.Range(minZ, maxZ)
        );
        currentApple = Instantiate(applePrefab, spawnPos, Quaternion.identity);
        //currentApple.tag = "Apple";
        }

    void SpawnBomb()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(minX, maxX),
            spawnY,
            Random.Range(minZ, maxZ)
        );
        GameObject bomb = Instantiate(bombPrefab, spawnPos, Quaternion.identity);
        //bomb.tag = "Bomb";
        //if (!bomb.GetComponent<Collider>()) bomb.AddComponent<SphereCollider>();
        activeBombs.Add(new BombInfo(bomb, Time.time));
    }

    float GetNextBombSpawnTime()
    {
        float earliestSpawnTime = float.MaxValue;
        foreach (var bombInfo in activeBombs)
        {
            if (bombInfo.spawnTime < earliestSpawnTime)
                earliestSpawnTime = bombInfo.spawnTime;
        }

        if (earliestSpawnTime == float.MaxValue)
            return Time.time + bombSpawnInterval;

        return earliestSpawnTime + bombSpawnInterval;
    }
}