using System.Collections;
using UnityEngine;

public class ButtonVisual : MonoBehaviour
{
    [Header("Logic")]
    [SerializeField] private Transform _body;

    [Header("Animation")]
    [SerializeField] private float _pressDepth = 0.2f;
    [SerializeField] private float _duration = 0.15f;
    [SerializeField] private AnimationCurve _curve;

    private IButton _logic;
    private Vector3 _startPos;
    private Coroutine _moving;

    private void Awake()
    {
        _startPos = _body.localPosition;

        if (TryGetComponent<IButton>(out _logic) == false)
        {
            Debug.LogError($"No IButton Interface on Object! {gameObject}");

            return;
        }
    }

    private void OnEnable()
    {
        if (_logic != null)
            _logic.OnStateChanged += UpdateVisual;
    }

    private void OnDisable()
    {
        if (_logic != null)
            _logic.OnStateChanged -= UpdateVisual;
    }

    private void UpdateVisual()
    {
        Vector3 target = _logic.IsActive
            ? _startPos - Vector3.up * _pressDepth
            : _startPos;

        if (_moving != null)
            StopCoroutine(_moving);

        _moving = StartCoroutine(Animate(target));
    }

    private IEnumerator Animate(Vector3 target)
    {
        Vector3 start = _body.localPosition;
        float time = 0f;

        while (time < _duration)
        {
            time += Time.deltaTime;
            float t = time / _duration;
            float curveT = _curve.Evaluate(t);

            _body.localPosition = Vector3.Lerp(start, target, curveT);
            yield return null;
        }

        _body.localPosition = target;
        _moving = null;
    }
}