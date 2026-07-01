using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Game/Character")]
public class CharacterData : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public GameObject Prefab;
}