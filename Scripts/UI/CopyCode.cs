using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CopyCode : MonoBehaviour
{
    [SerializeField] private TMP_Text _targetText; 
    [SerializeField] private float _feedbackDuration = 1.5f;
    [SerializeField] private Color _feedBackColor;

    private Color _defaultColor;
    private Button _button;
    private Image _image;
    private string _originalText;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(CopyText);
        _defaultColor = _image.color;
    }

    public void CopyText()
    {
        if (_targetText == null) 
            return;

        _button.enabled = false;

        GUIUtility.systemCopyBuffer = _targetText.text;
        _originalText = _targetText.text;

        ShowFeedback();
    }

    private void ShowFeedback()
    {
        if (_targetText == null) 
            return;

        _image.color = _feedBackColor;
        _targetText.text = "Copied!";
        Invoke(nameof(RestoreText), _feedbackDuration);
    }

    private void RestoreText()
    {
        _targetText.text = _originalText;
        _button.enabled = true;
        _image.color = _defaultColor;
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(CopyText);
    }
}