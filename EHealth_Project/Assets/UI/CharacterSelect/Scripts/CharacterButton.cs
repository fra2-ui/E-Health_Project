using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public CharacterData characterData;

    public void OnPointerEnter(PointerEventData eventData)
    {
        FindFirstObjectByType<CharacterSelectManager>()?.OnCharacterHover(this);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        FindFirstObjectByType<CharacterSelectManager>()?.OnCharacterSelected(this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        FindFirstObjectByType<CharacterSelectManager>()?.OnHoverExit();
    }
}
