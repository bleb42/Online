using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NicknameSelect : MonoBehaviour
{
    private const string NicknameKey = "PlayerNickname";
    private const string DefaultPlayerNickname = "Pidaras";

    [Header("UI References")]
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _selectNamePanel;
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private Button _saveButton;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.onClick.AddListener(OpenSelectNamePanel);
        _saveButton.onClick.AddListener(SaveNickname);

        _mainPanel.SetActive(true);
        _selectNamePanel.SetActive(false);
    }

    private void OpenSelectNamePanel()
    {
        _mainPanel.SetActive(false);
        _selectNamePanel.SetActive(true);

        _nameInputField.text = PlayerPrefs.GetString(NicknameKey, "");
    }

    private void SaveNickname()
    {
        string newName = _nameInputField.text.Trim();

        if (!string.IsNullOrEmpty(newName))
        {
            PlayerPrefs.SetString(NicknameKey, newName);
            PlayerPrefs.Save();

            _mainPanel.SetActive(true);
            _selectNamePanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Nickname cannot be empty!");
        }
    }

    private void OnDestroy()
    {
        _saveButton.onClick.RemoveListener(SaveNickname);
    }
}