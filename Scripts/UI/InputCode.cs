using TMPro;
using UnityEngine;

public class InputCode : MonoBehaviour
{
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.characterLimit = 6;
        inputField.onValueChanged.AddListener(ValidateInput);
    }

    private void ValidateInput(string value)
    {
        string filtered = "";
        foreach (char c in value.ToUpper())
        {
            if ((c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9'))
            {
                filtered += c;
            }
        }

        if (filtered != inputField.text)
        {
            inputField.text = filtered;
        }
    }
}
