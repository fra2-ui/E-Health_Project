using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SocialPopupUI : MonoBehaviour
{
    [SerializeField] private Image popupImage;
    [SerializeField] private TextMeshProUGUI keyText;

    public KeyCode RequiredKey { get; private set; }
    public RectTransform Rect => (RectTransform)transform;

    public void Init(Sprite sprite, KeyCode key)
    {
        RequiredKey = key;

        if (popupImage != null)
            popupImage.sprite = sprite;

        if (keyText != null)
            keyText.text = key.ToString().ToUpper();
    }
}