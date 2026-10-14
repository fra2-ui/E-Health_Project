using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; 

public class TuningUIController : MonoBehaviour
{
    public enum CanvasType { StartMenu, EndMenu }

    [Header("Configurazione Tipo")]
    public CanvasType canvasType; 
    
    [Header("Pannello Principale")]
    public GameObject contentPanel; 

    [Header("--- SE E' START MENU ---")]
    public Button startButton;     
    public GameObject gameArea;     

    [Header("--- SE E' END MENU ---")]
    public Button retryButton;      
    public Button exitButton;         
    public string sceneToExitTo;
    
    [Header("Output Punteggio")]
    public TMP_Text scoreText;

    void Start()
    {
        if (canvasType == CanvasType.StartMenu) SetupStartMenu();
        else if (canvasType == CanvasType.EndMenu) SetupEndMenu();
    }

    void OnDestroy()
    {
        if (canvasType == CanvasType.EndMenu && TuningGameManager.HasInstance)
        {
            TuningGameManager.Instance.OnVictory -= HandleVictory;
        }
    }

    void SetupStartMenu()
    {
        if (contentPanel != null) contentPanel.SetActive(true);
        if (gameArea != null) gameArea.SetActive(false);
        if (TuningGameManager.HasInstance) TuningGameManager.Instance.SetGameActive(false);

        if (startButton != null)
        {
            startButton.onClick.AddListener(() => {
                if (contentPanel != null) contentPanel.SetActive(false);
                if (gameArea != null) gameArea.SetActive(true);
                if (TuningGameManager.HasInstance) TuningGameManager.Instance.SetGameActive(true);
            });
        }
    }

    void SetupEndMenu()
    {
        if (contentPanel != null) contentPanel.SetActive(false);
        if (TuningGameManager.HasInstance) TuningGameManager.Instance.OnVictory += HandleVictory;

        if (retryButton != null)
        {
            retryButton.onClick.AddListener(() => {
                Time.timeScale = 1f; 
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
            });
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(() => {
                Time.timeScale = 1f; 
                
                // Recupera il punteggio
                int scoreToSend = 0;
                if (TuningGameManager.HasInstance) 
                    scoreToSend = TuningGameManager.Instance.finalScore;

                // Lo invia al QuestManager
                if (QuestManager.Instance != null)
                {
                    QuestManager.Instance.ReturnFromMinigame(scoreToSend);
                }
                else
                {
                    // Fallback
                    if (!string.IsNullOrEmpty(sceneToExitTo)) SceneManager.LoadScene(sceneToExitTo);
                }
            });
        }
    }

    void HandleVictory()
    {
        if (contentPanel != null) contentPanel.SetActive(true);

        // Mostra il punteggio
        if (scoreText != null && TuningGameManager.HasInstance)
        {
            int score = TuningGameManager.Instance.finalScore;
            scoreText.text = $"Score: {score}/100";
        }
    }
}