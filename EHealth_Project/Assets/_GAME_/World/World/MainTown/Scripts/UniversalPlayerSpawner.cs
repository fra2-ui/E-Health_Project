using UnityEngine;
using Unity.Cinemachine;

public class UniversalPlayerSpawner : MonoBehaviour
{
    public CinemachineCamera vcam;

    // Se vuoi testare senza passare dal menu, trascina qui AlexData
    public CharacterData debugCharacter;

    void Start()
    {
        // 1. PULIZIA (Via le marionette)
        foreach (Transform child in transform) Destroy(child.gameObject);

        // 2. CHI DEVO CREARE?
        CharacterData data = null;

        // Priorità al GameManager (Gioco vero)
        if (GameManager.Instance != null && GameManager.Instance.selectedCharacter != null)
        {
            data = GameManager.Instance.selectedCharacter;
        }
        // Altrimenti Debug
        else if (debugCharacter != null)
        {
            data = debugCharacter;
        }

        if (data == null)
        {
            Debug.LogError("Non so chi spawnare!");
            return;
        }

        // 3. SPAWN BRUTALE (Esattamente dove è questo oggetto ORA)
        // transform.position = Le coordinate X,Y,Z di questo oggetto PlayerStartPoint
        GameObject player = Instantiate(data.gameplayPrefab, transform.position, Quaternion.identity);

        // 4. TELECAMERA
        if (vcam != null)
        {
            vcam.Follow = player.transform;
            vcam.PreviousStateIsValid = false;
        }
    }
}