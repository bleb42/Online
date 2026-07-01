using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SelectButton : MonoBehaviour
{
    [SerializeField] private GameObject _targetPannel;
    [SerializeField] private GameObject _selectImage;

    private Button _button;
    private UIButton _uiButton;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _uiButton = GetComponent<UIButton>();
    }

    public void Select()
    {
        _button.enabled = false;
        _uiButton.enabled = false;
        _selectImage.SetActive(true);
        _targetPannel.SetActive(true);
    }

    public void Deselect()
    {
        _button.enabled = true;
        _uiButton.enabled = true;
        _selectImage.SetActive(false);
        _targetPannel.SetActive(false);
    }
}