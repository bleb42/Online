using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private AudioSource _source;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
        }
        else
        {
            Destroy(gameObject);
        }

        _source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip, float delay, float pitchAdded, bool randomPitch, float spatialBlend)
    {
        StartCoroutine(Play(clip, delay, pitchAdded, randomPitch, spatialBlend));
    }

    private IEnumerator Play(AudioClip clip, float delay, float pitch, bool randomPitch, float spatialBlend)
    {
        if (clip == null) 
            yield break;
        
        yield return new WaitForSeconds(delay);
        
        _source.spatialBlend = spatialBlend;
        float pitchAdded = randomPitch ? Random.Range(-pitch, pitch) : pitch;
        _source.pitch = 1 + pitchAdded;
        _source.PlayOneShot(clip);
        
        yield return null;
    }
}