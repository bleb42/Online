using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EventsInvoker : MonoBehaviour
{
    [SerializeField] private DelayedEvent[] _events;

    private Coroutine _invokingEvents;

    public void InvokeEvents()
    {
        Debug.Log("InvokeEvents");

        if (_invokingEvents != null)
            StopCoroutine(_invokingEvents);

        _invokingEvents = StartCoroutine(InvokeSequence());
    }

    private IEnumerator InvokeSequence()
    {
        Debug.Log("InvokeSequence");

        foreach (var @event in _events)
        {
            @event.Event?.Invoke();
            Debug.Log(@event);

            if (@event.DelayAfter > 0f)
                yield return new WaitForSecondsRealtime(@event.DelayAfter);
        }
    }
}