using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitSession : MonoBehaviour
{
    private const string MenuHash = "Menu";

    public void Exit()
    {
        SceneManager.LoadScene(MenuHash);
     
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.Shutdown();
    }
}