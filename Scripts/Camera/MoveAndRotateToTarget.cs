using System.Collections;
using UnityEngine;

public class MoveAndRotateToTarget : MonoBehaviour
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _endPosition;
    [SerializeField] private float _duration = 1.5f;
    [SerializeField] private CharacterLight _characterLight;

    private Coroutine moveRoutine;

    public void Open()
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveAndRotate(_endPosition, () =>
        {
            _characterLight?.ShowCharacter(); 
        }));
    }

    public void Close()
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        _characterLight?.DesableLight(); 

        moveRoutine = StartCoroutine(MoveAndRotate(_startPosition));
    }

    private IEnumerator MoveAndRotate(Transform target, System.Action onComplete = null)
    {
        if (target == null || _duration <= 0f)
            yield break;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 endPos = target.position;
        Quaternion endRot = target.rotation;

        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _duration);

            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            yield return null;
        }

        transform.position = endPos;
        transform.rotation = endRot;

        onComplete?.Invoke();
        moveRoutine = null;
    }
}