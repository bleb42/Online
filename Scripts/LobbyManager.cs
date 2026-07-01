using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;

public class LobbyManager : NetworkBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _lobbyPanel;
    [SerializeField] private GameObject _lobbyClosedPanel;
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private GameObject _joinRoomPanel;

    [Header("Main Panel Elements")]
    [SerializeField] private TMP_InputField _codeInput;

    [Header("Lobby Panel Elements")]
    [SerializeField] private GameObject _startGameButton;
    [SerializeField] private TMP_Text _lobbyCodeText;
    [SerializeField] private OffScreen _offScreen;

    private string _joinCode;

    private void Awake()
    {
        _offScreen.gameObject.SetActive(true);
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

        _mainPanel.SetActive(true);
        _lobbyPanel.SetActive(false);
    }

    private void OnEnable()
    {
        _offScreen.FadedOut += OnScreenFadeOut;
        _offScreen.FadedIn += OnScreenFadedIn;
    }

    private void OnDisable()
    {
        _offScreen.FadedOut -= OnScreenFadeOut;
        _offScreen.FadedIn -= OnScreenFadedIn;
    }

    public void ExitLobby()
    {
        if (IsHost)
        {
            NotifyClientRpc();

            NetworkManager.Singleton.Shutdown();
        }
        else
        {
            NetworkManager.Singleton.Shutdown();
        }

        _joinCode = null;
        _codeInput.text = "";

        _lobbyPanel.SetActive(false);
    }

    private void OnScreenFadedIn()
    {
        _mainPanel.GetComponent<PanelController>().Close();
    }

    private void OnScreenFadeOut()
    {
        _mainPanel.GetComponent<PanelController>().Open();
    }

    [ClientRpc]
    private void NotifyClientRpc()
    {
        if (!IsHost) 
        {
            _lobbyPanel.SetActive(false);
            _lobbyClosedPanel.SetActive(true);

            NetworkManager.Singleton.Shutdown();
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        _mainPanel.SetActive(false);
        _lobbyPanel.SetActive(true);

        _lobbyCodeText.text = IsHost ? _joinCode : _codeInput.text;

        _startGameButton.SetActive(IsHost);
    }

    public async void CreateLobby()
    {
        _mainPanel.SetActive(false);
        _loadingPanel.SetActive(true);

        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4);
            _joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
            );

            NetworkManager.Singleton.StartHost();
        }
        catch
        {
            _mainPanel.SetActive(true);
        }

        _loadingPanel.SetActive(false);
    }

    public async void JoinLobby()
    {
        _mainPanel.SetActive(false);
        _loadingPanel.SetActive(true);

        try
        {
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(_codeInput.text);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData,
                allocation.HostConnectionData
            );

            NetworkManager.Singleton.StartClient();
            _lobbyCodeText.text = $"Lobby Code: {_codeInput.text}";
        }
        catch
        {
            _joinRoomPanel.SetActive(true);
        }

        _loadingPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }
}