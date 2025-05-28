using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] GameObject enemySpawner;
    [SerializeField] GameObject waveTextUI;
    public int _wave = 1;
    public int _realTime;
    private float timer;


    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            timer = 0;
            _realTime++;
        }

        if (_realTime >= 20)
        {
            _realTime = 0;
            timer = 0;
            _wave++;
            enemySpawner.GetComponent<EnemySpawn>().stop = 1;
        }
    }
}
