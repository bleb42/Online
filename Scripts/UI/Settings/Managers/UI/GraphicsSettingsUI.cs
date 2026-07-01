using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettingsUI : MonoBehaviour
{
    [SerializeField] private GraphicsSettingsController _graphicsController;

    [Header("UI Elements")]
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private Slider _fpsSlider;
    [SerializeField] private TMP_Text _fpsValueText;
    [SerializeField] private TMP_Dropdown _aaDropdown;
    [SerializeField] private Toggle _vsyncToggle;
    [SerializeField] private Toggle _shadowsToggle;
    [SerializeField] private Toggle _postProcessingToggle;
    [SerializeField] private Toggle _fullscreenToggle;

    [Header("Config")]
    [SerializeField] private int _fpsStep = 15;
    [SerializeField] private List<Vector2Int> _availableResolutions = new List<Vector2Int>
    {
        new Vector2Int(2560, 1440),
        new Vector2Int(1920, 1080),
        new Vector2Int(1600, 900),
        new Vector2Int(1280, 720)
    };

    [SerializeField] private List<string> _aaOptions = new List<string> { "Off", "FXAA", "SMAA" };

    private void Start()
    {
        InitializeUI();
        SubscribeToEvents();
    }

    private void InitializeUI()
    {
        SetupResolutionsDropdown();
        SetupAADropdown();

        _fullscreenToggle.isOn = _graphicsController.GetSavedFullscreen();
        _vsyncToggle.isOn = _graphicsController.GetSavedVsync();
        _shadowsToggle.isOn = _graphicsController.GetSavedShadows();
        _postProcessingToggle.isOn = _graphicsController.GetSavedPostProcessing();

        int savedFps = _graphicsController.GetSavedFps();
        _fpsSlider.value = savedFps;
        UpdateFpsText(savedFps);

        _graphicsController.SetVsync(_vsyncToggle.isOn);
        _graphicsController.SetShadows(_shadowsToggle.isOn);
        _graphicsController.SetPostProcessing(_postProcessingToggle.isOn);
        _graphicsController.SetAntiAliasing(_graphicsController.GetSavedAA());
        _graphicsController.SetFps(savedFps);
    }

    private void SubscribeToEvents()
    {
        _resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        _fullscreenToggle.onValueChanged.AddListener(isOn =>
            _graphicsController.ApplyResolution(_availableResolutions[_resolutionDropdown.value].x, _availableResolutions[_resolutionDropdown.value].y, isOn));

        _fpsSlider.onValueChanged.AddListener(OnFpsSliderChanged);
        _vsyncToggle.onValueChanged.AddListener(_graphicsController.SetVsync);
        _shadowsToggle.onValueChanged.AddListener(_graphicsController.SetShadows);

        _postProcessingToggle.onValueChanged.AddListener(isOn =>
        {
            _graphicsController.SetPostProcessing(isOn);
            _graphicsController.SetAntiAliasing(_aaDropdown.value);
        });

        _aaDropdown.onValueChanged.AddListener(_graphicsController.SetAntiAliasing);
    }

    private void SetupResolutionsDropdown()
    {
        _resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int savedWidth = _graphicsController.GetSavedWidth(_availableResolutions[0].x);
        int savedHeight = _graphicsController.GetSavedHeight(_availableResolutions[0].y);
        int currentIndex = 0;

        for (int i = 0; i < _availableResolutions.Count; i++)
        {
            options.Add($"{_availableResolutions[i].x}x{_availableResolutions[i].y}");

            if (_availableResolutions[i].x == savedWidth && _availableResolutions[i].y == savedHeight)
            {
                currentIndex = i;
            }
        }

        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentIndex;
        _resolutionDropdown.RefreshShownValue();
    }

    private void SetupAADropdown()
    {
        _aaDropdown.ClearOptions();
        _aaDropdown.AddOptions(_aaOptions);
        _aaDropdown.value = _graphicsController.GetSavedAA();
    }

    private void OnResolutionChanged(int index)
    {
        var res = _availableResolutions[index];
        _graphicsController.ApplyResolution(res.x, res.y, _fullscreenToggle.isOn);
    }

    private void OnFpsSliderChanged(float rawValue)
    {
        int steppedFps = Mathf.RoundToInt(rawValue / _fpsStep) * _fpsStep;
        _fpsSlider.SetValueWithoutNotify(steppedFps);
        UpdateFpsText(steppedFps);
        _graphicsController.SetFps(steppedFps);
    }

    private void UpdateFpsText(int fps)
    {
        _fpsValueText.text = fps.ToString();
    }
}
