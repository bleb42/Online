using UnityEngine;
using UnityEngine.UI;

public class CloseUIWindow : MonoBehaviour
{
    [SerializeField] private BlockableWindow _windowToClose;

    private void Awake()
    {
        if (TryGetComponent<Button>(out var button))
        {
            button.onClick.AddListener(() =>
            {
                if (_windowToClose != null)
                    UIManager.Instance.Close(_windowToClose);
            });
        }
    }
}