using UnityEngine;

public class ApplySelectedCharacterSkin : MonoBehaviour
{
    [Header("Where to read the skin from (prefab chosen in menu)")]
    [SerializeField] private string sourceVisualPath = "Visuals/Character"; // nel prefab scelto

    [Header("Where to apply the skin on THIS player (the probe player)")]
    [SerializeField] private Animator targetAnimator;     // quello usato dal Player_Controller
    [SerializeField] private SpriteRenderer targetRenderer;

    private void Awake()
    {
        if (targetAnimator == null) targetAnimator = GetComponentInChildren<Animator>(true);
        if (targetRenderer == null) targetRenderer = GetComponentInChildren<SpriteRenderer>(true);
    }

    private void Start()
    {
        var gm = GameManager.Instance;
        if (gm == null || gm.selectedCharacter == null || gm.selectedCharacter.gameplayPrefab == null)
            return;

        // Prendo l'Animator del prefab scelto
        Transform src = gm.selectedCharacter.gameplayPrefab.transform.Find(sourceVisualPath);
        if (src == null)
        {
            Debug.LogError($"[ApplySelectedCharacterSkin] Path '{sourceVisualPath}' non trovato nel prefab '{gm.selectedCharacter.gameplayPrefab.name}'.");
            return;
        }

        var srcAnimator = src.GetComponent<Animator>();
        if (srcAnimator == null || srcAnimator.runtimeAnimatorController == null)
        {
            Debug.LogError("[ApplySelectedCharacterSkin] Nel prefab scelto manca Animator o Controller.");
            return;
        }

        // Applico SOLO il controller (non distruggo niente)
        if (targetAnimator != null)
            targetAnimator.runtimeAnimatorController = srcAnimator.runtimeAnimatorController;

        // Opzionale: se usi un override di sprite idle nel renderer, puoi copiarlo:
        // var srcRenderer = src.GetComponent<SpriteRenderer>();
        // if (srcRenderer != null && targetRenderer != null) targetRenderer.sprite = srcRenderer.sprite;
    }
}