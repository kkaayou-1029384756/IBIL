using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private GameObject _enemy01;
    [SerializeField] private GameObject _enemy02;

    private CanvasGroup canvasGroup;
    private bool isFading = false;

    private void Awake()
    {
        _enemy01.SetActive(false);
        _enemy02.SetActive(false);
    }

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isFading) // ¿ÞÂÊ Å¬¸¯
        {
            StartCoroutine(FadeAndDisable());
        }
    }

    IEnumerator FadeAndDisable()
    {
        isFading = true;
        float t = 0f;
        float startAlpha = canvasGroup.alpha;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, t / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        _enemy01.SetActive(true);
        _enemy02.SetActive(true);
        gameObject.SetActive(false);
    }
}
