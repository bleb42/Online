using UnityEngine;

public class Secret : MonoBehaviour
{
    [SerializeField] private GameObject _Button;
    [SerializeField] private KeyCode _firstCode;
    [SerializeField] private KeyCode _secondCode;

    private void Awake()
    {
        _Button.SetActive(false);
    }

    private void Update()
    {
        if (isActiveAndEnabled && Input.GetKey(_firstCode) && Input.GetKey(_secondCode))
            _Button.SetActive(true);
    }
}