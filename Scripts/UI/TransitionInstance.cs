using UnityEngine;

public class TransitionInstance : MonoBehaviour
{
    public void StartTransition()
    {
        SceneTransition.Instance.StartSceneTransition();
    }
}