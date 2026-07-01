using UnityEngine.Events;

[System.Serializable]
public struct DelayedEvent
{
    public UnityEvent Event;
    public float DelayAfter;
}