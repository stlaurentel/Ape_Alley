using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject cactusPrefab;
    public float spawnRate = 2f;
    public float fixedYSpawn = -1f;

    private float nextSpawnTime;

    public GameObject[] cactusPrefabs;

    void Update()
    {
        if (Time.time > nextSpawnTime) {
            nextSpawnTime = Time.time + spawnRate;
            SpawnCactus();
        }
    }

    void SpawnCactus() {
        Vector2 spawnPos = new Vector2(10f, fixedYSpawn);
        int randomIndex = Random.Range(0, cactusPrefabs.Length);
        Instantiate(cactusPrefabs[randomIndex], spawnPos, Quaternion.identity);
    }
}
