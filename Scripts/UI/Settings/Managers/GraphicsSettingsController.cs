using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GraphicsSettingsController : MonoBehaviour
{
    [Header("Default Settings")]
    [SerializeField] private int _defaultFps = 60;
    [SerializeField] private bool _defaultFullscreen = true;
    [SerializeField] private bool _defaultShadows = true;
    [SerializeField] private bool _defaultPost = true;
    [SerializeField] private bool _defaultVsync = false;
    [SerializeField] private int _defaultAA = 1;

    private UniversalAdditionalCameraData _cameraData;

    private void Awake()
    {
        if (Camera.main != null)
        {
            _cameraData = Camera.main.GetComponent<UniversalAdditionalCameraData>();
        }
    }

    public void ApplyResolution(int width, int height, bool isFullscreen)
    {
        Screen.SetResolution(width, height, isFullscreen);
        PlayerPrefs.SetInt(SettingsKeys.ResolutionWidth, width);
        PlayerPrefs.SetInt(SettingsKeys.ResolutionHeight, height);
        PlayerPrefs.SetInt(SettingsKeys.Fullscreen, isFullscreen ? 1 : 0);
    }

    public void SetFps(int targetFps)
    {
        Application.targetFrameRate = targetFps;
        PlayerPrefs.SetInt(SettingsKeys.FPS, targetFps);
    }

    public void SetVsync(bool isEnabled)
    {
        QualitySettings.vSyncCount = isEnabled ? 1 : 0;
        PlayerPrefs.SetInt(SettingsKeys.VSync, isEnabled ? 1 : 0);
    }

    public void SetShadows(bool isEnabled)
    {
        if (_cameraData != null) 
            _cameraData.renderShadows = isEnabled;

        PlayerPrefs.SetInt(SettingsKeys.Shadows, isEnabled ? 1 : 0);
    }

    public void SetPostProcessing(bool isEnabled)
    {
        if (_cameraData != null) _cameraData.renderPostProcessing = isEnabled;
        PlayerPrefs.SetInt(SettingsKeys.PostProcessing, isEnabled ? 1 : 0);
    }

    public void SetAntiAliasing(int aaIndex)
    {
        if (_cameraData == null) 
            return;

        AntialiasingMode mode = AntialiasingMode.None;

        switch (aaIndex)
        {
            case 1:
                mode = AntialiasingMode.FastApproximateAntialiasing;
                break;
            case 2:
                mode = _cameraData.renderPostProcessing
                    ? AntialiasingMode.SubpixelMorphologicalAntiAliasing
                    : AntialiasingMode.FastApproximateAntialiasing;
                break;
        }

        _cameraData.antialiasing = mode;
        PlayerPrefs.SetInt(SettingsKeys.AntiAliasing, aaIndex);
    }

    public int GetSavedWidth(int defaultWidth) => PlayerPrefs.GetInt(SettingsKeys.ResolutionWidth, defaultWidth);
    public int GetSavedHeight(int defaultHeight) => PlayerPrefs.GetInt(SettingsKeys.ResolutionHeight, defaultHeight);
    public bool GetSavedFullscreen() => PlayerPrefs.GetInt(SettingsKeys.Fullscreen, _defaultFullscreen ? 1 : 0) == 1;
    public int GetSavedFps() => PlayerPrefs.GetInt(SettingsKeys.FPS, _defaultFps);
    public bool GetSavedVsync() => PlayerPrefs.GetInt(SettingsKeys.VSync, _defaultVsync ? 1 : 0) == 1;
    public bool GetSavedShadows() => PlayerPrefs.GetInt(SettingsKeys.Shadows, _defaultShadows ? 1 : 0) == 1;
    public bool GetSavedPostProcessing() => PlayerPrefs.GetInt(SettingsKeys.PostProcessing, _defaultPost ? 1 : 0) == 1;
    public int GetSavedAA() => PlayerPrefs.GetInt(SettingsKeys.AntiAliasing, _defaultAA);
}