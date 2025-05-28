using System.Collections;
using UnityEngine;

public class ItemSelect : MonoBehaviour
{
    [SerializeField] private GameObject _uiObject;
    [SerializeField] private float _fadeDuration = 0.5f;     // 알파 변경 속도를 조금 빠르게
    [SerializeField] private float _displayDuration = 10f;   // 보여지는 시간 (게임 시간 기준)

    private void Start()
    {
        ShowUI();
    }
    public void ShowUI()
    {
        StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut()
    {
        if (_uiObject == null)
            yield break;

        _uiObject.SetActive(true);

        CanvasGroup canvasGroup = _uiObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = _uiObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        Time.timeScale = 0f;
        float t = 0f;
        while (t < _fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Clamp01(t / _fadeDuration);
            yield return null;
        }
        float elapsed = 0f;
        while (elapsed < _displayDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        t = 0f;
        while (t < _fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1 - (t / _fadeDuration));
            yield return null;
        }

        canvasGroup.alpha = 0f;
        _uiObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
