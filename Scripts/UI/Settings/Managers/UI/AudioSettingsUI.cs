using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private AudioSettingsController _audioController;

    [Header("Sliders")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _effectsSlider;
    [SerializeField] private Slider _uiSlider;

    [Header("Value Texts")]
    [SerializeField] private TMP_Text _masterValueText;
    [SerializeField] private TMP_Text _musicValueText;
    [SerializeField] private TMP_Text _effectsValueText;
    [SerializeField] private TMP_Text _uiValueText;

    private void Start()
    {
        InitializeUI();
        SubscribeToEvents();
    }

    private void InitializeUI()
    {
        _audioController.ApplyAllSavedAudio();

        SetupSlider(_masterSlider, _masterValueText, SettingsKeys.AudioMaster);
        SetupSlider(_musicSlider, _musicValueText, SettingsKeys.AudioMusic);
        SetupSlider(_effectsSlider, _effectsValueText, SettingsKeys.AudioEffects);
        SetupSlider(_uiSlider, _uiValueText, SettingsKeys.AudioUI);
    }

    private void SubscribeToEvents()
    {
        _masterSlider.onValueChanged.AddListener(val => OnSliderChanged(SettingsKeys.AudioMaster, val, _masterValueText));
        _musicSlider.onValueChanged.AddListener(val => OnSliderChanged(SettingsKeys.AudioMusic, val, _musicValueText));
        _effectsSlider.onValueChanged.AddListener(val => OnSliderChanged(SettingsKeys.AudioEffects, val, _effectsValueText));
        _uiSlider.onValueChanged.AddListener(val => OnSliderChanged(SettingsKeys.AudioUI, val, _uiValueText));
    }

    private void SetupSlider(Slider slider, TMP_Text textRef, string parameterKey)
    {
        float savedValue = _audioController.GetSavedVolume(parameterKey);
        slider.value = savedValue;
        UpdatePercentageText(textRef, savedValue);
    }

    private void OnSliderChanged(string parameterKey, float value, TMP_Text textRef)
    {
        _audioController.SetVolume(parameterKey, value);
        UpdatePercentageText(textRef, value);
    }

    private void UpdatePercentageText(TMP_Text text, float value)
    {
        text.text = Mathf.RoundToInt(value * 100f).ToString();
    }
}