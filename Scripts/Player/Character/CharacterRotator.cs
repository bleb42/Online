using UnityEngine;

public class CharacterRotator : MonoBehaviour
{
    private const string MouseInput = "Mouse X";

    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _inertiaDamping = 5f; 

    private float _currentYRotation;
    private float _currentVelocity; 

    private void OnEnable()
    {
        _currentYRotation = transform.eulerAngles.y;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis(MouseInput);

            _currentVelocity = -mouseX * _rotationSpeed;
        }
        else
        {
            _currentVelocity = Mathf.Lerp(_currentVelocity, 0f, Time.deltaTime * _inertiaDamping);
        }

        _currentYRotation += _currentVelocity;
        transform.rotation = Quaternion.Euler(0f, _currentYRotation, 0f);
    }

    public void ResetRotation()
    {
        _currentYRotation = 0;
        _currentVelocity = 0;
        transform.rotation = Quaternion.Euler(0f, _currentYRotation, 0f);
    }
}