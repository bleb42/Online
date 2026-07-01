using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNameNetwork : NetworkBehaviour
{
    private const string NicknameKey = "PlayerNickname";
    private const string DefaultName = "Piska";

    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>(
        new FixedString32Bytes(DefaultName),
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        gameObject.SetActive(IsOwner);
        
        if (IsOwner)
        {
            string name = PlayerPrefs.GetString(NicknameKey, DefaultName);
            PlayerName.Value = new FixedString32Bytes(name);
        }
    }
}