using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class StartGame : NetworkBehaviour
{
    [SerializeField] private UnityEvent _onAllPlayersReady;
    [SerializeField] private float _delay = 3f;

    private Coroutine _delayCoroutine;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) 
            return;

        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += AllClientsLoaded;
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer && NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= AllClientsLoaded;
        }
    }

    private void AllClientsLoaded(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadMode, System.Collections.Generic.List<ulong> clientsCompleted, System.Collections.Generic.List<ulong> clientsTimedOut)
    {
        if (_delayCoroutine != null) 
            StopCoroutine(_delayCoroutine);

        _delayCoroutine = StartCoroutine(WaitingForStartGame(_delay));
    }

    [ClientRpc]
    private void StartGameClientRpc()
    {
        _onAllPlayersReady?.Invoke();
    }

    private IEnumerator WaitingForStartGame(float delay)
    {
        yield return new WaitForSeconds(delay);

        StartGameClientRpc();   
    }

    ////Test
    //private void Start()
    //{
    //    StartCoroutine(WaitingForStartGame1(_delay));
    //}

    //private IEnumerator WaitingForStartGame1(float delay)
    //{
    //    yield return new WaitForSeconds(delay);

    //    _onAllPlayersReady?.Invoke();
    //}
}