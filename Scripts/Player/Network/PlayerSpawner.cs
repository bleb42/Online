using Synty.AnimationBaseLocomotion.Samples.InputSystem;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    private const string SelectedCharacterKey = "SelectedCharacter";

    [SerializeField] private CharacterData[] characterDatas;
    [SerializeField] private Transform[] spawnPositions;

    private readonly Dictionary<ulong, string> clientCharacterSelections = new();

    private int _nextSpawnIndex = 0;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;

        string hostCharName = PlayerPrefs.GetString(SelectedCharacterKey, characterDatas[0].Name);
        SpawnPlayer(NetworkManager.LocalClientId, hostCharName);

        foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (clientId != NetworkManager.LocalClientId)
            {
                RequestCharacterClientRpc(new ClientRpcParams
                {
                    Send = new ClientRpcSendParams { TargetClientIds = new[] { clientId } }
                });
            }
        }

        SceneTransition.Instance?.StartFadeInClientRpc();
    }

    [ClientRpc]
    private void RequestCharacterClientRpc(ClientRpcParams rpcParams)
    {
        string selectedChar = PlayerPrefs.GetString(SelectedCharacterKey, characterDatas[0].Name);
        SendCharacterSelectionServerRpc(selectedChar);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendCharacterSelectionServerRpc(string characterName, ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        if (clientCharacterSelections.ContainsKey(clientId) == false)
            clientCharacterSelections.Add(clientId, characterName);

        SpawnPlayer(clientId, characterName);
    }

    private void SpawnPlayer(ulong clientId, string characterName)
    {
        GameObject prefab = GetCharacterPrefab(characterName);
        Vector3 spawnPos = GetSpawnPosition();

        GameObject player = Instantiate(prefab, spawnPos, Quaternion.identity);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

        InitializeUIClientRpc(new ClientRpcParams
        {
            Send = new ClientRpcSendParams { TargetClientIds = new[] { clientId } }
        });
    }

    private GameObject GetCharacterPrefab(string name)
    {
        foreach (var data in characterDatas)
            if (data.Name == name)
                return data.Prefab;

        return characterDatas[0].Prefab;
    }

    private Vector3 GetSpawnPosition()
    {
        if (spawnPositions.Length == 0)
            return Vector3.zero;

        Vector3 position = spawnPositions[_nextSpawnIndex].position;

        _nextSpawnIndex++;

        if (_nextSpawnIndex >= spawnPositions.Length)
            _nextSpawnIndex = 0;

        return position;
    }

    [ClientRpc]
    private void InitializeUIClientRpc(ClientRpcParams rpcParams = default)
    {
        var player = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();

        if (player == null) 
            return;

        var inputReader = player.GetComponent<InputReader>();

        if (inputReader != null && PlayerMovementToggle.Instance != null)
        {
            PlayerMovementToggle.Instance.Init(inputReader);
        }
    }
}