using UnityEngine;
using UnityEngine.Events;

public class BootsTrap : MonoBehaviour
{
    [SerializeField] private UnityEvent _actions;

    private void Start()
    {
        _actions?.Invoke();
    }
}