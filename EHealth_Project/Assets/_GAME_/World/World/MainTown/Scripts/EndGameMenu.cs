using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour
{
    [Header("Nomi delle Scene")]
    public string mainMenuSceneName = "MainMenu"; // Scrivi qui il nome esatto della scena Menu
    public string creditsSceneName = "Credits";   // Scrivi qui il nome esatto della scena Titoli

    // Funzione per il bottone YES
    public void OnYesClicked()
    {
        // 1. Distruggiamo il GameManager per resettare inventario, vita, ecc.
        // Se non lo facciamo, iniziando una nuova partita avrai i dati vecchi!
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        // 2. Carichiamo il Main Menu
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Funzione per il bottone NO
    public void OnNoClicked()
    {
        // Carichiamo la scena dei titoli di coda
        SceneManager.LoadScene(creditsSceneName);
    }
}