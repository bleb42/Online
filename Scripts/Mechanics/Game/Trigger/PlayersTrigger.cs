using Unity.Netcode;
using UnityEngine;

public class PlayersTrigger : MonoBehaviour
{
    [SerializeField] private EventsInvoker _eventsInvoker;

    private int _currentPlayers = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out CharacterController player))
        {
            _currentPlayers++;

            Check();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CharacterController player))
        {
            _currentPlayers--;

            Check();
        }
    }

    private void Check()
    {
        if(NetworkManager.Singleton == null)
        {
            Debug.Log("No Network");
            return;
        }    

        if (_currentPlayers == NetworkManager.Singleton.ConnectedClients.Count)
            _eventsInvoker.InvokeEvents();
    }
}