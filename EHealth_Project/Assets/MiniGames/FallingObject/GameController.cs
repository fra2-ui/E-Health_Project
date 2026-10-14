using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Game Settings")]
    [Tooltip("Quanti secondi devi sopravvivere per fare 100 punti?")]
    [SerializeField] private float targetTime = 60f; 
    
    [SerializeField] private Player_Controller playerController;
    [SerializeField] private ObjectFallController objectFallController; 

    [Header("UI References")]
    public GameObject StartCanvas;    // Menu Iniziale
    public GameObject GameOverCanvas; // Menu Finale
    
    [Header("UI Elements")]
    public Button startButton;      // Bottone Play (Start Menu)
    public Button retryButton;      // Bottone Riprova (End Menu)
    public Button exitButton;       // Bottone Esci (End Menu)
    
    public TMP_Text timerText;      // (Opzionale) Timer in gioco
    public TMP_Text finalScoreText; // Testo "Score: X/100" nel menu finale
    
    [Header("Navigation")]
    public string sceneToExitTo;    // Nome scena dove tornare (se manca QuestManager)

    // STATO GIOCO
    private bool gameStarted = false;
    private float _startTime = 0f;
    private int finalScore = 0;
    
    // Per il Soft Reset (Retry)
    private Vector3 initialPlayerPosition; 

    private void Awake()
    {
        Time.timeScale = 1f;
        SetupStartMenu();
    }

    
    void SetupStartMenu()
    {
        // 0. Cursore libero
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 1. Cerca Player se manca
        if (playerController == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerController = p.GetComponent<Player_Controller>();
        }

        // 2. Mostra Start, Nascondi End
        if (StartCanvas != null) StartCanvas.SetActive(true);
        if (GameOverCanvas != null) GameOverCanvas.SetActive(false);

        // 3. CONGELA IL PLAYER
        FreezePlayer(true);

        if (objectFallController != null) objectFallController.StopSpawning();

        // 4. Configura bottone Start
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(StartGame);
        }
    }

        public void StartGame()
    {
        if (gameStarted) return;
        gameStarted = true;
        Time.timeScale = 1f;

        // Nascondi UI Start
        if (StartCanvas != null) StartCanvas.SetActive(false);

        // Sicurezza: cerca Player
        if (playerController == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerController = p.GetComponent<Player_Controller>();
        }

        if (playerController != null)
        {
            // Salva posizione iniziale per il Retry
            initialPlayerPosition = playerController.transform.position;

            // SCONGELA IL PLAYER (Attiva controlli e fisica)
            FreezePlayer(false);

            // Collega evento morte
            playerController.PlayerDied -= WhenPlayerDies;
            playerController.PlayerDied += WhenPlayerDies;
        }

        // Avvia Spawner
        if (objectFallController != null) objectFallController.StartSpawning();

        _startTime = Time.time;
    }

    
    void Update()
    {
        if (gameStarted && timerText != null && playerController != null)
        {
            float timeElapsed = Time.time - _startTime;
            timerText.text = $"{Mathf.Round(timeElapsed)}s";
        }
    }

    
    void WhenPlayerDies()
    {
        if (!gameStarted) return;

        Debug.Log("Game Over! Congelamento Player.");

        // 1. FREEZE DEL GIOCO
        Time.timeScale = 0f;
        
        // 2. Congela Player
        FreezePlayer(true);
        
        // 3. UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 4. Calcola punteggio
        float timeLasted = Time.unscaledTime - (Time.realtimeSinceStartup - Time.time + _startTime);
        CalculateScore(Time.time - _startTime);

        // 5. Mostra UI Finale
        if (GameOverCanvas != null) GameOverCanvas.SetActive(true);
        if (finalScoreText != null) finalScoreText.text = $"Score: {finalScore}/100";

        // 6. Stop Spawner
        if (objectFallController != null) objectFallController.StopSpawning();

        // 7. Configura pulsanti finali
        SetupEndButtons();
    }

    
    void FreezePlayer(bool isFrozen)
    {
        if (playerController == null) return;

        // Blocca Script
        playerController.enabled = !isFrozen;

        // Blocca Fisica
        Rigidbody2D rb = playerController.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (isFrozen)
            {
                rb.linearVelocity = Vector2.zero;
                rb.isKinematic = true; 
            }
            else
            {
                rb.isKinematic = false;
            }
        }

        // Blocca Animazioni
        Animator anim = playerController.GetComponent<Animator>();
        if (anim != null) anim.enabled = !isFrozen;
    }

    void CalculateScore(float timeLasted)
    {
        float rawScore = (timeLasted / targetTime) * 100f;
        finalScore = Mathf.FloorToInt(Mathf.Clamp(rawScore, 0f, 100f));
    }

    void SetupEndButtons()
    {
        // RETRY
        if (retryButton != null)
        {
            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(ResetGame);
        }

        // EXIT -> Chiama QuestManager
        if (exitButton != null)
        {
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(() => {
                Time.timeScale = 1f; 

                
                FreezePlayer(false);

                if (QuestManager.Instance != null)
                {
                    QuestManager.Instance.ReturnFromMinigame(finalScore);
                }
                else
                {
                    if (!string.IsNullOrEmpty(sceneToExitTo)) SceneManager.LoadScene(sceneToExitTo);
                }
            });
        }
    }

    
    void ResetGame()
    {
        Debug.Log("Resettando il gioco...");
        Time.timeScale = 1f;

        if (GameOverCanvas != null) GameOverCanvas.SetActive(false);

        // Reset Player
        if (playerController != null)
        {
            playerController.transform.position = initialPlayerPosition;
            FreezePlayer(false); // Riattiva
            
            // Ricollega evento morte
            playerController.PlayerDied -= WhenPlayerDies; 
            playerController.PlayerDied += WhenPlayerDies;
        }

        // Reset Spawner e Timer
        if (objectFallController != null)
        {
            
            objectFallController.StopSpawning();
            objectFallController.StartSpawning();
        }

        _startTime = Time.time;
    }
}