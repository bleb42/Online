using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayersList : NetworkBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private Transform _contentParent;
    [SerializeField] private GameObject _playerEntryPrefab;
    [SerializeField] private Color _hostColor = Color.blue;
    [SerializeField] private Color _clientColor = Color.green;
    [SerializeField] private EyesToggler eyesToggler;

    private bool _isInitialized;

    public override void OnNetworkSpawn()
    {
        Initialize();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientChanged;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientChanged;
        }
    }

    private void Initialize()
    {
        if (NetworkManager.Singleton == null)
            return;

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientChanged;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientChanged;

        UpdatePlayersList();
        _isInitialized = true;
    }

    private void OnClientChanged(ulong clientId)
    {
        if (!IsSpawned) 
            return;

        UpdatePlayersList();

        if (IsHost)
        {
            UpdatePlayersListClientRpc();
        }
    }

    [ClientRpc]
    private void UpdatePlayersListClientRpc()
    {
        if (!IsHost) 
        {
            UpdatePlayersList();
        }
    }

    private void UpdatePlayersList()
    {
        if (!_isInitialized || NetworkManager.Singleton == null)
            return;

        ClearList();

        foreach (var client in NetworkManager.Singleton.ConnectedClients.Values)
        {
            AddPlayerEntry(client);
        }

        eyesToggler.HandlePlayerLeft();
    }

    private void AddPlayerEntry(NetworkClient client)
    {
        if (client.PlayerObject == null)
            return;

        var entry = Instantiate(_playerEntryPrefab, _contentParent);
        var text = entry.GetComponentInChildren<TMP_Text>();
        var image = entry.GetComponent<Image>();

        if (client.PlayerObject.TryGetComponent<PlayerNameNetwork>(out var nameNet))
        {
            text.text = nameNet.PlayerName.Value.ToString();

            nameNet.PlayerName.OnValueChanged += (oldValue, newValue) =>
            {
                text.text = newValue.ToString();
            };

            image.color = client.ClientId == NetworkManager.ServerClientId
                ? _hostColor
                : _clientColor;
        }
    }

    private void ClearList()
    {
        if (_contentParent == null)
            return;

        foreach (Transform child in _contentParent)
        {
            Destroy(child.gameObject);
        }
    }
}