using System.Collections;
using UnityEngine;
using Unity.Cinemachine; // Assicurati di avere questo per collegare la camera

public class BusAnimation : MonoBehaviour
{
    [Header("Riferimenti Scena")]
    public Transform bus;
    public Transform busStopPoint;
    public Transform busExitPoint;

    [Tooltip("Il punto esatto (Empty Object) dove deve apparire il personaggio")]
    public Transform playerSpawnPoint;

    [Tooltip("Trascina qui la CinemachineCamera della scena per farla agganciare al player")]
    public CinemachineCamera vcam;

    [Header("Settings")]
    public float speed = 7f;
    public float waitAtStop = 1.5f;
    public DialogueBox dialogueBox;

    // Variabile interna per ricordarsi chi abbiamo spawnato
    private GameObject spawnedPlayer;

    IEnumerator Start()
    {
        // Se la cutscene NON è stata giocata
        if (!GameSession.playedBusCutscene)
        {
            // Avvia la sequenza
            StartCoroutine(Cutscene());
        }
        else
        {
            // Se è già stata giocata, spawna subito il personaggio senza animazione
            SpawnSelectedCharacter();

            // Spegni il bus
            if (bus != null) bus.gameObject.SetActive(false);
        }
        yield return null;
    }

    // Funzione semplice che crea il personaggio
    void SpawnSelectedCharacter()
    {
        // 1. Controlla se abbiamo i dati nel GameManager
        if (GameManager.Instance != null && GameManager.Instance.selectedCharacter != null)
        {
            // 2. Prendi il PREFAB dal GameManager
            GameObject prefabToSpawn = GameManager.Instance.selectedCharacter.gameplayPrefab;

            // 3. CREALO (Instantiate) nella posizione del playerSpawnPoint
            spawnedPlayer = Instantiate(prefabToSpawn, playerSpawnPoint.position, Quaternion.identity);

            // 4. Collega la videocamera al nuovo personaggio
            if (vcam != null)
            {
                // 1. Assegna il bersaglio
                vcam.Follow = spawnedPlayer.transform;

                // 2. MAGIA: Dimentica dove eri prima, scatta subito qui!
                // (Se usi Unity 6 o Cinemachine 3, il comando potrebbe essere diverso, vedi sotto)
                vcam.PreviousStateIsValid = false;

                // Se 'PreviousStateIsValid' ti dà errore rosso, usa invece:
                // vcam.OnTargetObjectWarped(vcam.Follow, spawnedPlayer.transform.position - vcam.transform.position);
            }
        }
        else
        {
            Debug.LogError("ERRORE: Non hai selezionato nessun personaggio nel Menu!");
        }
    }

    IEnumerator Cutscene()
    {
        // 1. BUS ENTRA
        while (bus != null && Mathf.Abs(bus.position.x - busStopPoint.position.x) > 0.1f)
        {
            float newX = Mathf.MoveTowards(bus.position.x, busStopPoint.position.x, speed * Time.deltaTime);
            bus.position = new Vector3(newX, bus.position.y, bus.position.z);
            yield return null;
        }

        // 2. ATTESA
        yield return new WaitForSeconds(waitAtStop);

        // --- 3. SPAWN DEL PERSONAGGIO ---
        // Qui creiamo fisicamente il personaggio scelto
        SpawnSelectedCharacter();

        // Blocchiamo momentaneamente il movimento del nuovo personaggio finché il bus non parte
        Player_Controller pc = spawnedPlayer.GetComponent<Player_Controller>();
        if (pc != null) pc.canMove = false;

        // 4. BUS RIPARTE
        while (bus != null && Mathf.Abs(bus.position.x - busExitPoint.position.x) > 0.1f)
        {
            float newX = Mathf.MoveTowards(bus.position.x, busExitPoint.position.x, speed * Time.deltaTime);
            bus.position = new Vector3(newX, bus.position.y, bus.position.z);
            yield return null;
        }

        // 5. FINE
        GameSession.playedBusCutscene = true;

        // Riabilita il movimento
        if (pc != null) pc.canMove = true;

        // Dialogo
        if (!GameSession.shownIntroDialogue && dialogueBox != null)
        {
            GameSession.shownIntroDialogue = true;
            dialogueBox.ShowMessage("Finalmente ci sono... questa città sembra enorme.");
        }

        if (bus != null) bus.gameObject.SetActive(false);
    }
}