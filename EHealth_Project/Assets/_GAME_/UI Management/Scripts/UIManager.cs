using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseUI;
    public GameObject pauseButton;

    [Header("Scene")]
    public string sceneToLoadOnExit = "MainMenu";

    public void OnGameResumePress()
    {
        Debug.Log("Resume premuto");
        pauseUI.SetActive(false);
        pauseButton.SetActive(true);    
    }

    public void OnGameExitPress()
    {
        Debug.Log("Exit premuto");
        SceneManager.LoadScene(sceneToLoadOnExit);
    }
    
    public void OnEnterPausePress()
    {
        pauseUI.SetActive(true);
        pauseButton.SetActive(false);
    }
}