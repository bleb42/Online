using UnityEngine;
using Unity.Netcode;
using System.Collections;

[RequireComponent(typeof(MovingObject))]
public class MovingPlatform : NetworkBehaviour
{
    [SerializeField] private float _delayBeforeOpen = 0f;
    [SerializeField] private float _delayBeforeClose = 0f;

    private MovingObject _moveable;
    private Coroutine _delayRoutine;

    private void Awake()
    {
        _moveable = GetComponent<MovingObject>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;

        _moveable.OnOpen += HandleOpen;
        _moveable.OnClose += HandleClose;

        _moveable.Open();
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer)
            return;

        _moveable.OnOpen -= HandleOpen;
        _moveable.OnClose -= HandleClose;

        if (_delayRoutine != null)
            StopCoroutine(_delayRoutine);
    }

    private void HandleOpen()
    {
        StartDelayedAction(_delayBeforeClose, _moveable.Close);
    }

    private void HandleClose()
    {
        StartDelayedAction(_delayBeforeOpen, _moveable.Open);
    }

    private void StartDelayedAction(float delay, System.Action action)
    {
        if (_delayRoutine != null)
            StopCoroutine(_delayRoutine);

        if (delay > 0f)
            _delayRoutine = StartCoroutine(DelayRoutine(delay, action));
        else
            action.Invoke();
    }

    private IEnumerator DelayRoutine(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);

        action.Invoke();
    }
}