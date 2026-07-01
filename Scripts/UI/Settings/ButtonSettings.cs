using UnityEngine;

public class ButtonSettings : MonoBehaviour
{
    [Header("Sound Settings")]
    public AudioClip HoverSound;
    public AudioClip ClickSound;

    [Header("Color Settings")]
    public Color OriginalColor = new Color(1f, 1f, 1f, 1f);
    public Color HoverColor = new Color(1f, 1f, 1f, 0.8f);
    public Color ClickColor = new Color(1f, 1f, 1f, 0.5f);
    public float FadeDuration = 0.1f;

    public static ButtonSettings Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
