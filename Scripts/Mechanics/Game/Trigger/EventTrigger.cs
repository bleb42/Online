using Synty.AnimationBaseLocomotion.Samples.InputSystem;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _eventsOnEnter;
    [SerializeField] private UnityEvent _eventsOnExit;
    [SerializeField] private bool _isActiveOnce = true;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out InputReader player))
        {
            InvokeEvents(_eventsOnEnter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out InputReader player))
        {
            InvokeEvents(_eventsOnExit);
        }
    }

    private void InvokeEvents(UnityEvent events)
    {
        if (events == null || events.GetPersistentEventCount() == 0)
            return;

        events.Invoke();

        if (!_isActiveOnce)
            return;

        bool hasEnter = _eventsOnEnter != null && _eventsOnEnter.GetPersistentEventCount() > 0;
        bool hasExit = _eventsOnExit != null && _eventsOnExit.GetPersistentEventCount() > 0;

        if (!hasEnter && !hasExit)
            Destroy(gameObject);
    }
}