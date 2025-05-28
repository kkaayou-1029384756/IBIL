using System.Collections;
using TMPro;
using UnityEngine;

public class ItemSelect : MonoBehaviour
{
    public GameObject _uiObject;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private float _displayDuration = 10f;
    [SerializeField] private TextMeshProUGUI _speedLevelTxt;
    [SerializeField] private TextMeshProUGUI _chargeLevelTxt;
    [SerializeField] private TextMeshProUGUI _rangeLevelTxt;

    int speedLevel = 1;
    int chargeLevel = 1;
    int rangeLevel = 1;

    [SerializeField] private PlayerMain _playerMain;

    private Coroutine fadeCoroutine;
    private CanvasGroup canvasGroup;

    public void ShowUI()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut()
    {
        if (_uiObject == null)
            yield break;

        _uiObject.SetActive(true);


        canvasGroup = _uiObject.GetComponent<CanvasGroup>();
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

        yield return StartCoroutine(FadeOutAndDisable());
    }

    private IEnumerator FadeOutAndDisable()
    {
        float t = 0f;
        while (t < _fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1 - (t / _fadeDuration));
            yield return null;
        }

        canvasGroup.alpha = 0f;
        _uiObject.SetActive(false);
        Time.timeScale = 1f;
        fadeCoroutine = null;
    }

    //  강제 종료 메서드
    public void ForceCloseUI()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        if (gameObject.activeInHierarchy)
            StartCoroutine(FadeOutAndDisable());
    }

    public void UpgradeSpeed()
    {
        speedLevel++;
        _speedLevelTxt.text = "속도: " + speedLevel;
        _playerMain._moveSpeed += 2;
        ForceCloseUI(); //  추가
    }

    public void UpgradeCharge()
    {
        chargeLevel++;
        _chargeLevelTxt.text = "충전: " + chargeLevel;
        _playerMain._chargeSpeed += 1;
        ForceCloseUI(); //  추가
    }

    public void UpgradeRange()
    {
        rangeLevel++;
        _rangeLevelTxt.text = "범위: " + rangeLevel;
        _playerMain._launchDistance += 2;
        ForceCloseUI(); //  추가
    }
}
