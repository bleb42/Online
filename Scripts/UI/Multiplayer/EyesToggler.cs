using Unity.Netcode;
using UnityEngine;

public class EyesToggler : NetworkBehaviour
{
    [SerializeField] private GameObject[] _playersEyes;

    private void Awake() => DisableAll();

    public override void OnNetworkSpawn()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientChanged;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientChanged;
        }
        UpdateEyesImmediately();
    }

    private void OnClientChanged(ulong clientId)
    {
        Invoke(nameof(UpdateEyesImmediately), 0.1f);
    }

    public void UpdateEyesImmediately()
    {
        if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsListening)
        {
            DisableAll();
            return;
        }

        DisableAll();

        int activeEyes = 0;

        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            if (activeEyes >= _playersEyes.Length) break;

            if (client.Value.PlayerObject != null &&
                client.Value.PlayerObject.IsSpawned)
            {
                _playersEyes[activeEyes].SetActive(true);
                activeEyes++;
            }
        }
    }

    private void DisableAll()
    {
        foreach (var eye in _playersEyes)
        {
            if (eye != null)
                eye.SetActive(false);
        }
    }

    public override void OnNetworkDespawn()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientChanged;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientChanged;
        }
        DisableAll();
    }

    public void HandlePlayerLeft()
    {
        UpdateEyesImmediately();
    }
}