using Synty.AnimationBaseLocomotion.Samples.InputSystem;
using UnityEngine;

public class PlayerMovementToggle : MonoBehaviour
{
    public static PlayerMovementToggle Instance { get; private set; }

    private InputReader _inputReader;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Init(InputReader inputReader)
    {
        _inputReader = inputReader;
    }

    public void SetControlActive(bool active)
    {
        if (_inputReader != null)
            _inputReader.enabled = active;

        Cursor.visible = !active;
        Cursor.lockState = active ? CursorLockMode.Locked : CursorLockMode.None;
    }
}