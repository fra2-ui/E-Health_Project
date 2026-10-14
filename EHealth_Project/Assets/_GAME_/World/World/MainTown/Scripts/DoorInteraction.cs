using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;   // nuovo Input System

public class DoorInteraction : MonoBehaviour
{
    [Header("Spawn")]
    public string targetSpawnPoint;   // spawnId nella scena di ARRIVO

    [Header("Scene")]
    public string sceneToLoad;          // Nome della scena da caricare

    [Header("Components")]
    public Animator animator;           // Animator della porta
    public Collider2D solidCollider;    // Collider solido (opzionale)
    public ScreenFader screenFader;     // Fader della scena

    [Header("Timing")]
    public float openAnimationTime = 0.7f;  // Durata animazione apertura

    [Header("Animator")]
    public string openTriggerName = "Open"; // Nome del trigger nell'Animator

    private bool isPlayerInside = false;
    private bool hasBeenUsed = false;

    void Reset()
    {
        animator = GetComponent<Animator>();
        solidCollider = GetComponent<Collider2D>();
    }

    void Awake()
    {
        // Se non li hai messi a mano nell’inspector, prova a recuperarli
        if (animator == null)
            animator = GetComponent<Animator>();

        if (solidCollider == null)
            solidCollider = GetComponent<Collider2D>();

        if (screenFader == null)
            screenFader = FindFirstObjectByType<ScreenFader>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerInside = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerInside = false;
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (isPlayerInside && !hasBeenUsed && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            StartCoroutine(UseDoor());
        }
    }

    IEnumerator UseDoor()
    {
        hasBeenUsed = true;

        // 1) Animazione porta
        if (animator != null && !string.IsNullOrEmpty(openTriggerName))
            animator.SetTrigger(openTriggerName);

        // Attesa animazione
        yield return new WaitForSeconds(openAnimationTime);

        // 2) Disattivo eventuale collider solido
        if (solidCollider != null)
            solidCollider.enabled = false;

        if (GameManager.Instance != null && !string.IsNullOrEmpty(targetSpawnPoint))
        {
            GameManager.Instance.nextSpawnPoint = targetSpawnPoint;
        }

        // 3) Fade + cambio scena
        if (screenFader != null && !string.IsNullOrEmpty(sceneToLoad))
        {
            yield return StartCoroutine(screenFader.FadeOutInAndLoad(sceneToLoad));
        }
        else if (!string.IsNullOrEmpty(sceneToLoad))
        {
            // fallback: niente fade, cambio diretto
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
