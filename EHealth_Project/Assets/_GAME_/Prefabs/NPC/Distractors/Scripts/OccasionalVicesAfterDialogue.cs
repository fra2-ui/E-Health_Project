using UnityEngine;

public class OccasionalVicesAfterDialogue : MonoBehaviour
{
    [Header("Character Gate (0=Alex, 1=Adley, 2=Colt, 3=Anna)")]
    public int requiredStoryIndex = 3;

    [Header("Invert Controls")]
    public float invertDuration = 60f;

    [Header("Wellbeing")]
    public float wellbeingDrop = 15f;
    public string sourceId = "VicesDistractor";

    private bool _playerInside = false;
    private bool _wasDialogueOpen = false;
    private bool _waitingDialogueEnd = false;

    private void Update()
    {
        if (!_playerInside) return;
        if (DialogueUI.Instance == null) return;

        // ✅ attivo SOLO se il personaggio selezionato è quello giusto
        if (StatusManager.Instance != null && !StatusManager.Instance.IsSelectedStory(requiredStoryIndex))
            return;

        bool open = DialogueUI.Instance.IsActive;

        if (!_wasDialogueOpen && open)
            _waitingDialogueEnd = true;

        if (_wasDialogueOpen && !open && _waitingDialogueEnd)
        {
            _waitingDialogueEnd = false;

            if (StatusManager.Instance != null)
            {
                StatusManager.Instance.TriggerInvertedControls(invertDuration);
                StatusManager.Instance.ApplyWellbeingDrop(wellbeingDrop, sourceId);
            }
        }

        _wasDialogueOpen = open;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _playerInside = false;
        _waitingDialogueEnd = false;
        _wasDialogueOpen = false;
    }
}