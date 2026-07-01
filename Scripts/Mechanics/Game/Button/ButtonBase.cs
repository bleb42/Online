using System;
using UnityEngine;

public abstract class ButtonBase : MonoBehaviour, IButton
{
    public bool IsActive { get; private set; }
    public event Action OnStateChanged;

    protected void SetState(bool active)
    {
        if (IsActive == active)
            return;

        IsActive = active;
        OnStateChanged?.Invoke();
    }
}