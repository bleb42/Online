using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class OffScreen : MonoBehaviour
{
    [SerializeField] private GameObject _mainPannel;
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _delay = 0f;

    public event Action FadedOut;
    public event Action FadedIn;

    private Image _image;
    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        _image = GetComponent<Image>();

        Color startColor = _image.color;
        startColor.a = 1f;
        _image.color = startColor;
    }

    private void Start()
    {
        StartFading();
    }

    public void FadeOut()
    {
        StartFade(1f, 0f);
    }

    public void FadeIn()
    {
        StartFade(0f, 1f);
    }

    public void StartFading()
    {
        StartCoroutine(FadingOut());
    }

    public void ReturToMain(GameObject pannelToDisable)
    {
        StartCoroutine(RetutningToMain(pannelToDisable));
    }

    private void StartFade(float from, float to)
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(Fading(from, to));
    }

    private IEnumerator Fading(float startAlpha, float endAlpha)
    {
        float time = 0f;
        Color c = _image.color;

        while (Mathf.Abs(c.a - endAlpha) > 0.01f)
        {
            time += Time.deltaTime * _speed;
            float a = Mathf.Lerp(startAlpha, endAlpha, time);
            c.a = a;
            _image.color = c;
            yield return null;
        }

        c.a = endAlpha;
        _image.color = c;

        FadedOut?.Invoke();
    }

    private IEnumerator FadingOut()
    {
        yield return new WaitForSeconds(_delay);

        yield return Fading(1f, 0f);

        FadedOut?.Invoke();
    }

    private IEnumerator RetutningToMain(GameObject pannelToDisable)
    {
        yield return Fading(0f, 1f);

        pannelToDisable.SetActive(false);
        FadedIn?.Invoke();
        _mainPannel.SetActive(true);

        StartFading();
    }
}