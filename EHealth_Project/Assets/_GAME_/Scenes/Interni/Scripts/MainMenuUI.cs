using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    // Nome della scena di gioco iniziale
    [SerializeField] private string firstGameScene = "LIBRARY"; // o "World", "MainTown", ecc.

    public void OnStartGame()
    {
        SceneManager.LoadScene(firstGameScene);
    }

    public void OnQuitGame()
    {
        // In editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // In build
        Application.Quit();
        #endif
    }

    // Se in futuro avrai un sistema di “continue”, puoi aggiungere:
    public void OnContinueGame()
    {
        // TODO: carica scena da save
        SceneManager.LoadScene(firstGameScene);
    }
}