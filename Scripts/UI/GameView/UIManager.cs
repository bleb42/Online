using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private readonly Stack<IBlockableUI> _openWindows = new();

    private void Awake()
    {
        if (Instance != null) 
            Destroy(gameObject);
        else 
            Instance = this;
    }

    public void Open(IBlockableUI window)
    {
        window.Panel.SetActive(true);
        _openWindows.Push(window);

        if (window.BlocksGameplay)
            PlayerMovementToggle.Instance.SetControlActive(false);
    }

    public void Close(IBlockableUI window)
    {
        if (_openWindows.Contains(window))
        {
            window.Panel.SetActive(false);
            _openWindows.Pop();

            if (!_openWindows.Any(w => w.BlocksGameplay))
                PlayerMovementToggle.Instance.SetControlActive(true);
        }
    }
}