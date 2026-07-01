using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class UIMover : UIMovable
{
    [SerializeField] private Vector2 _openPosition;
    [SerializeField] private Vector2 _closePosition;
    [SerializeField] private float _duration = 0.3f;
    [SerializeField] private float _delay = 0.25f;
    [SerializeField] private Ease _ease = Ease.OutCubic;

    private RectTransform _target;
    private Tween _currentTween;

    private void Awake()
    {
        _target = GetComponent<RectTransform>();
    }

    public override void Open()
    {
        StartMove(_closePosition, _openPosition);
    }

    public override void Close()
    {
        StartMove(_openPosition, _closePosition);
    }

    private void StartMove(Vector2 startPosition, Vector2 targetPosition)
    { 
        if (_currentTween != null && _currentTween.IsActive())
        {
            _currentTween.Kill();
        }

        _target.anchoredPosition = startPosition;

        _currentTween = _target.DOAnchorPos(targetPosition, _duration)
            .SetEase(_ease)
            .SetDelay(_delay)
            .OnKill(() => _currentTween = null);
    }
}

public abstract class UIMovable : MonoBehaviour
{
    public abstract void Open();
    public abstract void Close();
}