using UnityEngine;
using TMPro;

public class WaveText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI realWaveText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI waveText;


    [SerializeField] WaveManager waveManager;

    public bool isMinute = false;

    private float second;
    private float minute;

    private void Update()
    {
        waveText.text = $"Wave : {waveManager._wave}";
        second += Time.deltaTime;

        if (isMinute)
        {
            timerText.text = $"{minute}:{(int)second}";
            realWaveText.text = $"Wave : {waveManager._wave}";
        }
        else
        {
            timerText.text = $"{(int)second}";
            realWaveText.text = $"Wave : {waveManager._wave}";
        }
        if (second >= 60f)
        {
            isMinute = true;
            minute++;
            second -= 60f;
        }
    }
}
