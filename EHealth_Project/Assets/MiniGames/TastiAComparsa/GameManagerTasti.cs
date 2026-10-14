using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class GameManagerTasti : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject panelIntro;
    public GameObject panelGameOver;
    public GameObject panelWin;

    [Header("UI Buttons")]
    public Button startButton;
    public Button retryButton;
    public Button exitButton;

    [Header("Timer")]
    public TextMeshProUGUI timerText;
    public float gameDuration = 30f;

    private float timer;
    private bool gameActive = false;

    void Start()
    {
        panelIntro.SetActive(true);
        panelGameOver.SetActive(false);
        panelWin.SetActive(false);
        timerText.gameObject.SetActive(false);

        startButton.onClick.AddListener(StartGame);
        retryButton.onClick.AddListener(RetryGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    void Update()
    {
        if (!gameActive) return;

        timer -= Time.deltaTime;
        timerText.text = Mathf.Ceil(timer).ToString() + "s";

        if (timer <= 0f)
        {
            Win();
            return;
        }

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.wasPressedThisFrame ||
                Keyboard.current.aKey.wasPressedThisFrame ||
                Keyboard.current.sKey.wasPressedThisFrame ||
                Keyboard.current.dKey.wasPressedThisFrame)
            {
                GameOver();
            }
        }
    }

    void StartGame()
    {
        panelIntro.SetActive(false);
        panelGameOver.SetActive(false);
        panelWin.SetActive(false);

        timer = gameDuration;
        timerText.gameObject.SetActive(true);
        gameActive = true;
    }

    void GameOver()
    {
        gameActive = false;
        timerText.gameObject.SetActive(false);
        panelGameOver.SetActive(true);
    }

    void Win()
    {
        gameActive = false;
        timerText.gameObject.SetActive(false);
        panelWin.SetActive(true);
    }

    void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ? FUNZIONE PER LO SPAWNER
    public bool IsGameActive()
    {
        return gameActive;
    }
}
