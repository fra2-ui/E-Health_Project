using UnityEngine;

public class QuestNPC : MonoBehaviour
{
    [Header("--- Identity ---")]
    public string npcID; 

    [Header("--- Default Dialogue ---")]
    [TextArea] public string[] defaultLines; 

    private bool _playerInZone = false;

    private void Update()
    {
        if (_playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            if (DialogueUI.Instance != null && !DialogueUI.Instance.IsActive)
            {
                Interact();
            }
        }
    }

    void Interact()
    {
        if (QuestManager.Instance == null) return;

        MissionStep activeStep = QuestManager.Instance.GetActiveStepForNPC(npcID);

        if (activeStep != null)
        {
            Debug.Log($"[NPC {npcID}] Dialogo QUEST attivo.");
            DialogueUI.Instance.StartDialogue(activeStep.dialogueLines, true);
        }
        else
        {
            Debug.Log($"[NPC {npcID}] Dialogo DEFAULT.");
            DialogueUI.Instance.StartDialogue(defaultLines, false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _playerInZone = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _playerInZone = false;
    }
}