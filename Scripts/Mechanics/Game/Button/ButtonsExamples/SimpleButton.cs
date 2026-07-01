using System;
using UnityEngine;

public class SimpleButton : ButtonBase
{
    private int _playersInside;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out CharacterController _))
            return;

        _playersInside++;
        SetState(_playersInside > 0);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out CharacterController _))
            return;

        _playersInside = Mathf.Max(0, _playersInside - 1);
        SetState(_playersInside > 0);
    }
} 