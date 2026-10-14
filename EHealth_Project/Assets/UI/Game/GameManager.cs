using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton (Accessibile da ovunque con GameManager.Instance)
    public static GameManager Instance { get; private set; }

    [Header("Dati selezionati dal giocatore")]
    // Questo serve al PlayerSpawner per sapere QUALE prefab creare
    public CharacterData selectedCharacter;

    // Questo serve all'EndingManager per sapere QUALE storia riavviare (0, 1, 2, 3)
    public int selectedStoryIndex;

    [Header("Sistema di Spawn")]
    // Questo ti servirà quando cambi stanza (es. da OutWorld a Pub) per sapere a quale porta apparire
    public string nextSpawnPoint;

    private void Awake()
    {
        // Logica Singleton: Se ne esiste già uno, distruggi il nuovo impostore
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Sopravvive ai cambi scena
    }

    // ---- Funzioni chiamate dal Menu ----

    // Questa viene chiamata dal tuo CharacterSelectManager quando clicchi un personaggio
    public void SetCharacter(CharacterData character, int storyIndex)
    {
        selectedCharacter = character;
        selectedStoryIndex = storyIndex;

        Debug.Log($"[GameManager] Salvato: {character.characterName} (Storia Indice: {storyIndex})");
    }

    // Helper opzionale
    public CharacterData GetSelectedCharacter()
    {
        return selectedCharacter;
    }
}