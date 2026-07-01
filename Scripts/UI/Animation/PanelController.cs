using UnityEngine;

public class PanelController : MonoBehaviour
{
    [SerializeField] private UIMover[] _moveable;

    public void Open()
    {
        foreach(var UIMover in _moveable)
        {
            UIMover.Open();
        }
    }

    public void Close()
    {
        foreach (var UIMover in _moveable)
        {
            UIMover.Close();
        }
    }
}