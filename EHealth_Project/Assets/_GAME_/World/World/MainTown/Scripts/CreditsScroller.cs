using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManualEnd : MonoBehaviour
{
    [Header("Settings")]
    public float scrollSpeed = 50f;          // Speed of the text
    public string mainMenuSceneName = "MainMenu";

    [Header("References")]
    public Transform endMarker;              // DRAG THE 'EndMarker' OBJECT HERE!

    [Header("Optional")]
    public bool enableSkip = true;

    private void Update()
    {
        // 1. Move the text UP (The marker moves with it because it's a child)
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);

        // 2. Check if the 'EndMarker' has left the screen (Top)
        // We add a small buffer (100 pixels) to be sure it's fully off-screen
        if (endMarker.position.y > Screen.height + 100)
        {
            ReturnToMenu();
        }

        // 3. Skip functionality
        if (enableSkip && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            ReturnToMenu();
        }
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}