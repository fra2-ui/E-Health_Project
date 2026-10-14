using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    [TextArea] public string description;
    public Sprite characterSprite; // Sprite del menu

    [Header("Animazione Loading")]
    public Sprite[] loadingSprites;

    [Header("Gioco Reale")]
    public GameObject gameplayPrefab;
}