using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [Header("Prefab")]
    [SerializeField] private TutorialUI tutorialCanvasPrefab;

    [Header("Where it auto-opens (city scene name)")]
    [SerializeField] private string townSceneName = "OutWorld";

    [Header("Auto-open only once")]
    [SerializeField] private bool openOnce = true;

    private TutorialUI _ui;
    private bool _alreadyAutoOpened = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void Update()
    {
        // Toggle manuale solo quando sei in città
        if (SceneManager.GetActiveScene().name == townSceneName && Input.GetKeyDown(KeyCode.T))
        {
            EnsureUI();
            if (_ui != null) _ui.Toggle();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != townSceneName) return;

        // Non aprire subito: aspetta che il player sia "a terra" dopo il bus
        StartCoroutine(WaitPlayerSpawnedAfterBusThenOpen());
    }

    private IEnumerator WaitPlayerSpawnedAfterBusThenOpen()
    {
        // 1 frame di sicurezza
        yield return null;

        EnsureUI();
        if (_ui == null) yield break;

        // Se deve aprirsi solo una volta e l'ha già fatto -> stop
        if (openOnce && _alreadyAutoOpened) yield break;

        // Aspetta che esista il Player in scena
        GameObject player = null;
        while (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            yield return null;
        }

        // Aspetta che il player sia attivo (durante cutscene del bus lo spegni)
        while (!player.activeInHierarchy)
            yield return null;

        // Aspetta che la cutscene bus sia considerata "finita"
        // (BusAnimation deve mettere GameSession.playedBusCutscene = true quando termina)
        while (!GameSession.playedBusCutscene)
            yield return null;

        // Aspetta ancora 1 frame per evitare qualsiasi “scatto” visivo
        yield return null;

        _ui.Open();
        _alreadyAutoOpened = true;
    }

    private void EnsureUI()
    {
        if (_ui != null) return;

        // 1) prova a trovarla (anche inattiva)
        _ui = FindFirstObjectByType<TutorialUI>(FindObjectsInactive.Include);

        // 2) se non esiste, istanzia il prefab
        if (_ui == null)
        {
            if (tutorialCanvasPrefab == null)
            {
                Debug.LogError("[TutorialManager] tutorialCanvasPrefab NON assegnato in Inspector.");
                return;
            }

            _ui = Instantiate(tutorialCanvasPrefab);
            _ui.name = "TutorialCanvas (Runtime)";
        }
    }
}