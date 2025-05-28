using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] GameObject normalEnemyPrefab;
    [SerializeField] GameObject waveManager;

    private float timer;
    private int enemyCount = 0, enemyLimit = 5;

    private Vector2 spawnPosition;

    float minTime = 2.5f, maxTime = 3.5f;
    float minClamp = 0.5f, maxClamp = 5f;
    float decreaseRate = 0.5f;

    private float currentSpawnDelay;

    private void Start()
    {
        spawnPosition = RandomSpawnPosition();
        SpawnMob();
        enemyCount++;
        SetNextSpawnDelay();
    }

    private void Update()
    {
        minTime = Mathf.Max(minClamp, 2f - waveManager.GetComponent<WaveManager>()._wave * decreaseRate);
        maxTime = Mathf.Max(maxClamp, 3f - waveManager.GetComponent<WaveManager>()._wave * decreaseRate);

        timer += Time.deltaTime;

        if (timer >= currentSpawnDelay)
        {
            if (enemyCount < enemyLimit)
            {
                spawnPosition = RandomSpawnPosition();
                SpawnMob();
                enemyCount++;
                SetNextSpawnDelay();
            }
            else
            {
                // 웨이브 끝났다고 가정하고 리셋 또는 증가
                enemyLimit += 3;
                enemyCount = 0;
                SetNextSpawnDelay();
            }

            timer = 0;
        }
    }

    private void SetNextSpawnDelay()
    {
        currentSpawnDelay = Random.Range(minTime + 1.5f, maxTime + 1.5f);
    }

    private Vector2 RandomSpawnPosition()
    {
        return new Vector2(Random.Range(-11f, 11f), 5.5f);
    }

    public void SpawnMob()
    {
        var mob = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.identity);
    }
}
