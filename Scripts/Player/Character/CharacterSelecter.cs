using UnityEngine;

public class CharacterSelecter : MonoBehaviour
{
    private const string SelectedCharacterKey = "SelectedCharacter";
    private const string DefaultCharacterKey = "Default";

    [SerializeField] private CharacterData[] _allCharactersData;
    [SerializeField] private CharacterInstance[] _sceneCharacters;

    private void Start()
    {
        string savedName = PlayerPrefs.GetString(SelectedCharacterKey, DefaultCharacterKey);
        ApplySelection(savedName);
    }

    public void SelectCharacter(CharacterData character)
    {
        PlayerPrefs.SetString(SelectedCharacterKey, character.Name);
        PlayerPrefs.Save();

        ApplySelection(character.Name);
    }

    private void ApplySelection(string selectedName)
    {
        foreach (var character in _sceneCharacters)
        {
            character.gameObject.SetActive(character.CharacterName == selectedName);
        }
    }
}