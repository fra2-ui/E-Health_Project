using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniggameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [Tooltip("Secondi necessari per fare 100 punti")]
    [SerializeField] private float targetTime = 60f; 

    [Header("References")]
    [SerializeField] private GameObject player;               
    [SerializeField] private GameObject obstacleSpawner;      

    [Header("Canvas UI")]
    public GameObject GameOverCanvas;  // Canvas finale
    public GameObject StartCanvas;     // Canvas iniziale
    
    [Header("UI Elements")]
    public Button startButton;      // Bottone Play
    public Button retryButton;      // Bottone Riprova
    public Button exitButton;       // Bottone Esci

    public TMP_Text timerText;      // Timer in gioco
    public TMP_Text finalScoreText; // Score finale
    
    [Header("Navigation")]
    public string sceneToExitTo;    // Fallback scene

    private bool gameStarted = false;
    private float _startTime = 0f;
    private int finalScore = 0;

    private void Awake()
    {
        
        Time.timeScale = 1f;
        SetupStartMenu();
    }

    
    void SetupStartMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 1. CERCA IL PLAYER (Se manca)
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // 2. DISABILITA IL PLAYER
        if (player != null)
        {
            
            var controller = player.GetComponent<Player_Controller>();
            if (controller != null) controller.enabled = false;
            
            
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }

        // 3. Gestione Pannelli
        if (GameOverCanvas != null) GameOverCanvas.SetActive(false);
        if (StartCanvas != null) StartCanvas.SetActive(true);

        // 4. Disattiva lo spawner
        if (obstacleSpawner != null) obstacleSpawner.SetActive(false);

        // 5. Configura Start Button
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

        
        if (StartCanvas != null) StartCanvas.SetActive(false);

        
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("ERRORE: Player non trovato!");
                return;
            }
        }

        
        var playerScript = player.GetComponent<Player_Controller>(); 
        if (playerScript != null)
        {
            playerScript.enabled = true; // Ora il player può muoversi

            // Ricollegamento eventi
            playerScript.PlayerDied -= WhenPlayerDies; 
            playerScript.PlayerDied += WhenPlayerDies;
            
            Debug.Log("Player attivato e collegato!");
        }

        // Attiva Spawner
        if (obstacleSpawner != null) obstacleSpawner.SetActive(true);

        _startTime = Time.time;
    }

    
    void Update()
    {
        if (gameStarted && timerText != null && player != null)
        {
            float timeElapsed = Time.time - _startTime;
            timerText.text = $"{Mathf.Round(timeElapsed)}s";
        }
    }

    
    public void WhenPlayerDies()
    {
        if (!gameStarted) return;
        
        Debug.Log("Game Over!");

        // 1. FREEZE DEL GIOCO
        Time.timeScale = 0f; // Ferma tutto!
        
        // 2. DISABILITA CONTROLLI
        if (player != null)
        {
            var controller = player.GetComponent<Player_Controller>();
            if (controller != null) controller.enabled = false;
        }

        // 3. SBLOCCO CURSORE
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        
        float timeLasted = Time.unscaledTime - _startTime; 
        
        float finalTime = Time.time - _startTime;
        CalculateScore(finalTime);

        // 5. UI Game Over
        if (GameOverCanvas != null) GameOverCanvas.SetActive(true);
        if (finalScoreText != null) finalScoreText.text = $"Score: {finalScore}/100";

        // 6. Configura bottoni
        SetupEndButtons();
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
            retryButton.onClick.AddListener(() => {
                
                Time.timeScale = 1f; 
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }

        // EXIT
        if (exitButton != null)
        {
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(() => {
                Time.timeScale = 1f; // Ripristina il tempo

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
}