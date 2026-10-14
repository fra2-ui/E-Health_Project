using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine; // Unity 6 richiede questo

public class PlayerSpawnFromGameManager : MonoBehaviour
{
    private void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    private void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }
    private void Start() { StartCoroutine(ApplySpawnNextFrame()); }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) { StartCoroutine(ApplySpawnNextFrame()); }

    private IEnumerator ApplySpawnNextFrame()
    {
        yield return null;

        if (GameManager.Instance == null)
        {
            Debug.LogError("ERRORE CRITICO: GameManager non trovato!");
            yield break;
        }

        string targetId = GameManager.Instance.nextSpawnPoint;
        Debug.Log($"Cerco SpawnPoint: '{targetId}' nella scena '{SceneManager.GetActiveScene().name}'");

        if (string.IsNullOrEmpty(targetId)) yield break;

        SpawnPoint[] points = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        bool spawnFound = false;

        foreach (var p in points)
        {
            if (p.spawnId == targetId)
            {
                spawnFound = true;
                transform.position = p.transform.position;
                Debug.Log("Player posizionato correttamente.");

                // --- DIAGNOSTICA CAMERA ---
                // Cerchiamo la camera nuova (Unity 6)
                var vcam = FindAnyObjectByType<CinemachineCamera>();

                // Se non la trova, prova a cercare quella vecchia per avvisarti
                if (vcam == null)
                {
                    var oldCam = FindAnyObjectByType("Cinemachine.CinemachineVirtualCamera"); // Reflection per evitare errori di compilazione se non esiste

                    if (oldCam != null)
                    {
                        Debug.LogError("ERRORE CAMERA: Nella scena c'è una 'CinemachineVirtualCamera' (VECCHIA). Cancellala e metti una 'CinemachineCamera' (NUOVA)!");
                    }
                    else
                    {
                        Debug.LogError(" ERRORE CAMERA: Non trovo NESSUNA camera Cinemachine in questa scena. Assicurati di averne creata una!");
                    }
                }
                else
                {
                    vcam.Follow = this.transform;
                    vcam.OnTargetObjectWarped(this.transform, p.transform.position - transform.position);
                    Debug.Log($"CAMERA COLLEGATA: Ho trovato '{vcam.name}' e ora ti segue.");
                }
                // --------------------------
                break;
            }
        }

        if (!spawnFound) Debug.LogError($"ERRORE SPAWN: Non ho trovato nessun oggetto SpawnPoint chiamato '{targetId}' in questa scena!");
    }

    // Helper per trovare tipi vecchi senza rompere il codice
    private UnityEngine.Object FindAnyObjectByType(string typeName)
    {
        foreach (var asm in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            var type = asm.GetType(typeName);
            if (type != null) return FindAnyObjectByType(type);
        }
        return null;
    }
}