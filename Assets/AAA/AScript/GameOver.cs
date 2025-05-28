using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] Image gameOverUI;

    public IEnumerator Fade()
    {
        float alpha = 1;
        while (alpha >= 0)
        {
            alpha -= Time.deltaTime;
            Color(alpha);
            yield return null;
        }
        gameOverUI.gameObject.SetActive(false);
    }

    private void Color(float alpha)
    {
        Color color = gameOverUI.color;
        color.a = Mathf.Clamp01(alpha);
        gameOverUI.color = color;
    }
}
