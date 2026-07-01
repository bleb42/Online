using Unity.Netcode;
using UnityEngine;

public class ClientPlayerMove : NetworkBehaviour
{
    [SerializeField] private GameObject _playerCamera;
    [SerializeField] private Behaviour[] _componentsToDisable;

    private void Awake()
    {
        _playerCamera.gameObject.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        SetComponentsState(IsOwner);

        if (IsOwner)
        {
            _playerCamera.gameObject.SetActive(true);
        }
    }

    private void SetComponentsState(bool isEnabled)
    {
        foreach (var component in _componentsToDisable)
        {
            component.enabled = isEnabled;
        }
    }
}