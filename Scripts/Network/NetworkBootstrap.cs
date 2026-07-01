using Synty.AnimationBaseLocomotion.Samples.InputSystem;
using UnityEngine;

public class NetworkBootstrap : MonoBehaviour
{
    public static NetworkBootstrap Instance;

    [Header("Dependencies")]
    [SerializeField] private PlayerMovementToggle _playerMovementToggle;

    [Header("Settings Controllers")]
    [SerializeField] private GraphicsSettingsController _graphicsController;
    [SerializeField] private AudioSettingsController _audioController;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
        { 
            Destroy(gameObject); 
            return; 
        }
    }

    public void Init(InputReader inputReader)
    {
        _playerMovementToggle.Init(inputReader);

        ApplyGameSettings();
    }

    private void ApplyGameSettings()
    {
        _audioController.ApplyAllSavedAudio();

        _graphicsController.SetVsync(_graphicsController.GetSavedVsync());
        _graphicsController.SetShadows(_graphicsController.GetSavedShadows());
        _graphicsController.SetPostProcessing(_graphicsController.GetSavedPostProcessing());
        _graphicsController.SetAntiAliasing(_graphicsController.GetSavedAA());
        _graphicsController.SetFps(_graphicsController.GetSavedFps());

        int w = _graphicsController.GetSavedWidth(Screen.currentResolution.width);
        int h = _graphicsController.GetSavedHeight(Screen.currentResolution.height);
        bool fs = _graphicsController.GetSavedFullscreen();
        _graphicsController.ApplyResolution(w, h, fs);

        Debug.Log("Game Settings Applied Successfully.");
    }
}