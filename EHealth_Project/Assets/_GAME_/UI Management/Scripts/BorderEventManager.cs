using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BorderEventManager : MonoBehaviour
{
    [Header("Riferimenti Player")]
    public MonoBehaviour playerMovementScript;
    public Rigidbody2D playerRB;

    [Header("Riferimenti Macchina Specifica")]
    public GameObject carObject;    // Trascina qui la macchina specifica di questo lato (es. Macchina Rossa)
    public Transform startPoint;    // Punto A di questo lato
    public Transform endPoint;      // Punto B di questo lato

    [Header("Impostazioni")]
    public float carSpeed = 20f;
    public float messageDuration = 4.0f;

    [Header("Riferimenti UI")]
    public GameObject warningBoxUI; // Trascina qui SEMPRE LO STESSO box della UI per tutti e 4 i manager

    private bool isEventActive = false;

    void Start()
    {
        // Assicuriamoci che la macchina gestita da questo manager sia spenta all'inizio
        if (carObject != null) carObject.SetActive(false);

        // La UI la spegniamo, ma attenzione: se hai 4 manager, tutti proveranno a spegnerla.
        // Non � un problema, ma � bene saperlo.
        if (warningBoxUI != null) warningBoxUI.SetActive(false);
    }

    public void StartEasterEggEvent()
    {
        if (isEventActive) return;
        StartCoroutine(MainEventSequence());
    }

    IEnumerator MainEventSequence()
    {
        isEventActive = true;

        // 1. Blocca Player
        if (playerMovementScript != null) playerMovementScript.enabled = false;
        if (playerRB != null) playerRB.linearVelocity = Vector2.zero;

        // 2. Mostra UI
        if (warningBoxUI != null) warningBoxUI.SetActive(true);

        // 3. Muovi la Macchina
        StartCoroutine(MoveCarRoutine());

        // 4. Aspetta tempo lettura
        yield return new WaitForSeconds(messageDuration);

        // 5. Ripristina
        if (warningBoxUI != null) warningBoxUI.SetActive(false);
        if (playerMovementScript != null) playerMovementScript.enabled = true;

        yield return new WaitForSeconds(1f);
        isEventActive = false;
    }

    IEnumerator MoveCarRoutine()
    {
        // Posizionamento forzato (resetta la posizione all'inizio)
        carObject.transform.position = new Vector3(startPoint.position.x, startPoint.position.y, 0);

        // Accendi la macchina
        carObject.SetActive(true);

        float safetyTimer = 0f;

        // Muovi finch� non arriva
        while (Vector2.Distance(carObject.transform.position, endPoint.position) > 0.5f)
        {
            carObject.transform.position = Vector3.MoveTowards(
                carObject.transform.position,
                endPoint.position,
                carSpeed * Time.deltaTime
            );

            safetyTimer += Time.deltaTime;
            if (safetyTimer > 5f) break; // Timeout di sicurezza

            yield return null;
        }

        // Spegni la macchina
        carObject.SetActive(false);
    }
}