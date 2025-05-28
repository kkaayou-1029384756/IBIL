using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class WaveText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI realWaveText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI waveText;

    [SerializeField] TextMeshProUGUI spriteRenderer;
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

    public IEnumerator FadeStart(bool fade)
    {
        if (fade)
        {
            waveText.gameObject.SetActive(true);
            float alpha = 0;
            while (alpha <= 1)
            {
                SetAlpha(alpha);
                alpha += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(FadeStart(false));
        }
        else
        {
            waveText.gameObject.SetActive(false);
            float alpha = 1;
            while (alpha >= 0)
            {
                SetAlpha(alpha);
                alpha -= Time.deltaTime;
                yield return null;
            }
        }
    }

    public void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = Mathf.Clamp01(alpha);
        spriteRenderer.color = color;
    }
}
