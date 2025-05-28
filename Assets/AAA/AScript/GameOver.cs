using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] Image gameOverUI;

    public IEnumerator Fade()
    {
        float alpha = 0;
        while (alpha <= 1)
        {
            alpha += Time.deltaTime;
            Color(alpha);
            yield return null;
        }
    }

    private void Color(float alpha)
    {
        Color color = gameOverUI.color;
        color.a = Mathf.Clamp01(alpha);
        gameOverUI.color = color;
    }
}
