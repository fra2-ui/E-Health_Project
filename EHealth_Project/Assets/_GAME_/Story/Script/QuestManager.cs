using UnityEngine;
using UnityEngine.SceneManagement;
using System; 

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public static event Action<int> OnDayChanged;

    [Header("--- Player Data ---")]
    public int selectedCharacterIndex = 0;

    [Header("--- Database Giorni ---")]
    public DailyMission[] allDays; 
    
    [Header("--- SCENE DI RITORNO (Città) ---")]
    [Tooltip("Scena dove torni dopo aver finito il minigioco del GIORNO 2")]
    public string townSceneAfterDay2 = "TownScene"; 

    [Tooltip("Scena dove torni dopo aver finito il minigioco del GIORNO 4")]
    public string townSceneAfterDay4 = "TownScene_2"; 

    [Tooltip("Scena dove torni dopo aver finito il minigioco del GIORNO 6")]
    public string townSceneAfterDay6 = "TownScene_3"; 

    [Tooltip("Scena di default per gli altri giorni")]
    public string defaultTownScene = "TownScene"; 

    [Header("--- SPAWN POINTS DI RITORNO (Torna in Città) ---")]
    [Tooltip("ID dello SpawnPoint in città dopo il minigioco del Giorno 2")]
    public string spawnPointAfterDay2 = "Spawn_AfterMinigame1"; 

    [Tooltip("ID dello SpawnPoint in città dopo il minigioco del Giorno 4")]
    public string spawnPointAfterDay4 = "Spawn_AfterMinigame2"; 

    [Tooltip("ID dello SpawnPoint in città dopo il minigioco del Giorno 6")]
    public string spawnPointAfterDay6 = "InEndlessrun";    

    [Header("--- SPAWN POINTS DI ENTRATA (Entra nel Minigioco) ---")]
    [Tooltip("ID dello SpawnPoint INTERNO alla scena del minigioco del Giorno 4")]
    public string entrySpawnDay4 = "Spawn_StartMinigame"; 

    [Tooltip("ID dello SpawnPoint INTERNO alla scena del minigioco del Giorno 6")]
    public string entrySpawnDay6 = "Spawn_StartEndless";

    [Header("--- Audio ---")]
    public AudioSource musicPlayer; 

    [Header("--- Current Progress ---")]
    public int currentDayIndex = 0;   
    public int currentStepIndex = 0;  
    
    public int totalGameScore = 0; 

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (CharacterSelectManager.selectedStoryIndex != -1)
        {
            selectedCharacterIndex = CharacterSelectManager.selectedStoryIndex;
        }
        Debug.Log($"[QuestManager] Personaggio registrato: Indice {selectedCharacterIndex}");

        LoadDayData(currentDayIndex);
        NotifyDayChanged();
    }

    private void NotifyDayChanged()
    {
        OnDayChanged?.Invoke(currentDayIndex);
    }

    private void LoadDayData(int dayIndex)
    {
        if (dayIndex >= allDays.Length) return;

        if (musicPlayer != null && allDays[dayIndex].dayMusic != null)
        {
            musicPlayer.clip = allDays[dayIndex].dayMusic;
            musicPlayer.Play();
        }
        
        Debug.Log($"[QuestManager] Day {dayIndex + 1} Loaded: {allDays[dayIndex].dayName}");
    }

    public MissionStep GetActiveStepForNPC(string npcID)
    {
        if (currentDayIndex >= allDays.Length) return null;
        DailyMission currentDay = allDays[currentDayIndex];
        if (currentStepIndex >= currentDay.steps.Length) return null;

        MissionStep step = currentDay.steps[currentStepIndex];
        if (step.targetNpcID == npcID) return step;

        return null;
    }

    public void AdvanceQuest()
    {
        currentStepIndex++;
        if (currentStepIndex >= allDays[currentDayIndex].steps.Length)
        {
            FinishDay();
        }
    }

    
    private void FinishDay()
    {
        string sceneToLoad = allDays[currentDayIndex].minigameSceneName;
        
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            string entrySpawnID = "";

            if (currentDayIndex == 3) entrySpawnID = entrySpawnDay4;
            else if (currentDayIndex == 5) entrySpawnID = entrySpawnDay6;

            if (!string.IsNullOrEmpty(entrySpawnID) && GameManager.Instance != null)
            {
                Debug.Log($"[QuestManager] Entering Minigame. Setting SpawnPoint to: {entrySpawnID}");
                GameManager.Instance.nextSpawnPoint = entrySpawnID;
            }

            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            StartNextDay();
        }
    }

    
    public void ReturnFromMinigame(int finalScore)
    {
        Debug.Log($"Sono tornato dal minigioco con score: {finalScore}");

        totalGameScore += finalScore;

        float wellbeingReward = 10f + ((float)finalScore / 100f * 10f);

        if (StatusManager.Instance != null)
        {
            StatusManager.Instance.AddWellbeing(wellbeingReward, "MinigameReward");
        }

        string sceneToReturnTo = defaultTownScene;
        string targetSpawnPointID = ""; 

        // Ritorno Giorno 2
        if (currentDayIndex == 1) 
        {
            sceneToReturnTo = townSceneAfterDay2;
            targetSpawnPointID = spawnPointAfterDay2; 
        }
        // Ritorno Giorno 4
        else if (currentDayIndex == 3) 
        {
            sceneToReturnTo = townSceneAfterDay4;
            targetSpawnPointID = spawnPointAfterDay4; 
        }
        // Ritorno Giorno 6
        else if (currentDayIndex == 5) 
        {
            sceneToReturnTo = townSceneAfterDay6;
            targetSpawnPointID = spawnPointAfterDay6; 
        }

        // Imposta Spawn Point di RITORNO
        if (GameManager.Instance != null && !string.IsNullOrEmpty(targetSpawnPointID))
        {
            Debug.Log($"[QuestManager] Returning to Town. Setting SpawnPoint to: {targetSpawnPointID}");
            GameManager.Instance.nextSpawnPoint = targetSpawnPointID;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // 1. Assicuriamoci che l'oggetto sia attivo
            player.SetActive(true);

            // 2. Riattiva lo script Player_Controller
            var controller = player.GetComponent<Player_Controller>();
            if (controller != null) 
            {
                controller.enabled = true;
            }

            // 3. Sblocca la fisica (se era kinematic nel minigioco)
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) 
            {
                rb.isKinematic = false;
                rb.linearVelocity = Vector2.zero;
            }
            
            Debug.Log("[QuestManager] Player resettato e riattivato per il ritorno alla storia.");
        }
        

        StartNextDay();
        SceneManager.LoadScene(sceneToReturnTo);
    }

    private void StartNextDay()
    {
        currentDayIndex++;
        currentStepIndex = 0;

        if (currentDayIndex < allDays.Length)
        {
            LoadDayData(currentDayIndex);
            NotifyDayChanged();
        }
        else
        {
            Debug.Log("GAME OVER");
        }
    }
}