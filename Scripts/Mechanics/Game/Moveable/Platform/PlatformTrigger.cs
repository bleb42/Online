using Synty.AnimationBaseLocomotion.Samples;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public Vector3 Velocity { get; private set; }

    private Vector3 _lastPos;

    private void Start()
    {
        _lastPos = transform.position;

        Collider col = GetComponent<Collider>();
        col.isTrigger = true; 
    }

    private void Update()
    {
        Velocity = (transform.position - _lastPos) / Time.deltaTime;
        _lastPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SamplePlayerAnimationController player))
        {
            player.SetCurrentPlatform(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out SamplePlayerAnimationController player))
        {
            player.SetCurrentPlatform(null);
        }
    }
}