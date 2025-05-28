using UnityEngine;
using UnityEngine.InputSystem;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] GameObject normalEnemyPrefab;
    [SerializeField] GameObject waveManager;

    private float timer;
    public int stop = 1;
    private int enemyCount, enemyLimit = 5;

    public int nowStage;

    private Vector2 spawnPosition;

    float minTime = 4f, maxTime = 5;
    float minClamp = 0.5f, maxClamp = 5f;
    float decreaseRate = 0.5f;

    private void Update()
    {
        minTime = Mathf.Max(minClamp, minTime - waveManager.GetComponent<WaveManager>()._wave * decreaseRate);
        maxTime = Mathf.Max(maxClamp, maxTime - waveManager.GetComponent<WaveManager>()._wave * decreaseRate);

        timer += Time.deltaTime * stop;
        if (timer >= Random.Range(minTime + 1.5f, maxTime + 1.5f))
        {
            enemyCount++;
            if (enemyCount >= enemyLimit)
            {
                enemyLimit += 3;
                enemyCount = 0;
                stop = 0;
            }
            spawnPosition = RandomSpawnPosition();
            SpawnMob();
            timer = 0;
        }

        Vector2 RandomSpawnPosition()
        {
            return new Vector2(Random.Range(-11, 11), 5.5f);
        }
    }

    public void SpawnMob()
    {
        Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.identity);
    }
}
