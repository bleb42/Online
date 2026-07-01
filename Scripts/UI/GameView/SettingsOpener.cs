using UnityEngine;

public class SettingsOpener : MonoBehaviour
{
    [SerializeField] private BlockableWindow _settingsWindow;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _settingsWindow.gameObject.activeSelf == false)
        {
            UIManager.Instance.Open(_settingsWindow);
        }
    }
}