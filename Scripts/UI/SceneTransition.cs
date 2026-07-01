using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : NetworkBehaviour
{
    private const string GameSceneNameHash = "Main";
    private const string StartTransitionHash = "FadeOut";
    private const string EndTransitionHash = "FadeIn";

    [SerializeField] private Animator _transitionScreenAnimator;
    [SerializeField] private GameObject _transitionPanel;

    public static SceneTransition Instance;

    private bool _isInTransition = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _transitionPanel.SetActive(false);
    }


    [ClientRpc]
    public void StartFadeInClientRpc()
    {
        _transitionScreenAnimator.SetTrigger(EndTransitionHash);
        _isInTransition = false;
    }

    public void StartSceneTransition()
    {
        if (_isInTransition == true)
            return;

        if (IsHost)
        {
            StartTransitionClientRpc();
            _isInTransition = true;
        }
    }

    [ClientRpc]
    private void StartTransitionClientRpc()
    {
        _transitionPanel.SetActive(true);
        _transitionScreenAnimator.SetTrigger(StartTransitionHash);
    }

    public void OnFadeOutComplete()
    {
        if (IsHost)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(GameSceneNameHash, LoadSceneMode.Single);
        }
    }
}