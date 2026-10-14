using UnityEngine;

public class ApplySpawnPosition : MonoBehaviour
{
    private void Start()
    {
        // Cerco il player nella scena esterna
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("[Spawn] Nessun oggetto con Tag 'Player' trovato in scena.");
            return;
        }

        if (SpawnManager.NextSpawnPosition.HasValue)
        {
            Debug.Log("[Spawn] Applico spawn speciale: " + SpawnManager.NextSpawnPosition.Value);
            player.transform.position = SpawnManager.NextSpawnPosition.Value;
            SpawnManager.NextSpawnPosition = null;  // consumato
        }
        else
        {
            Debug.Log("[Spawn] Nessuno spawn speciale, uso la posizione di default del player.");
        }
    }
}