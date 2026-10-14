using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [TextArea(2, 4)]
    public string[] dialogueLines;   // frasi stile Pokémon

    private bool _playerInRange;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
            Debug.Log("[NPC] Player entrato nel raggio dialogo");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
            Debug.Log("[NPC] Player uscito dal raggio dialogo");
        }
    }

    private void Update()
    {
        if (!_playerInRange)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("[NPC] Player ha premuto E vicino all'NPC");

            if (DialogueUI.Instance != null)
            {
                Debug.Log("[NPC] Chiamo StartDialogue su DialogueUI");
                DialogueUI.Instance.StartDialogue(dialogueLines);
            }
            else
            {
                Debug.LogError("[NPC] DialogueUI.Instance è NULL");
            }
        }
    }
}