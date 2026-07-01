using UnityEngine;

[RequireComponent(typeof(Light))]
public class CharacterLight : MonoBehaviour
{
    [SerializeField] private AudioClip _enableClip;

    private Light _light;

    private void Awake()
    {
        _light = GetComponent<Light>();
        _light.enabled = false;
    }

    public void ShowCharacter()
    {
        _light.enabled = true;

        SoundManager.Instance.PlaySound(
            _enableClip,
            delay: 0f,
            pitchAdded: 0f,
            randomPitch: false,
            spatialBlend: 0f
        );
    }

    public void DesableLight()
    {
        _light.enabled = false;
    }
}