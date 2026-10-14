using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 

public class CharacterSelectManager : MonoBehaviour
{
    [Header("Riferimenti")]
    public TypewriterEffect typewriter;
    public List<CharacterButton> buttons;
    private CharacterButton hoveredButton;
    private CharacterButton selectedButton;

    
    public static int selectedStoryIndex = 0;

    public void OnCharacterHover(CharacterButton button)
    {
        hoveredButton = button;
        if (selectedButton != button)
        {
            string text = $"{button.characterData.characterName}\n{button.characterData.description}";
            typewriter.ShowText(text);
        }
        UpdateButtonVisuals();
    }

    public void OnCharacterSelected(CharacterButton button)
    {
        selectedButton = button;
        string text = $"{button.characterData.characterName}\n{button.characterData.description}";
        typewriter.ShowText(text);

        
        selectedStoryIndex = buttons.IndexOf(button);
        Debug.Log($"Hai scelto il personaggio numero: {selectedStoryIndex}");

        
        if (GameManager.Instance != null)
        {
            
            GameManager.Instance.SetCharacter(button.characterData, selectedStoryIndex);
        }
        else
        {
            Debug.LogError("ATTENZIONE: Manca il GameManager nella scena del Menu!");
        }

        
        
        LoadingManager.nextScene = "OutWorld";

        
        SceneManager.LoadScene("LoadingScene");

        UpdateButtonVisuals();
    }

    public void OnHoverExit()
    {
        hoveredButton = null;
        UpdateButtonVisuals();
    }

    private void UpdateButtonVisuals()
    {
        foreach (var b in buttons)
        {
            var image = b.GetComponent<Image>();
            var color = image.color;
            if (b == selectedButton)
                color.a = 1f;
            else if (b == hoveredButton)
                color.a = 0.8f;
            else
                color.a = 0.4f;
            image.color = color;
        }
    }
}