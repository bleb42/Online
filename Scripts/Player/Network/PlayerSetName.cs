using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerSetName : NetworkBehaviour
{
    private const string NicknameKey = "PlayerNickname";
    private const string DefaultName = "Piska";

    [SerializeField] private TextMeshPro _nameText;

    private string nickname;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            nickname = PlayerPrefs.GetString(NicknameKey, DefaultName);
            SetName(nickname);

            SetNameServerRpc(nickname);
        }
    }

    public void SetName(string name)
    {
        _nameText.text = name;
    }

    [ServerRpc]
    private void SetNameServerRpc(string name)
    {
        UpdateNameClientRpc(name);
    }

    [ClientRpc]
    private void UpdateNameClientRpc(string name)
    {
        SetName(name);
    }
}