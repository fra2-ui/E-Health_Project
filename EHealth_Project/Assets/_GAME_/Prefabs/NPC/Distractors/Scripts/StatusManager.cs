using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class StatusManager : MonoBehaviour
{
    public static StatusManager Instance { get; private set; }

    [Header("Wellbeing")]
    [Range(0f, 100f)] public float wellbeing = 100f;

    [Header("Alcohol PostFX (URP Volume)")]
    public Volume alcoholVolume;
    public float fadeIn = 0.35f;
    public float fadeOut = 0.35f;

    [Header("Vices / Trauma States")]
    public bool controlsInverted = false;
    public bool traumaLagActive = false;

    [Header("Trauma Lag Tuning")]
    public int traumaFramesBack = 12;
    public float traumaTickInterval = 0.20f;
    [Range(0f, 1f)] public float traumaTickChance = 0.55f;

    // ✅ UI BAR
    [Header("UI - Wellbeing Bar")]
    [Tooltip("Prefab con Canvas + WellbeingBarUI. Va assegnato in Inspector sullo StatusManager.")]
    public GameObject wellbeingBarPrefab;

    [Tooltip("Nome scena dove vuoi creare la barra la prima volta (es: MainTown).")]
    public string createBarInSceneName = "MainTown";

    private GameObject _barInstance;

    private Coroutine _alcoholRoutine;
    private Coroutine _invertRoutine;
    private Coroutine _traumaRoutine;

    // =========================================================
    // ✅ CHARACTER / STORY GATE (lettura dal GameManager)
    // =========================================================
    public int SelectedStoryIndex
    {
        get
        {
            if (GameManager.Instance == null) return -1;
            return GameManager.Instance.selectedStoryIndex;
        }
    }

    public bool IsSelectedStory(int storyIndex)
    {
        int idx = SelectedStoryIndex;
        // se non sappiamo chi è selezionato (es. test), NON blocchiamo nulla
        if (idx < 0) return true;
        return idx == storyIndex;
    }
    // =========================================================

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (alcoholVolume == null)
            alcoholVolume = GetComponentInChildren<Volume>(true);

        if (alcoholVolume == null)
            Debug.LogWarning("[StatusManager] alcoholVolume ancora NULL: metti AlcoholVolume come figlio o assegnalo in Inspector.");

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryEnsureWellbeingBar(scene.name);
    }

    private void TryEnsureWellbeingBar(string sceneName)
    {
        if (sceneName != createBarInSceneName) return;
        if (_barInstance != null) return;

        if (wellbeingBarPrefab == null)
        {
            Debug.LogWarning("[StatusManager] wellbeingBarPrefab NON assegnato -> la barra non può essere creata.");
            return;
        }

        _barInstance = Instantiate(wellbeingBarPrefab);
        DontDestroyOnLoad(_barInstance);

        var ui = _barInstance.GetComponentInChildren<WellbeingBarUI>(true);
        if (ui != null) ui.ForceRefresh();

        Debug.Log("[StatusManager] Wellbeing Bar creata e resa persistente.");
    }

    // ------------------------
    // WELLBEING
    // ------------------------
    public void ApplyWellbeingDrop(float amount, string sourceId)
    {
        if (amount <= 0f) return;
        wellbeing = Mathf.Clamp(wellbeing - amount, 0f, 100f);
        Debug.Log($"[StatusManager] Wellbeing -{amount} ({sourceId}) -> {wellbeing}");
        WellbeingBarUI.NotifyWellbeingChanged();
    }

    public void AddWellbeing(float amount, string sourceId = "unknown")
    {
        if (amount <= 0f) return;
        wellbeing = Mathf.Clamp(wellbeing + amount, 0f, 100f);
        Debug.Log($"[StatusManager] Wellbeing +{amount} ({sourceId}) -> {wellbeing}");
        WellbeingBarUI.NotifyWellbeingChanged();
    }

    // ------------------------
    // ALCOHOL FX
    // ------------------------
    public void TriggerAlcoholEffect(float durationSeconds)
    {
        if (alcoholVolume == null)
        {
            Debug.LogWarning("[StatusManager] alcoholVolume non assegnato.");
            return;
        }

        if (_alcoholRoutine != null)
            StopCoroutine(_alcoholRoutine);

        _alcoholRoutine = StartCoroutine(AlcoholRoutine(durationSeconds));
    }

    private IEnumerator AlcoholRoutine(float duration)
    {
        yield return FadeVolume(alcoholVolume, 0f, 1f, fadeIn);
        yield return new WaitForSeconds(duration);
        yield return FadeVolume(alcoholVolume, 1f, 0f, fadeOut);
    }

    private IEnumerator FadeVolume(Volume v, float from, float to, float time)
    {
        v.weight = from;

        if (time <= 0f)
        {
            v.weight = to;
            yield break;
        }

        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            v.weight = Mathf.Lerp(from, to, t / time);
            yield return null;
        }

        v.weight = to;
    }

    // ------------------------
    // INVERTED CONTROLS
    // ------------------------
    public void TriggerInvertedControls(float durationSeconds)
    {
        if (_invertRoutine != null)
            StopCoroutine(_invertRoutine);

        _invertRoutine = StartCoroutine(InvertedControlsRoutine(durationSeconds));
    }

    private IEnumerator InvertedControlsRoutine(float duration)
    {
        controlsInverted = true;
        yield return new WaitForSeconds(duration);
        controlsInverted = false;
    }

    // ------------------------
    // TRAUMA LAG
    // ------------------------
    public void TriggerTraumaLag(float durationSeconds)
    {
        if (_traumaRoutine != null)
            StopCoroutine(_traumaRoutine);

        _traumaRoutine = StartCoroutine(TraumaLagRoutine(durationSeconds));
    }

    private IEnumerator TraumaLagRoutine(float duration)
    {
        traumaLagActive = true;
        yield return new WaitForSeconds(duration);
        traumaLagActive = false;
    }
}