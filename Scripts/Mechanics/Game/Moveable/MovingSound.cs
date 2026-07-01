using UnityEngine;

[RequireComponent(typeof(MovingObject))]
public class MovingSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource; 
    [SerializeField] private AudioClip _openSound;
    [SerializeField] private AudioClip _closeSound;

    private MovingObject _movingObject;

    private void Awake()
    {
        _movingObject = GetComponent<MovingObject>();
    }

    private void OnEnable()
    {
        if(_openSound != null)
            _movingObject.OnOpenStart += PlayOpenSound;
        
        if(_closeSound != null)
            _movingObject.OnCloseStart += PlayCloseSound;
    }

    private void OnDisable()
    {
        if (_openSound != null)
            _movingObject.OnOpenStart -= PlayOpenSound;

        if (_closeSound != null)
            _movingObject.OnCloseStart -= PlayCloseSound;
    }

    private void PlayOpenSound() => _audioSource.PlayOneShot(_openSound);
    private void PlayCloseSound() => _audioSource.PlayOneShot(_closeSound);
}