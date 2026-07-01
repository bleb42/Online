using System.Linq;
using UnityEngine;

public class ButtonReader : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private ButtonBase[] _buttons;
    [SerializeField] private bool _needAllButtons;

    [Header("Events")]
    [SerializeField] private EventsInvoker _eventsOnPressed;
    [SerializeField] private EventsInvoker _eventsOnReleased;

    private bool _currentState;
    private Coroutine _routine;

    private void OnEnable()
    {
        foreach (var button in _buttons)
            button.OnStateChanged += Recalculate;

        Recalculate(); 
    }

    private void OnDisable()
    {
        foreach (var button in _buttons)
            button.OnStateChanged -= Recalculate;
    }

    private void Recalculate()
    {
        bool result = _needAllButtons
            ? _buttons.All(b => b.IsActive)
            : _buttons.Any(b => b.IsActive);

        if (_currentState == result)
            return;

        _currentState = result;

        if (_currentState)
            _eventsOnPressed.InvokeEvents();
        else
            _eventsOnReleased.InvokeEvents();
    }
}