using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsController : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    public void SetVolume(string parameterName, float normalizedVolume)
    {
        float dbVolume = Mathf.Log10(Mathf.Clamp(normalizedVolume, 0.0001f, 1f)) * 20f;
        _audioMixer.SetFloat(parameterName, dbVolume);

        PlayerPrefs.SetFloat(parameterName, normalizedVolume);
    }

    public float GetSavedVolume(string parameterName, float defaultValue = 1f)
    {
        return PlayerPrefs.GetFloat(parameterName, defaultValue);
    }

    public void ApplyAllSavedAudio()
    {
        SetVolume(SettingsKeys.AudioMaster, GetSavedVolume(SettingsKeys.AudioMaster));
        SetVolume(SettingsKeys.AudioMusic, GetSavedVolume(SettingsKeys.AudioMusic));
        SetVolume(SettingsKeys.AudioEffects, GetSavedVolume(SettingsKeys.AudioEffects));
        SetVolume(SettingsKeys.AudioUI, GetSavedVolume(SettingsKeys.AudioUI));
    }
}
