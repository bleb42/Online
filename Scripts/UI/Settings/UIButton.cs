using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

[RequireComponent(typeof(Image))]
public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    private Image _image;
    private Coroutine _fadeCoroutine;
    private ButtonSettings _settings;

    private void Start()
    {
        _image = GetComponent<Image>();
        _settings = ButtonSettings.Instance;

        if (_settings != null)
            _image.color = _settings.OriginalColor;
    }

    private void OnEnable()
    {
        if (_settings != null)
            StartFade(_settings.OriginalColor);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_settings.HoverSound)
            SoundManager.Instance.PlaySound(_settings.HoverSound, 0f, 0f, false, 0f);

        StartFade(_settings.HoverColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartFade(_settings.OriginalColor);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_settings != null && _settings.ClickSound)
            SoundManager.Instance.PlaySound(_settings.ClickSound, 0f, 0f, false, 0f);

        StartFade(_settings.ClickColor, () =>
        {
            StartFade(_settings.OriginalColor);
        });
    }

    private void StartFade(Color targetColor, Action onComplete = null)
    {
        if (isActiveAndEnabled == false)
            return;

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeToColor(targetColor, onComplete));
    }

    private IEnumerator FadeToColor(Color targetColor, Action onComplete = null)
    {
        Color startColor = _image.color;
        float time = 0f;
        float duration = _settings.FadeDuration;

        while (time < duration)
        {
            _image.color = Color.Lerp(startColor, targetColor, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        _image.color = targetColor;
        onComplete?.Invoke();
    }
}