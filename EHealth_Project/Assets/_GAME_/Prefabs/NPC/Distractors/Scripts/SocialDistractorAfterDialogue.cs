using UnityEngine;

public class SocialDistractorAfterDialogue : MonoBehaviour
{
    [Header("Character Gate (0=Alex, 1=Adley, 2=Colt, 3=Anna)")]
    public int requiredStoryIndex = 1;

    [Header("Effect")]
    [SerializeField] private SocialNotificationsController controller;
    public int burstCount = 12;

    [Header("Wellbeing")]
    public float wellbeingDrop = 12f;
    public string sourceId = "social";

    private bool _playerInside = false;
    private bool _wasDialogueOpen = false;
    private bool _waitingDialogueEnd = false;

    private void Awake()
    {
        if (controller == null)
            controller = GetComponentInChildren<SocialNotificationsController>(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInside = true;
            _wasDialogueOpen = (DialogueUI.Instance != null && DialogueUI.Instance.IsActive);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInside = false;
            _waitingDialogueEnd = false;
            _wasDialogueOpen = false;
        }
    }

    private void Update()
    {
        if (!_playerInside) return;
        if (DialogueUI.Instance == null) return;

        // ✅ attivo SOLO se il personaggio selezionato è quello giusto
        if (StatusManager.Instance != null && !StatusManager.Instance.IsSelectedStory(requiredStoryIndex))
            return;

        bool open = DialogueUI.Instance.IsActive;

        if (!_wasDialogueOpen && open)
        {
            _waitingDialogueEnd = true;
            _wasDialogueOpen = true;
            return;
        }

        if (_waitingDialogueEnd && _wasDialogueOpen && !open)
        {
            _waitingDialogueEnd = false;
            _wasDialogueOpen = false;

            if (controller == null)
                controller = GetComponentInChildren<SocialNotificationsController>(true);

            if (controller != null)
                controller.SpawnBurst(burstCount);
            else
                Debug.LogError("[SocialDistractorAfterDialogue] controller NULL: non trovo SocialNotificationsController nei figli.");

            if (StatusManager.Instance != null)
                StatusManager.Instance.ApplyWellbeingDrop(wellbeingDrop, sourceId);
        }
    }
}