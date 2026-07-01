using System;
using System.Collections;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [Header("Position")]
    [SerializeField] private Vector3 _closedPosition;
    [SerializeField] private Vector3 _openedPosition;
    [SerializeField] private bool _isOpenedOnStart = false;

    [Header("Animation")]
    [SerializeField] private float _duration;
    [SerializeField] private AnimationCurve _curve;

    public event Action OnOpenStart;
    public event Action OnCloseStart;

    public event Action OnOpen;
    public event Action OnClose;

    private Coroutine _moving;
    private bool _isOpening;

    private void Start()
    {
        transform.localPosition = _isOpenedOnStart ? _openedPosition : _closedPosition;
    }

    public void Open()
    {
        _isOpening = true;
        OnOpenStart?.Invoke();
        Move(_openedPosition);
    }

    public void Close()
    {
        _isOpening = false;
        OnCloseStart?.Invoke();
        Move(_closedPosition);
    }

    private void Move(Vector3 target)
    {
        if (_moving != null)
            StopCoroutine(_moving);

        _moving = StartCoroutine(MoveRoutine(target));
    }

    private IEnumerator MoveRoutine(Vector3 target)
    {
        Vector3 start = transform.localPosition;
        float t = 0f;

        while (t < _duration)
        {
            t += Time.deltaTime;
            float k = _curve.Evaluate(t / _duration);
            transform.localPosition = Vector3.Lerp(start, target, k);

            yield return null;
        }

        transform.localPosition = target;
        _moving = null;

        if (_isOpening)
            OnOpen?.Invoke();
        else
            OnClose?.Invoke();
    }
}