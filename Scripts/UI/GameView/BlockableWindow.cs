using UnityEngine;

public class BlockableWindow : MonoBehaviour, IBlockableUI
{
    [SerializeField] private bool blocksGameplay = true;
    [SerializeField] private bool _isEnabledOnStart = false;

    private void Awake()
    {
        gameObject.SetActive(_isEnabledOnStart);
    }

    public GameObject Panel => gameObject;
    public bool BlocksGameplay => blocksGameplay;
}

public interface IBlockableUI
{
    GameObject Panel { get; }
    bool BlocksGameplay { get; }
}