using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MenuIntro : MonoBehaviour
{
    [SerializeField] private float _delay = 1f;
    [SerializeField] private float _introDuration = 1f;
    [SerializeField] private AudioClip _clip;

    private AudioSource _audioSource;
    private Coroutine _volumeCoroutine;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0.0f;
        _audioSource.loop = true;
        _audioSource.clip = _clip;
    }

    private void Start()
    {
        _audioSource.Play();
        IntroSound();
    }

    private void IntroSound()
    {
        _volumeCoroutine = StartCoroutine(IncreaseVolumeRoutine());
    }

    private IEnumerator IncreaseVolumeRoutine()
    {
        float elapsedTime = 0f;
        float startVolume = _audioSource.volume;

        yield return new WaitForSeconds(_delay);

        while (elapsedTime < _introDuration)
        {
            elapsedTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(startVolume, 1f, elapsedTime / _introDuration);

            yield return null;
        }

        _audioSource.volume = 1f;
        _volumeCoroutine = null;
    }
}
