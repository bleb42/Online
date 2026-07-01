using UnityEngine;

public class TransitionPanel : MonoBehaviour
{
    [SerializeField] private SceneTransition _sceneTransition;

    public void ChangeScene()
    {
        _sceneTransition.OnFadeOutComplete();
    }
}