using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitBuildingPrompt : MonoBehaviour
{
    public string sceneName = "OUTWORLD";
    public Vector3 spawnPosition;

    [TextArea]
    public string promptText = "Do you want to go out?\n[E] Yes    [Q] No";

    private bool _playerInTrigger = false;
    private bool _promptDisabledUntilExit = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _playerInTrigger = true;

        // se NON è stato disabilitato, prova ad aprire
        if (!_promptDisabledUntilExit)
            TryOpenPrompt();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _playerInTrigger = false;

        // 🔓 reset: ora può riapparire la prossima volta
        _promptDisabledUntilExit = false;

        if (DialogueUI.Instance != null)
            DialogueUI.Instance.CloseIfOwner(gameObject);
    }

    private void Update()
    {
        // NON riaprire se:
        // - prompt disabilitato
        // - non siamo nel trigger
        if (!_playerInTrigger) return;
        if (_promptDisabledUntilExit) return;
        if (DialogueUI.Instance == null) return;
        if (DialogueUI.Instance.IsActive) return;

        TryOpenPrompt();
    }

    private void TryOpenPrompt()
    {
        if (DialogueUI.Instance == null) return;

        DialogueUI.Instance.TryShowPrompt(
            owner: gameObject,
            text: promptText,
            onYes: ExitBuilding,
            onNo: OnRefuseExit
        );
    }

    private void OnRefuseExit()
    {
        // ❌ rifiuto → NON deve riapparire finché non esce dal tappeto
        _promptDisabledUntilExit = true;
    }

    private void ExitBuilding()
    {
        SpawnManager.NextSpawnPosition = spawnPosition;
        SceneManager.LoadScene(sceneName);
    }
}