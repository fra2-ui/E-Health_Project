using UnityEngine;

public class QuitGameButton : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Hai premuto QUIT!");

        // Chiude il gioco nella build
        Application.Quit();

        // Ferma il Play Mode in editor (solo per test)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}