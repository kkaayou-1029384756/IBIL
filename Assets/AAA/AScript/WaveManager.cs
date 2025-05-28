using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] GameObject enemySpawner;
    [SerializeField] WaveText waveText;
    [SerializeField] ItemSelect itemSelect;
    public int _wave = 1;
    public int _realTime;
    private float timer;
    private int waveTime = 20;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            timer = 0;
            _realTime++;
        }

        if (_realTime >= waveTime)
        {
            _realTime = 0;
            waveTime += 4;
            timer = 0;
            _wave++;
            StartCoroutine(waveText.FadeStart(true));
            itemSelect.ShowUI();
            Debug.Log("aa");
        }
    }
}
