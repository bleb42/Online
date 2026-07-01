using UnityEngine;

public class CharacterInstance : MonoBehaviour 
{
    [SerializeField] private string _characterName;

    public string CharacterName => _characterName;
}