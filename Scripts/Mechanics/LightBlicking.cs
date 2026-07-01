using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightBlicking : MonoBehaviour
{
    [Header("Настройки мерцания")]
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 1.5f;
    [SerializeField] private float minInterval = 0.1f;
    [SerializeField] private float maxInterval = 0.5f;
    [SerializeField] private bool smoothTransition = true;
    [SerializeField] private float transitionSpeed = 2f;

    private Light _light;
    private Coroutine _blinkingCoroutine;
    private float _targetIntensity;

    private void Awake()
    {
        _light = GetComponent<Light>();
    }

    private void Start()
    {
        StartBlinking();
    }

    public void StartBlinking()
    {
        if (_blinkingCoroutine != null)
            StopCoroutine(_blinkingCoroutine);

        _blinkingCoroutine = StartCoroutine(BlinkingRoutine());
    }

    public void StopBlinking()
    {
        if (_blinkingCoroutine != null)
            StopCoroutine(_blinkingCoroutine);

        _light.intensity = maxIntensity;
    }

    private IEnumerator BlinkingRoutine()
    {
        while (true)
        {
            _targetIntensity = Random.Range(minIntensity, maxIntensity);

            float interval = Random.Range(minInterval, maxInterval);

            if (smoothTransition)
            {
                float elapsedTime = 0f;
                float startIntensity = _light.intensity;

                while (elapsedTime < interval)
                {
                    _light.intensity = Mathf.Lerp(startIntensity, _targetIntensity, elapsedTime * transitionSpeed);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                _light.intensity = _targetIntensity;

                yield return new WaitForSeconds(interval);
            }
        }
    }

    private void OnDisable()
    {
        StopBlinking();
    }
}
