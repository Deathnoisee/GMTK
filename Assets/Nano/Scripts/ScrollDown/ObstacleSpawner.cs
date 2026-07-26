using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacles")]
    public GameObject[] obstaclePrefabs; // drag all obstacle variants here


    public GameObject winner;

    [Header("Spawn Timing")]
    public float initialSpawnInterval = 2f;
    public float minSpawnInterval = 0.5f; // fastest spawn rate reached at max difficulty
    public float spawnRangeX = 8f;

    [Header("Difficulty Ramp")]
    public float difficultyRampDuration = 60f; // time (seconds) to go from easiest to hardest

    public SurfController surfController; // Reference to the SurfController script

    private float timer = 0f;
    private float elapsedSinceStart = 0f;
    private float currentSpawnInterval;


    public bool canSpawn = true;    

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
    }

    void Update()
    {
        if (!surfController.started) // Check if the game has started
        {
            return;
        }

        elapsedSinceStart += Time.deltaTime;
        timer += Time.deltaTime;

        // Recalculate how hard it should be right now (0 = just started, 1 = fully ramped up)
        float difficultyT = Mathf.Clamp01(elapsedSinceStart / difficultyRampDuration);
        currentSpawnInterval = Mathf.Lerp(initialSpawnInterval, minSpawnInterval, difficultyT);

        if (timer >= currentSpawnInterval)
        {
            if (canSpawn) {

                SpawnObstacle();
                timer = 0f;
            }
           
        }
    }
    public void SpawnWinner() {

        GameObject winnerButoon = Instantiate(winner, transform.position, Quaternion.identity);
        winnerButoon.transform.SetParent(transform);
    }
    private void SpawnObstacle()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("No obstacle prefabs assigned!");
            return;
        }

        GameObject prefabToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, 0f);

        GameObject obstacle = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        obstacle.transform.SetParent(transform);
    }
}